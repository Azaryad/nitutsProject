# Pipeline Prompts — every flow in the system, traced through the real code

> **What this is.** One self-contained "pipeline prompt" per flow. Each block traces the **actual code
> path** — exact classes, methods, stored procedures, services, and state transitions — written so it
> can stand alone: hand any single block to a developer (or an AI) and it fully explains, or could
> regenerate, that flow without the rest of this file.
>
> **Tiers (read this first).** The system is documented *in full*. Each pipeline is tagged:
>
> - 🟦 **OBLIGATED — Use Case.** One of the 6 course-required use cases.
> - 🟦 **OBLIGATED — Infrastructure.** Required by the SAD architecture (`PATTERNS.md`) but **not** a use
>   case (startup/data-load, entity CRUD).
> - 🟧 **EXTRA.** An enhancement we added beyond the assignment (external integrations, AI, the report,
>   settings). Cross-referenced from the UC pipeline it plugs into. The system runs end-to-end with all
>   🟧 services offline; the 🟦 behaviour is identical.
>
> Visual companion: the class diagram + a sequence diagram per flow live in
> [implementation-uml.md](implementation-uml.md).

**Index**

| # | Pipeline | Tier |
|---|---|---|
| 1 | View Open Trips | 🟦 UC |
| 2 | Assign Trip to Region | 🟦 UC |
| 3 | Send Trip Offer (UC17) | 🟦 UC |
| 4 | Respond to Trip Offer (UC07) | 🟦 UC |
| 5 | Forward Offer to Next Driver | 🟦 UC |
| 6 | Update Ride Control (UC08) | 🟦 UC |
| 7 | Application startup & in-memory load | 🟦 Infra |
| 8 | Entity CRUD (Region / Driver / Trip / Offer / Message) | 🟦 Infra |
| 9 | Maps trip enrichment (Service 1) | 🟧 Extra |
| 10 | AI driver ranking (Service 2) | 🟧 Extra |
| 11 | AI reply interpretation + restriction parsing (Service 2) | 🟧 Extra |
| 12 | WhatsApp delivery + approved templates (Service 3) | 🟧 Extra |
| 13 | Real inbound WhatsApp: webhook + ngrok tunnel | 🟧 Extra |
| 14 | Driver Performance report | 🟧 Extra |
| 15 | Settings — per-service live/offline | 🟧 Extra |

---
---

# PART 1 — Course-obligated use-case pipelines 🟦

---

## 1. View Open Trips  · 🟦 OBLIGATED — Use Case  *(entry point)*

**Prompt.** Trace the dispatcher's entry flow: how the open-trips queue is rendered and how selecting a
trip primes the rest of the board. No database round-trip happens here — the queue is read from the
in-memory `Program.Trips` that was loaded once at startup (Pipeline 7).

- **Entry point:** `DispatchBoardPanel` constructor → `loadTrips()`; and `dgvTrips_CellClick`.
- **Preconditions:** dispatcher logged in (`LoginPanel`); `Program.initLists()` has run.
- **Pipeline:**
  1. Constructor calls `UiTheme.Apply(this)`, sets `svc.Log = log`, `refreshRegionCombo()` (fills
     `comboRegion` from `Program.Regions`), then `loadTrips()`.
  2. `loadTrips()` iterates `Program.Trips`, **keeps only** `status ∈ {open, offered, unassigned}`,
     and binds a `DataTable` (ID, Booking, Origin, Destination, Time, Pax, Vehicle, Region, Status) to
     `dgvTrips`.
  3. Dispatcher clicks a row → `dgvTrips_CellClick` reads the ID cell → `Trip.seekTrip(id)` →
     `selectedTrip`.
  4. Sets `comboRegion.SelectedIndex` to the trip's region, then calls `refreshDrivers()` (Pipeline 3
     ranking) and `refreshOffers()`.
- **Postconditions:** `selectedTrip` is set; the ranked-drivers and offers grids reflect it.
- **Source:** [DispatchBoardPanel.cs](../../ExternalDriverDispatch/DispatchBoardPanel.cs) ·
  [Trip.cs](../../ExternalDriverDispatch/Trip.cs) `seekTrip`.

