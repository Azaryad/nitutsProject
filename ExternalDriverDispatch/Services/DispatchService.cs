using System;
using System.Collections.Generic;
using System.Linq;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// Domain orchestration for the end-to-end dispatch flow. The UI calls these methods; only
    /// the service *implementations* behind the injected interfaces ever touch the network. The
    /// three services compose here into one coherent algorithm:
    ///
    ///   EnrichTrip   -> Maps fills trip.distanceKm / estimatedDurationMinutes        [Service 1]
    ///   RankEligible -> eligibility (incl. long-distance gate) + AI ranking          [Service 2]
    ///   SendOffer    -> AI composes text + WhatsApp sends + Offer + outbound Message  [Service 2+3]
    ///   HandleReply  -> (simulated) inbound Message + AI interpret -> Offer state     [Service 3+2]
    ///   Forward      -> re-rank excluding contacted, send next, or escalate
    ///
    /// All DB writes run with success popups suppressed; the board narrates via <see cref="Log"/>.
    /// </summary>
    public class DispatchService
    {
        public const double LongDistanceThresholdKm = 100.0;

        /// <summary>How long before pickup an unassigned trip is escalated to manual assignment.</summary>
        public const double DeadlineHoursBeforePickup = 6.0;

        /// <summary>Narrative sink — the board points this at its activity log.</summary>
        public Action<string> Log = _ => { };

        private readonly IDriveInfoProvider maps = ServiceFactory.DriveInfo();
        private readonly IDriverRanker ranker = ServiceFactory.Ranker();
        private readonly IMessageComposer composer = ServiceFactory.Composer();
        private readonly IReplyInterpreter interpreter = ServiceFactory.Interpreter();
        private readonly IRestrictionParser restrictionParser = ServiceFactory.RestrictionParser();
        private readonly IMessageChannel channel = ServiceFactory.Channel();

        // ---- Service 1: enrich a trip with Maps numbers (called right after region assignment) ----
        public void EnrichTrip(Trip t)
        {
            DriveInfo info = maps.GetDriveInfo(
                t.getPickupAddress() + ", " + t.getPickupCity(),
                t.getDropoffAddress() + ", " + t.getDropoffCity(),
                t.getPickupTime());

            t.setDistanceKm(info.DistanceKm);
            t.setEstimatedDurationMinutes(info.DurationMinutes);
            Quiet(() => t.updateTrip());

            string longTag = info.DistanceKm >= LongDistanceThresholdKm ? "  (long distance)" : "";
            Log($"[Maps] {t.getPickupCity()} → {t.getDropoffCity()}: {info.DistanceKm:0.#} km, ~{info.DurationMinutes} min{longTag}");
        }

        // ---- Service 2: eligibility (+ long-distance gate) then AI ranking ----
        public IReadOnlyList<RankedDriver> RankEligible(Trip t)
        {
            Region region = t.getRegion();
            if (region == null) return new List<RankedDriver>();

            // active + enough capacity
            List<ExternalDriver> eligible = region.getEligibleDrivers(t).ToList();

            // long-distance gate uses the Maps distance: only long-distance drivers stay eligible
            if (t.getDistanceKm() >= LongDistanceThresholdKm)
                eligible = eligible.Where(d => d.getWorksLongDistance()).ToList();

            // exclude drivers already contacted for this trip (offer history)
            HashSet<int> contacted = new HashSet<int>(t.getOffers().Select(o => o.getDriver().getId()));
            eligible = eligible.Where(d => !contacted.Contains(d.getId())).ToList();

            return ranker.Rank(t, eligible);
        }

        // ---- Service 2 (compose) + Service 3 (send) + create Offer + outbound Message ----
        public Offer SendOffer(Trip t, RankedDriver rd)
        {
            Offer created = null;
            Quiet(() =>
            {
                int offerId = Offer.getNextOfferId();
                Offer o = new Offer(offerId, t, rd.Driver, DateTime.Now, DateTime.Now.AddHours(1),
                    OfferStatus.pending, null, null, rd.Rank, true);
                o.setRankReason(rd.Reason);
                o.updateOffer();        // persist the AI rank reason
                t.offer();              // assigned_to_region -> offered
                t.updateOfferCount();   // one more outreach attempt for this trip (attractiveness metric)

                string url = o.generateApprovalUrl();
                string text = composer.OfferMessage(rd.Driver, t, url);

                // Ordered substitutions for the approved WhatsApp template (must match {{1}}..{{7}} in
                // Twilio.ContentSid). When no template is configured the channel sends `text` as a plain
                // Body instead — same audit row either way.
                var templateVars = new List<string>
                {
                    rd.Driver.getName(),                                       // {{1}} driver
                    t.getPickupAddress() + ", " + t.getPickupCity(),           // {{2}} pickup
                    t.getPickupTime().ToString("yyyy-MM-dd HH:mm"),            // {{3}} time
                    t.getDropoffAddress() + ", " + t.getDropoffCity(),         // {{4}} destination
                    t.getNumPassengers().ToString(),                           // {{5}} passengers
                    t.getPriceToDriver().ToString("0.##") + " ILS",            // {{6}} pay
                    url                                                        // {{7}} accept/decline link
                };
                string waId = channel.SendTemplate(rd.Driver.getPhone(), Config.TwilioContentSid, templateVars, text);

                int mId = Message.getNextMessageId();
                new Message(mId, rd.Driver, o, MessageDirection.outbound, waId, text, DateTime.Now, true);

                Log($"[WhatsApp →] {rd.Driver.getName()} ({rd.Driver.getPhone()}) — rank #{rd.Rank}: {rd.Reason}");
                Log($"   \"{text}\"");
                created = o;
            });
            return created;
        }

        // ---- Service 3 inbound (simulated) + Service 2 interpret -> drives the Offer state machine ----
        public ReplyIntent HandleDriverReply(Offer o, string replyText)
        {
            Quiet(() =>
            {
                int mId = Message.getNextMessageId();
                new Message(mId, o.getDriver(), o, MessageDirection.inbound, null, replyText, DateTime.Now, true);
                o.setDriverReplyText(replyText);
            });
            Log($"[WhatsApp ←] {o.getDriver().getName()}: \"{replyText}\"");

            // optional: a free-text availability change ("no nights", "on vacation", ...) updates the driver
            DriverAvailabilityUpdate upd = restrictionParser.Parse(replyText);
            if (upd != null) ApplyRestriction(o.getDriver(), upd);

            ReplyIntent intent = interpreter.Interpret(o.getDriver().getName(), replyText);
            Quiet(() => { o.setAiInterpretation(intent.ToString().ToLowerInvariant()); o.updateOffer(); });
            Log($"[AI] interpreted reply as: {intent}");

            switch (intent)
            {
                case ReplyIntent.Yes:
                    Quiet(() => o.markPendingApproval());
                    Log("   → YES (intent): awaiting approval-link click.");
                    break;
                case ReplyIntent.No:
                    Quiet(() => o.reject());
                    Log("   → NO: offer rejected; forwarding to next driver.");
                    break;
                default:
                    Log("   → AMBIGUOUS: a clarifying question would be sent (no status change).");
                    break;
            }
            return intent;
        }

        // ---- re-rank excluding contacted drivers, send to next, or escalate ----
        public Offer Forward(Trip t)
        {
            IReadOnlyList<RankedDriver> ranked = RankEligible(t);
            if (ranked.Count == 0)
            {
                Quiet(() => t.flagManualAssignment());
                Log("[Forward] No eligible drivers remain → trip 'Manual Assignment' (dispatcher notified).");
                return null;
            }
            Log($"[Forward] {ranked.Count} driver(s) remain; offering the next-ranked.");
            return SendOffer(t, ranked[0]);
        }

        /// <summary>
        /// Trip-level deadline (driven by the real background timer): any trip still
        /// 'assigned_to_region' or 'offered' whose pickup is within 6 hours (or already past) and
        /// has no driver assigned is escalated to 'manual_assignment'; any still-pending offer on it
        /// is cancelled. Returns the number of trips escalated. Pure domain (no external service),
        /// so it is safe to call from the UI-thread deadline timer.
        /// </summary>
        public static int EscalateOverdueTrips()
        {
            if (Program.Trips == null) return 0;   // not loaded yet (before login)
            int escalated = 0;
            DateTime now = DateTime.Now;
            SQL_CON.SuppressSuccessMessages = true;
            try
            {
                foreach (Trip t in Program.Trips.ToList())
                {
                    if (t.getStatus() != TripStatus.assigned_to_region && t.getStatus() != TripStatus.offered)
                        continue;
                    if (now < t.getPickupTime().AddHours(-DeadlineHoursBeforePickup))
                        continue;

                    // cancel any still-pending offer before flagging the trip
                    foreach (Offer o in t.getOffers().ToList())
                        if (o.getStatus() == OfferStatus.pending || o.getStatus() == OfferStatus.pending_approval)
                            o.cancel();

                    if (t.flagManualAssignment()) escalated++;   // sets manual_assignment + notifies dispatcher
                }
            }
            finally { SQL_CON.SuppressSuccessMessages = false; }
            return escalated;
        }

        private void ApplyRestriction(ExternalDriver d, DriverAvailabilityUpdate u)
        {
            if (u.WorksNights.HasValue) d.setWorksNights(u.WorksNights.Value);
            if (u.WorksShabbat.HasValue) d.setWorksShabbat(u.WorksShabbat.Value);
            if (u.WorksLongDistance.HasValue) d.setWorksLongDistance(u.WorksLongDistance.Value);
            if (u.Active.HasValue) d.setActive(u.Active.Value);
            Quiet(() => d.updateExternalDriver());
            Log($"[AI] availability update for {d.getName()}: \"{u.Note}\"");
        }

        // run a DB action with the per-write "success" popup suppressed
        private void Quiet(Action act)
        {
            SQL_CON.SuppressSuccessMessages = true;
            try { act(); }
            finally { SQL_CON.SuppressSuccessMessages = false; }
        }
    }
}
