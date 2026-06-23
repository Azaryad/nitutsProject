using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ExternalDriverDispatch
{
    // =====================================================================
    // Service 2 — the "brain." Four single-responsibility roles, each an
    // interface so the responsibilities stay clean. Each has a deterministic
    // offline fallback (a real algorithm/template/keyword-match), so the
    // intelligence degrades gracefully and the demo never needs a key.
    // =====================================================================

    /// <summary>Interpreted intent of a free-text driver reply.</summary>
    public enum ReplyIntent { Yes, No, Ambiguous }

    /// <summary>A driver together with their assigned rank and a one-line justification.</summary>
    public record RankedDriver(ExternalDriver Driver, int Rank, string Reason);

    /// <summary>An availability change parsed from a free-text driver message (null fields = no change).</summary>
    public class DriverAvailabilityUpdate
    {
        public bool? WorksNights;
        public bool? WorksShabbat;
        public bool? WorksLongDistance;
        public bool? Active;
        public string Note;
    }

    /// <summary>Role: order eligible drivers best-first, with a reason per driver.</summary>
    public interface IDriverRanker
    {
        IReadOnlyList<RankedDriver> Rank(Trip trip, IReadOnlyList<ExternalDriver> eligible);
    }

    /// <summary>Role: compose the WhatsApp offer text for a driver/trip/approval-link.</summary>
    public interface IMessageComposer
    {
        string OfferMessage(ExternalDriver driver, Trip trip, string link);
    }

    /// <summary>Role: interpret a free-text reply as yes / no / ambiguous.</summary>
    public interface IReplyInterpreter
    {
        ReplyIntent Interpret(string driverName, string replyText);
    }

    /// <summary>Role: parse an availability change out of free text (null if none found).</summary>
    public interface IRestrictionParser
    {
        DriverAvailabilityUpdate Parse(string freeText);
    }

    // =====================================================================
    // Offline fallbacks — each a genuine deterministic algorithm.
    // =====================================================================

    /// <summary>
    /// Fallback ranker — proximity sort: drivers whose home city equals the pickup city first,
    /// then drivers in the region's city, then everyone else; ties broken by current load
    /// (lighter first) then id. A real ranking algorithm, just without the AI's judgement.
    /// </summary>
    public class ProximityDriverRanker : IDriverRanker
    {
        public IReadOnlyList<RankedDriver> Rank(Trip trip, IReadOnlyList<ExternalDriver> eligible)
        {
            var ordered = eligible
                .OrderBy(d => d.getVehicleType() == trip.getVehicleType() ? 0 : 1)
                .ThenBy(d => Proximity(d, trip))
                .ThenBy(d => Load(d))
                .ThenBy(d => d.getId())
                .ToList();

            var result = new List<RankedDriver>();
            int rank = 1;
            foreach (var d in ordered)
                result.Add(new RankedDriver(d, rank++, Reason(d, trip)));
            return result;
        }

        public static int Proximity(ExternalDriver d, Trip t)
        {
            if (d.getHomeCity() == t.getPickupCity()) return 0;
            if (t.getRegion() != null && d.getHomeCity() == t.getRegion().getCity()) return 1;
            return 2;
        }

        public static int Load(ExternalDriver d)
        {
            return Program.Offers.Count(o => o.getDriver() == d &&
                (o.getStatus() == OfferStatus.pending || o.getStatus() == OfferStatus.pending_approval));
        }

        private static string Reason(ExternalDriver d, Trip t)
        {
            string vehicle = d.getVehicleType() == t.getVehicleType() ? "vehicle match" : "vehicle substitute";
            string prox = Proximity(d, t) == 0 ? "home city = pickup"
                        : Proximity(d, t) == 1 ? "home city in region"
                        : "out-of-area";
            return vehicle + ", " + prox + ", load " + Load(d);
        }
    }

    /// <summary>Fallback composer — a simple Hebrew/English-neutral template via string interpolation.</summary>
    public class TemplateMessageComposer : IMessageComposer
    {
        public string OfferMessage(ExternalDriver driver, Trip trip, string link)
        {
            return $"Hi {driver.getName()}, new transfer on {trip.getPickupTime():dd/MM} at " +
                   $"{trip.getPickupTime():HH:mm} from {trip.getPickupCity()} to {trip.getDropoffCity()}, " +
                   $"{trip.getNumPassengers()} pax, {trip.getPriceToDriver():0} ILS. Approve here: {link}";
        }
    }

    /// <summary>Fallback interpreter — keyword match on common accept/decline phrases.</summary>
    public class KeywordReplyInterpreter : IReplyInterpreter
    {
        private static readonly string[] yes = { "yes", "ok", "okay", "sure", "take", "confirm", "accept", "yep", "yeah", "deal" };
        private static readonly string[] no = { "no", "can't", "cant", "cannot", "busy", "decline", "pass", "not ", "unavailable", "won't", "wont" };

        public ReplyIntent Interpret(string driverName, string replyText)
        {
            if (string.IsNullOrWhiteSpace(replyText)) return ReplyIntent.Ambiguous;
            string t = replyText.ToLowerInvariant();
            bool hasNo = no.Any(k => t.Contains(k));
            bool hasYes = yes.Any(k => t.Contains(k));
            if (hasNo && !hasYes) return ReplyIntent.No;
            if (hasYes && !hasNo) return ReplyIntent.Yes;
            return ReplyIntent.Ambiguous;
        }
    }

    /// <summary>Fallback restriction parser — keyword match on availability statements.</summary>
    public class KeywordRestrictionParser : IRestrictionParser
    {
        public DriverAvailabilityUpdate Parse(string freeText)
        {
            if (string.IsNullOrWhiteSpace(freeText)) return null;
            string t = freeText.ToLowerInvariant();
            var u = new DriverAvailabilityUpdate();
            bool found = false;

            if (t.Contains("no nights") || t.Contains("not at night") || t.Contains("no night"))
            { u.WorksNights = false; found = true; }
            if (t.Contains("no shabbat") || t.Contains("no saturday") || t.Contains("not on saturday"))
            { u.WorksShabbat = false; found = true; }
            if (t.Contains("no long distance") || t.Contains("local only") || t.Contains("short only"))
            { u.WorksLongDistance = false; found = true; }
            if (t.Contains("on vacation") || t.Contains("unavailable") || t.Contains("not working") || t.Contains("inactive"))
            { u.Active = false; found = true; }

            if (!found) return null;
            u.Note = freeText.Trim();
            return u;
        }
    }

    // =====================================================================
    // Real implementation — Anthropic Messages API. Implements all four roles;
    // every role wraps its call in try/catch and delegates to the matching
    // fallback on any failure. Off by default (Ai.Enabled=false) until a key is configured.
    // =====================================================================
    public class ClaudeAiService : IDriverRanker, IMessageComposer, IReplyInterpreter, IRestrictionParser
    {
        private static readonly HttpClient http = new HttpClient();
        private readonly string apiKey;
        private readonly string model;

        // fallbacks to degrade to
        private readonly ProximityDriverRanker rankFallback = new ProximityDriverRanker();
        private readonly TemplateMessageComposer composeFallback = new TemplateMessageComposer();
        private readonly KeywordReplyInterpreter interpretFallback = new KeywordReplyInterpreter();
        private readonly KeywordRestrictionParser restrictionFallback = new KeywordRestrictionParser();

        public ClaudeAiService(string apiKey, string model)
        {
            this.apiKey = apiKey;
            this.model = model;
        }

        public IReadOnlyList<RankedDriver> Rank(Trip trip, IReadOnlyList<ExternalDriver> eligible)
        {
            try
            {
                if (eligible.Count == 0) return new List<RankedDriver>();
                var sb = new StringBuilder();
                sb.AppendLine($"Trip: {trip.getPickupCity()} -> {trip.getDropoffCity()}, " +
                              $"{trip.getNumPassengers()} pax, vehicle {VehicleTypeHelper.ToDisplay(trip.getVehicleType())}, " +
                              $"distance {trip.getDistanceKm():0.#} km.");
                sb.AppendLine("Eligible drivers (id, name, vehicle, home city):");
                foreach (var d in eligible)
                    sb.AppendLine($"- {d.getId()}, {d.getName()}, {VehicleTypeHelper.ToDisplay(d.getVehicleType())}, {d.getHomeCity()}");
                sb.AppendLine("Return the driver ids best-first as a comma-separated list, ids only.");

                string reply = Call("You rank transfer drivers for a trip. Prefer vehicle match, proximity, and balanced workload.", sb.ToString());
                var ids = reply.Split(new[] { ',', ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(s => int.TryParse(s.Trim(), out int v) ? v : -1)
                               .Where(v => v > 0).ToList();

                var byId = eligible.ToDictionary(d => d.getId());
                var result = new List<RankedDriver>();
                int rank = 1;
                foreach (int id in ids)
                    if (byId.TryGetValue(id, out var d)) { result.Add(new RankedDriver(d, rank++, "AI-ranked")); byId.Remove(id); }
                foreach (var d in byId.Values) result.Add(new RankedDriver(d, rank++, "AI-ranked (appended)"));
                return result.Count > 0 ? result : rankFallback.Rank(trip, eligible);
            }
            catch { return rankFallback.Rank(trip, eligible); }
        }

        public string OfferMessage(ExternalDriver driver, Trip trip, string link)
        {
            try
            {
                string prompt = $"Write a short, friendly WhatsApp offer to driver {driver.getName()} for a transfer " +
                                $"on {trip.getPickupTime():dd/MM HH:mm} from {trip.getPickupCity()} to {trip.getDropoffCity()}, " +
                                $"{trip.getNumPassengers()} passengers, {trip.getPriceToDriver():0} ILS. " +
                                $"End with this approval link exactly: {link}";
                string reply = Call("You write concise driver dispatch messages.", prompt);
                return string.IsNullOrWhiteSpace(reply) ? composeFallback.OfferMessage(driver, trip, link) : reply.Trim();
            }
            catch { return composeFallback.OfferMessage(driver, trip, link); }
        }

        public ReplyIntent Interpret(string driverName, string replyText)
        {
            try
            {
                string reply = Call(
                    "Classify a driver's reply to a trip offer as exactly one word: yes, no, or ambiguous.",
                    $"Driver {driverName} replied: \"{replyText}\". One word only.");
                string r = reply.Trim().ToLowerInvariant();
                if (r.StartsWith("yes")) return ReplyIntent.Yes;
                if (r.StartsWith("no")) return ReplyIntent.No;
                if (r.StartsWith("ambig")) return ReplyIntent.Ambiguous;
                return interpretFallback.Interpret(driverName, replyText);
            }
            catch { return interpretFallback.Interpret(driverName, replyText); }
        }

        // Restriction parsing delegates to the keyword fallback (kept deterministic).
        public DriverAvailabilityUpdate Parse(string freeText) => restrictionFallback.Parse(freeText);

        /// <summary>One synchronous call to the Anthropic Messages API; returns the text content.</summary>
        private string Call(string system, string user)
        {
            var payload = new
            {
                model = this.model,
                max_tokens = 512,
                system = system,
                messages = new[] { new { role = "user", content = user } }
            };
            var req = new HttpRequestMessage(HttpMethod.Post, "https://api.anthropic.com/v1/messages");
            req.Headers.Add("x-api-key", apiKey);
            req.Headers.Add("anthropic-version", "2023-06-01");
            req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            using HttpResponseMessage resp = http.Send(req);
            using var stream = resp.Content.ReadAsStream();
            using JsonDocument doc = JsonDocument.Parse(stream);
            return doc.RootElement.GetProperty("content")[0].GetProperty("text").GetString() ?? "";
        }
    }
}