---

## 2. Assign Trip to Region  · 🟦 OBLIGATED — Use Case

**Prompt.** Trace associating an open trip with a region so driver matching can begin. The obligated
core is two steps (set region, persist). The Maps enrichment that follows is 🟧 extra (Pipeline 9).

- **Entry point:** `DispatchBoardPanel.btnAssignRegion_Click`.
- **Preconditions:** a trip is selected and `status == open`; ≥1 `Region` exists.
- **Pipeline:**
  1. Guard: `selectedTrip != null` and a region is chosen in `comboRegion`; resolve
     `Region region = Program.Regions[idx]`.
  2. 🟦 `selectedTrip.setRegion(region)` then `selectedTrip.updateTrip()` →
     `SQL_CON.execute_non_query("EXECUTE sp_Trip_update …")`. (Run inside `runQuiet(...)` so the
     per-write success popup is suppressed; the activity log narrates instead.)
  3. 🟧 `svc.EnrichTrip(selectedTrip)` → see **Pipeline 9** (Maps fills `distanceKm` /
     `estimatedDurationMinutes`, persists via a second `sp_Trip_update`).
  4. `refreshDrivers()` (Pipeline 3) and `loadTrips()`.
- **Branches:** region not active / not chosen → `warn(...)`, trip stays unassigned.
- **Postconditions:** trip's `region` is set and persisted; the trip appears under that region's
  eligible-driver matching; `status` stays `open`.
- **Source:** [DispatchBoardPanel.cs](../../ExternalDriverDispatch/DispatchBoardPanel.cs) ·
  [Trip.cs](../../ExternalDriverDispatch/Trip.cs) `updateTrip`.

---

## 3. Send Trip Offer (UC17)  · 🟦 OBLIGATED — Use Case

**Prompt.** Trace ranking the eligible drivers for the assigned region and sending the top-ranked driver
a WhatsApp offer with a unique confirmation link. The obligated core is creating the `Offer` and moving
the `Trip` `open → offered`; the ranking, message composing, and WhatsApp send are 🟧 extra services that
each have an offline fallback.

- **Entry point:** ranking runs in `refreshDrivers()`; sending in `btnSendOffer_Click`.
- **Preconditions:** trip selected, region assigned, `status == open`; ≥1 eligible driver.
- **Pipeline:**
  1. **Eligibility + rank** — `svc.RankEligible(trip)`:
     a. `region.getEligibleDrivers(trip)` = active drivers whose `getMaxPassengers() ≥ numPassengers`
        (`ExternalDriver.isEligibleForTrip`; vehicle type is a ranking preference, **not** a hard filter).
     b. 🟧 **long-distance gate:** if `trip.getDistanceKm() ≥ 100` keep only drivers with
        `worksLongDistance` (distance comes from Maps, Pipeline 9).
     c. **exclude already-contacted** drivers (`trip.getOffers()` → driver ids) — this is what prevents
        duplicate offers across the forwarding chain.
     d. 🟧 `ranker.Rank(trip, eligible)` → `IReadOnlyList<RankedDriver>` (rank + reason). See Pipeline 10.
  2. Dispatcher clicks **Send offer** → guard `status == open` → `pickDriverFromGridOrTop()` (selected
     row, else rank #1).
  3. **Send** — `svc.SendOffer(trip, rd)` (all DB writes quiet):
     - 🟦 `new Offer(getNextOfferId(), trip, driver, now, now+1h, OfferStatus.pending, …)` →
       `createOffer()` → `sp_Offer_create`; `setRankReason(rd.Reason)`; `updateOffer()` → `sp_Offer_update`.
     - 🟦 `trip.offer()` — guard `open` → `sp_Trip_offer` → in-memory `status = offered`.
     - `url = offer.generateApprovalUrl()` (`…/approve?offer={id}`).
     - 🟧 `text = composer.OfferMessage(driver, trip, url)` (Pipeline 10).
     - 🟧 build the 7 ordered template vars (driver, pickup, time, destination, pax, pay, link) →
       `channel.SendTemplate(phone, Config.TwilioContentSid, vars, text)` → `waMessageId` (Pipeline 12).
     - 🟦 `new Message(getNextMessageId(), driver, offer, MessageDirection.outbound, waId, text, now)` →
       `sp_Message_create` (audit trail).
  4. `loadTrips(); refreshDrivers(); refreshOffers()`.
- **Branches (UC17 extensions):** 4a no eligible driver → `warn`; offer expiry (`expiresAt = now+1h`) and
  rejection drive **Pipeline 5** (forward).
- **Postconditions:** an `Offer (pending)` exists; `Trip.status = offered`; an outbound `Message` is
  recorded; the approval URL is shown/sent.
- **Source:** [DispatchService.cs](../../ExternalDriverDispatch/Services/DispatchService.cs)
  `RankEligible` / `SendOffer` · [Offer.cs](../../ExternalDriverDispatch/Offer.cs) ·
  [Trip.cs](../../ExternalDriverDispatch/Trip.cs) `offer`.

---

## 4. Respond to Trip Offer (UC07)  · 🟦 OBLIGATED — Use Case

**Prompt.** Trace what happens when a driver replies. A free-text reply is interpreted to yes/no/ambiguous
and drives the `Offer` state machine; on the binding approval-link click the offer is accepted (which
includes Update Ride Control, Pipeline 6); a "no" triggers forwarding (Pipeline 5). The obligated core is
the `Offer` verbs + the `Message` audit row; AI interpretation and the real webhook are 🟧 extra.

- **Entry points:** simulated free-text reply `btnReceiveReply_Click`; real inbound `handleWebhookReply`
  (Pipeline 13); manual shortcuts `btnWhatsappYes_Click` / `btnApprove_Click` / `btnDecline_Click` /
  `btnTimeout_Click`.
- **Preconditions:** an `Offer` for the trip is `pending` or `pending_approval`.
- **Pipeline (free-text path):** `svc.HandleDriverReply(offer, text)`:
  1. 🟦 `new Message(inbound, …)` → `sp_Message_create`; `offer.setDriverReplyText(text)`.
  2. 🟧 `restrictionParser.Parse(text)` — if an availability change is found (e.g. "no nights",
     "on vacation") → `ApplyRestriction` sets the driver's flags and `updateExternalDriver()` →
     `sp_ExternalDriver_update` (Pipeline 11).
  3. 🟧 `interpreter.Interpret(name, text)` → `ReplyIntent {Yes|No|Ambiguous}`;
     `offer.setAiInterpretation(...)`, `updateOffer()` → `sp_Offer_update` (Pipeline 11).
  4. 🟦 dispatch on intent:
     - **Yes** → `offer.markPendingApproval()` → `sp_Offer_pending_approval` (`pending → pending_approval`;
       trip stays `offered` until the link is clicked).
     - **No** → `offer.reject()` → `sp_Offer_reject` (`offer → rejected`, `trip → open`), then caller runs
       `svc.Forward(trip)` (Pipeline 5).
     - **Ambiguous** → no state change (a clarifying question would be sent).
- **Binding approval (link click):** `btnApprove_Click` → `offer.accept()` → `sp_Offer_accept`
  (`pending|pending_approval → accepted`, `trip → confirmed`, one transaction) → **Pipeline 6 (UC08)**.
- **Manual shortcuts map to the same verbs:** decline → `reject()` + `Forward`; timeout → `timeout()`
  (`→ timeout`, `trip → open`) + `Forward`.
- **Postconditions:** accepted → trip `confirmed` + Ride Control synced + confirmation message; rejected/
  timed-out → forwarded.
- **Source:** [DispatchService.cs](../../ExternalDriverDispatch/Services/DispatchService.cs)
  `HandleDriverReply` · [Offer.cs](../../ExternalDriverDispatch/Offer.cs) verbs ·
  [DispatchBoardPanel.cs](../../ExternalDriverDispatch/DispatchBoardPanel.cs) buttons.

---

## 5. Forward Offer to Next Driver  · 🟦 OBLIGATED — Use Case  *(«extend» on reject/timeout/"No")*

**Prompt.** Trace re-queuing a declined/timed-out trip to the next eligible driver, or escalating when
none remain.

- **Entry point:** `DispatchService.Forward(trip)` — called from `btnDecline_Click`, `btnTimeout_Click`,
  and `HandleDriverReply` when intent is No.
- **Preconditions:** the prior offer has moved the trip back to `open` (via `reject()` / `timeout()`).
- **Pipeline:**
  1. 🟧 `RankEligible(trip)` — re-ranks, and because an `Offer` now exists for the contacted driver,
     step 1c of Pipeline 3 **excludes** them automatically.
  2. 🟦 if the ranked list is empty → `trip.unassign()` → `sp_Trip_unassign` (`open → unassigned`) + log
     a manager escalation; return.
  3. 🟦 else → `SendOffer(trip, ranked[0])` — repeats the send half of Pipeline 3 for the next driver.
- **Postconditions:** a fresh `Offer (pending)` to the next driver and `trip = offered`, **or**
  `trip = unassigned` + escalation.
- **Source:** [DispatchService.cs](../../ExternalDriverDispatch/Services/DispatchService.cs) `Forward` ·
  [Trip.cs](../../ExternalDriverDispatch/Trip.cs) `unassign`.

---

## 6. Update Ride Control (UC08)  · 🟦 OBLIGATED — Use Case  *(«include» of UC07 accept)*

**Prompt.** Trace the automatic sync back to Ride Control after a driver accepts.

- **Entry point:** triggered by `Offer.accept()` (no separate dispatcher action).
- **As-built reality:** `RideControlSystem` is a design-level `«interface»` (`importTrips`,
  `updateTripAssignment`) with **no concrete client in the C# code**. The sync is *represented by* the
  side-effect of `accept()`:
  1. `offer.accept()` → `sp_Offer_accept` runs `BEGIN TRAN → Offer = accepted AND Trip = confirmed →
     COMMIT` (one transaction = both entities consistent).
  2. In-memory mirror: `offer.status = accepted`, `trip.status = confirmed`.
  3. The board logs `"Ride Control updated (driver name, phone, vehicle)"` — standing in for
     `updateTripAssignment()` pushing the real driver identity to Ride Control.
- **Branches (UC08 extensions, modeled not coded):** RC unavailable → retry then alert; duplicate
  assignment → block + alert. In this build the trip simply remains `confirmed` locally.
- **Postconditions:** trip `confirmed`; offer `accepted`; both systems "show" the assignment.
- **Source:** [Offer.cs](../../ExternalDriverDispatch/Offer.cs) `accept` · `sp_Offer_accept` in
  [scripts/stored_procedures.sql](../../scripts/stored_procedures.sql).

---
---

# PART 2 — Obligated infrastructure pipelines 🟦  *(required architecture, not use cases)*

---

## 7. Application startup & in-memory load  · 🟦 OBLIGATED — Infrastructure

**Prompt.** Trace program start and the strict load order that every flow above depends on.

- **Entry point:** `Program.Main`.
- **Pipeline:**
  1. Single-instance `Mutex` guard (a second instance would fail to bind the inbound webhook port).
  2. `Program.initLists()` loads in **strict order** (each `init` resolves FKs against lists already in
     memory): `Region.initRegions` → `ExternalDriver.initExternalDrivers` →
     `ExternalDriverRegion.initExternalDriverRegions` (wires driver↔region) → `Trip.initTrips` →
     `Offer.initOffers` → `Message.initMessages`. Each `initXyz` runs `EXECUTE sp_Xyz_get_all` via
     `SQL_CON.execute_query` and constructs entities with `is_new = false`.
  3. `Application.Run(new mainForm())` → `mainForm` shows `LoginPanel`.
- **Postconditions:** all `Program.*` static lists populated; no further DB reads during normal use
  (writes only).
- **Source:** [Program.cs](../../ExternalDriverDispatch/Program.cs) ·
  [SQL_CON.cs](../../ExternalDriverDispatch/SQL_CON.cs).

---

## 8. Entity CRUD (Region / ExternalDriver / Trip / Offer / Message)  · 🟦 OBLIGATED — Infrastructure

**Prompt.** Trace the generic create/update/delete lifecycle the entity pattern mandates (one example,
identical shape per entity). Reachable from the **Data management** area (`DispatcherHomePanel` → the
entity panels). Not a use case, but required by `PATTERNS.md`.

- **Create:** panel gathers fields → `new Xyz(getNextXyzId(), …, is_new:true)` → constructor calls
  `createXyz()` → `sp_Xyz_create` (PK passed as first param, **no** `IDENTITY`) → adds to `Program.Xyz`.
- **Update:** mutate via setters → `updateXyz()` → `sp_Xyz_update`.
- **Delete:** `deleteXyz()` removes from `Program.Xyz` then `sp_Xyz_delete`.
- **Read:** always from the in-memory `Program.Xyz` list (loaded in Pipeline 7); `seekXyz(id)` linear
  search; `getNextXyzId()` = `max(id)+1`.
- **Invariant:** all DB access through `SQL_CON` + stored procedures (no ad-hoc SQL); entities own their
  own DB methods (no DAL/service layer for persistence).
- **Source:** [Region.cs](../../ExternalDriverDispatch/Region.cs) ·
  [ExternalDriver.cs](../../ExternalDriverDispatch/ExternalDriver.cs) ·
  [Trip.cs](../../ExternalDriverDispatch/Trip.cs) · [Offer.cs](../../ExternalDriverDispatch/Offer.cs) ·
  [Message.cs](../../ExternalDriverDispatch/Message.cs).

---
---

# PART 3 — Extra pipelines 🟧  *(enhancements beyond the assignment)*

> All of Part 3 is offline-first: each service is selected by `ServiceFactory` only when its own
> `*.Enabled` flag in `app.config` is on **and** its credentials exist; otherwise a deterministic
> fallback runs. With everything off (the lab default), Pipelines 1–8 behave identically.

---

## 9. Maps trip enrichment (Service 1)  · 🟧 EXTRA  *(plugs into Pipeline 2)*

**Prompt.** Given a trip's pickup/dropoff/time, fill its drive distance and ETA, which gate the
long-distance driver filter and feed the AI ranking prompt.

- **Entry point:** `DispatchService.EnrichTrip(trip)`.
- **Pipeline:** `maps.GetDriveInfo(pickup+city, dropoff+city, pickupTime)` →
  `DriveInfo(DurationMinutes, DistanceKm)` → `trip.setDistanceKm` / `setEstimatedDurationMinutes` →
  `trip.updateTrip()` (quiet) → `sp_Trip_update`; log a one-line summary (+ `(long distance)` tag when
  `≥ 100 km`).
- **Live vs offline:** `GoogleMapsDriveInfoProvider` (Distance Matrix API) when `Maps.Enabled` + key;
  else `StaticDriveInfoProvider` → `(60 min, 0 km)`. Any exception/non-OK response → the `(60, 0)`
  fallback (a missing key downgrades the feature, never throws).
- **Consumed by:** Pipeline 3 step 1b (long-distance gate) and Pipeline 10 (ranking prompt).
- **Source:** [Services/MapsService.cs](../../ExternalDriverDispatch/Services/MapsService.cs) ·
  [Services/DispatchService.cs](../../ExternalDriverDispatch/Services/DispatchService.cs) `EnrichTrip`.

---

## 10. AI driver ranking (Service 2)  · 🟧 EXTRA  *(plugs into Pipelines 3 & 5)*

**Prompt.** Order the eligible drivers best-first with a one-line reason per driver.

- **Entry point:** `ranker.Rank(trip, eligible)` (called inside `RankEligible`).
- **Live (`ClaudeAiService`):** builds a prompt (trip city pair, pax, vehicle, `distanceKm`; the eligible
  drivers' id/name/vehicle/home-city) → Anthropic Messages API (`Ai.Model`) asking for driver ids
  best-first → parses ids back into `RankedDriver(driver, rank, "AI-ranked")`; any unranked driver is
  appended; on **any** exception → proximity fallback.
- **Offline (`ProximityDriverRanker`):** deterministic sort — vehicle match first, then proximity
  (home city = pickup > home city in region > out-of-area), then current load (pending offers), then id;
  reason string built from those factors.
- **Fills:** `Offer.rankPosition` + `Offer.rankReason` (persisted in `SendOffer`).
- **Source:** [Services/AiService.cs](../../ExternalDriverDispatch/Services/AiService.cs)
  (`IDriverRanker`, `ProximityDriverRanker`, `ClaudeAiService.Rank`).

---

## 11. AI reply interpretation + restriction parsing (Service 2)  · 🟧 EXTRA  *(plugs into Pipeline 4)*

**Prompt.** Classify a free-text driver reply as yes/no/ambiguous (this drives the Offer state machine),
and separately extract any availability change ("no nights", "on vacation") to update the driver.

- **Interpret:** `interpreter.Interpret(name, text)` → `ReplyIntent`. Live = Claude one-word
  classification; offline = `KeywordReplyInterpreter` (yes/no keyword sets, ambiguous if both/neither).
  Result is written to `Offer.aiInterpretation` and switches the verb (Yes→`markPendingApproval`,
  No→`reject`, Ambiguous→no-op).
- **Restriction parse:** `restrictionParser.Parse(text)` → `DriverAvailabilityUpdate?` (nullable flags +
  note). Both live and offline use the deterministic `KeywordRestrictionParser`. A non-null result →
  `ApplyRestriction` sets `worksNights/worksShabbat/worksLongDistance/active` + `updateExternalDriver()`
  → `sp_ExternalDriver_update`.
- **Source:** [Services/AiService.cs](../../ExternalDriverDispatch/Services/AiService.cs)
  (`IReplyInterpreter`, `IRestrictionParser`) ·
  [Services/DispatchService.cs](../../ExternalDriverDispatch/Services/DispatchService.cs)
  `HandleDriverReply` / `ApplyRestriction`.

---

## 12. WhatsApp delivery + approved templates (Service 3)  · 🟧 EXTRA  *(plugs into Pipeline 3)*

**Prompt.** Deliver the composed offer text (or an approved template) to the driver's phone and return a
message id; the flow records a `Message` row either way.

- **Entry point:** `channel.SendTemplate(phone, contentSid, variables, fallbackBody)` (and `SendText`).
- **Provider selection (`ServiceFactory.Channel`):** `LoggingChannel` unless `WhatsApp.Enabled`; then
  `WhatsApp.Provider == "twilio"` (+ creds) → `TwilioWhatsAppChannel`, or `"meta"` (+ creds) →
  `WhatsAppCloudChannel`; otherwise back to `LoggingChannel`.
- **Twilio template:** `POST …/Messages.json` with `ContentSid` + `ContentVariables` (JSON map
  `"1"→value …`) when `Twilio.ContentSid` is set (required to *start* a conversation outside the 24h
  window); else a plain `Body` send. `whatsapp:` E.164 address prefix; HTTP Basic `AccountSid:AuthToken`.
- **Offline (`LoggingChannel`):** writes nothing to the network, returns a `LOCAL-…` id — indistinguishable
  to the rest of the flow (a `Message` row is still created).
- **Resilience:** every send wraps failures and returns a `LOCAL-…` id, so a send never crashes the flow.
- **Source:** [Services/MessageChannel.cs](../../ExternalDriverDispatch/Services/MessageChannel.cs) ·
  [Services/ServiceFactory.cs](../../ExternalDriverDispatch/Services/ServiceFactory.cs) `Channel`.

---

## 13. Real inbound WhatsApp: webhook + ngrok tunnel  · 🟧 EXTRA  *(real version of Pipeline 4 inbound)*

**Prompt.** Receive a real driver reply on a desktop app that has no public URL, and route it into the
same `HandleDriverReply` pipeline as the simulated box.

- **Pipeline:**
  1. `DispatchBoardPanel` starts an **app-scoped static** `WebhookServer` once (`startWebhookServer`),
     scanning from `BasePort` for a free port; each new board re-points `OnReply` at itself.
  2. `startTunnelInBackground` launches **ngrok** (`http <port> --host-header=rewrite`), reads the public
     https URL from `http://127.0.0.1:4040/api/tunnels`, copies it to the clipboard, and
     `tryUpdateTwilioWebhook` PATCHes the Twilio **WhatsApp Sender**'s `callback_url` (Messaging v2
     `Channels/Senders`) — needed every launch because free-tier ngrok URLs rotate.
  3. Inbound POST → `WebhookServer.OnReply(phone, body)` → `handleWebhookReply` (marshaled to the UI
     thread) → matches the most recent `pending`/`pending_approval` offer for that normalized phone →
     `svc.HandleDriverReply(...)` → on No, `svc.Forward(...)` (Pipelines 4 & 5).
- **Source:** [DispatchBoardPanel.cs](../../ExternalDriverDispatch/DispatchBoardPanel.cs)
  (`startWebhookServer` / `startTunnelInBackground` / `handleWebhookReply`) ·
  [WebhookServer.cs](../../ExternalDriverDispatch/WebhookServer.cs).

---

## 14. Driver Performance report  · 🟧 EXTRA  *(standalone, DB-only)*

**Prompt.** Produce a read-only per-driver performance table for an optional region + optional date range.

- **Entry point:** `DriverPerformancePanel` (Data management) → Generate.
- **Pipeline:** gather filters (region combo + two optional `DateTimePicker`s, unchecked = no bound) →
  `sp_DriverPerformance(@region_id, @from, @to)` (the one cross-table aggregation SP: `INNER JOIN`
  `Offer ↔ ExternalDriver ↔ Trip`, `GROUP BY` driver; offers received/accepted/rejected/timed-out via
  conditional `SUM(CASE…)`, acceptance rate, and average response time via `OUTER APPLY` over the inbound
  `Message` audit trail) → `DataTable.Load` → bind to the grid. No Save/Update/Delete; never calls an
  external service.
- **Source:** [DriverPerformancePanel.cs](../../ExternalDriverDispatch/DriverPerformancePanel.cs) ·
  `sp_DriverPerformance` in [scripts/stored_procedures.sql](../../scripts/stored_procedures.sql).

---

## 15. Settings — per-service live/offline  · 🟧 EXTRA  *(technical/NFR screen)*

**Prompt.** Trace how a user flips a service live/offline and supplies credentials at runtime.

- **Entry point:** `SettingsPanel` (Data management).
- **Pipeline:** per-service **Live** toggle + key fields + a WhatsApp **provider** dropdown
  (Meta/Twilio swaps the credential fields) → `Config.Save(values)`
  (`ConfigurationManager.OpenExeConfiguration` + `RefreshSection`) writes the output `<exe>.config` and
  `Config.Reload()`s. The board rebuilds its services from `Config` on next open, so changes apply then.
- **Gotchas:** writes the **output** config (an F5 rebuild copies source `app.config` over it); keys are
  plaintext on disk (masked in the UI, `chkShowKeys` reveals). Like Login, this is **not** a UC or entity
  and must not appear in the class/UC diagrams.
- **Source:** [SettingsPanel.cs](../../ExternalDriverDispatch/SettingsPanel.cs) ·
  [Services/Config.cs](../../ExternalDriverDispatch/Services/Config.cs).

---

## Deferred (built nothing yet) — Multi-ride bundling

UC17's "package trips for the top driver (no time conflicts)" — grouping an eligible driver's trips into
one offer with on-time + ≤5h Maps-chaining feasibility — is **deferred pending a course-director design
decision** and is **not implemented**. Plan only:
`C:\Users\Dan Azaryad\.claude\plans\precious-wandering-cerf.md`. Listed here for completeness; do not
implement without explicit confirmation.
