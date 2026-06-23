# CLAUDE.md — Group 17: External Driver Dispatch System (Wholestay / Transfers TLV)

## What This Project Is

This is the **Group 17 SAD course project** at Ben-Gurion University, Industrial Engineering and Management.

The organization is **Wholestay / Transfers TLV** — a tourism and ground transport company providing door-to-door transfer services for international tourists in Israel.

The system being built is an **automation layer on top of the existing Ride Control system**. It does not replace Ride Control; Ride Control remains the operational core. Architecturally, this system registers itself as a single "supplier" in Ride Control's external supplier interface, wrapping all freelance and external drivers.

**Core problem solved:** The dispatcher contacts each driver manually via WhatsApp, waits for a reply, then updates Ride Control by hand. For 10–15 trips a day this consumes 3–4 hours.

**What the system does:**
1. Receives open trips from Ride Control via the external supplier API.
2. The dispatcher views open trips and assigns each to a geographic region.
3. The system (with AI ranking) finds the best eligible driver and sends a WhatsApp message with a unique confirmation link.
4. The driver opens a Hebrew, mobile-friendly approval page and accepts or rejects each trip individually.
5. On acceptance: the system updates Ride Control automatically. On rejection or timeout: the system forwards the offer to the next ranked driver.

The dispatcher retains decision authority (which trips to dispatch, to which region). The system handles all repetitive outreach.

---

## Database

**Database name:** `ExternalDriverDispatch`

**Rule:** Every `execute_sql` call (and every stored procedure) must begin with `USE ExternalDriverDispatch;`. No exceptions — this prevents accidental writes to the wrong database.

**Schema source:** `scripts/create_database.sql` — run this once to create all tables. Do not run it again on a live database; write migration scripts for schema changes instead.

---

## Architecture Conventions

> The following conventions apply to all SAD student projects and are inherited verbatim from `cloned/PATTERNS.md`. Any conflict between other docs (including `docs/insightFromRealProject/`) and these conventions resolves in favour of these conventions.

### The Human vs. AI Distinction — Course-Wide Principle

This distinction is central to how you should approach work in this course. Do not blur it.

**Humans must do this** (AI cannot substitute, even though AI will later use the output):
- Organizational context: who the business is, what problem it has, why it matters
- Stakeholder identification and needs elicitation
- Problem statement and project scope
- Initial domain modeling: what entities exist, what relationships make sense
- Deciding which use cases exist and which don't (e.g., deciding Login is an NFR, not a UC)
- Prioritization and tradeoffs

**AI can accelerate this** (once the human thinking is done):
- Structuring requirements into user story format
- Writing VP18-style UC specs from a brief description
- Generating the UC diagram HTML from the spec data
- Generating entity classes, stored procedures, and panels from UC specs + implementation notes
- Checking consistency between artifacts (traceability)

Project artifacts are produced in this order: organization/problem context → requirements → UC specs → code. Each is input to the next.

---

### Architecture — Key Patterns

#### Entity Pattern
Every entity class is self-contained. Each one owns:
- Private fields + getters/setters
- Constructor with `bool is_new` — if `true`, calls `getNextXYZId()` to assign a new PK, then calls `createXYZ()`, then adds to `Program.list`; if `false`, just sets fields (used during loading)
- `createXYZ()`, `updateXYZ()`, `deleteXYZ()` — each builds a `SqlCommand` with a stored procedure
- `static initXYZs()` — loads all records from DB into `Program.XYZs`, always calls constructor with `is_new = false`
- `static seekXYZ(id)` — searches `Program.XYZs` by ID
- `static getNextXYZId()` — returns `max(id) + 1` over `Program.XYZs` (or `1` if the list is empty). See "Primary Key Strategy" below.

#### Primary Key Strategy
**Primary keys are assigned in C#, not by the database.**

- DDL: every PK is `INT NOT NULL PRIMARY KEY`. Do **not** use `IDENTITY(1,1)`.
- The entity class's `static getNextXYZId()` returns `max(id) + 1` over the in-memory list.
- The `is_new` constructor calls `getNextXYZId()` before `createXYZ()` to assign the new row's PK.
- Create stored procedures take the PK as the first parameter (`@<entity>_id`). They do **not** use `SCOPE_IDENTITY()` and do **not** return the new ID.

This is deliberate: students can read the full lifecycle of an ID in one place (the C# constructor), and DB writes are deterministic from the entity's state. Concurrency is not a concern in the single-user teaching context.

#### In-Memory Lists
All data lives in `Program.*` static lists after startup. No DB calls during normal use except writes.

`initLists()` load order is strict: **base entities first, then entities with FK references, then association classes last.** Each project's CLAUDE.md states its own concrete order.

#### DB Operations
All DB access goes through stored procedures. **No ad-hoc SQL strings in application code.** This is an NFR.

#### Panel Navigation
Single-window model. All screens are `UserControl` panels. Navigation: `mainForm.showPanel(new XYZPanel())`. Every panel has a Back button. **No additional Forms or dialogs during normal operation.**

#### Inheritance — Table-per-Subclass
When an entity has subtypes, use table-per-subclass: a base table for the parent + one table per subclass holding only the subclass's unique fields + a FK to the base table. Load with a LEFT JOIN and check for `DBNull.Value` to determine subtype. *(Sample project example: `Order` base with `DeliveryOrder` / `PickupOrder` subclasses.)*

#### Association Class
When a many-to-many relationship has its own attributes, model it as an association class linking the two sides. In the C# class, both sides are stored as **object references, not IDs**. *(Sample project example: `OrderItem` linking `Order` ↔ `Product` with quantity and unit price.)*

#### No Service Layer
Entity classes own their own DB methods. One file per entity. This is intentional for teaching — students see the full lifecycle of an entity in one place.

---

### UC Diagram — Conventions

The diagram is generated from inline JavaScript data and rendered by an external shared script. Rules:

- All data globals must use `var` (not `const`/`let`) so they become `window` properties
- Wireframes are embedded as `useCaseDocs[id].wireframe` HTML strings — **not** as separate files
- All wireframe visible text must be in Hebrew; all form fields use `disabled`; no `<script>` tags inside wireframes
- The `[hidden] { display: none !important; }` style override is required in `<head>` for tab switching to work
- **Login/authentication must never appear as a UC.** Note it only in the `assumptions` array.

#### Two-Layer UC Spec Format
Each detailed UC has two sections:
1. **Formal spec** (analysis level) — behavioral, technology-neutral. No class names, SP names, or field names.
2. **Implementation Notes** (design level, clearly labelled) — maps behavioral steps to specific classes, methods, and stored procedures.

This separation is intentional and pedagogically important. Do not merge them.

---

### Language Conventions

| Context | Language |
|---|---|
| C# code (classes, methods, variables) | English |
| UI labels, button text, MessageBox text | Hebrew |
| DB text fields | Hebrew — use `NVARCHAR`, never `VARCHAR` |
| Student guide docs (`docs/*.md`) | Hebrew |
| Requirements and UC spec documents | English |
| UC diagram text (actor labels, UC names, flow steps) | Hebrew |

#### RTL Layout for Hebrew UI

Every `Form` and `UserControl` with Hebrew visible text **must** be set up for right-to-left rendering:

- `RightToLeft = Yes` on the form/panel (mirrors text direction, button alignment, scrollbar position).
- `RightToLeftLayout = true` on the root form (mirrors the entire layout, including TabControl direction and DataGridView column order).
- Set these on the parent — children inherit unless overridden.

Generate panels with these properties set from the start. Retrofitting RTL onto LTR-built panels is painful — labels overlap, alignment breaks, the designer file fights you.

---

### Decisions Already Made — Do Not Revisit Without Discussion

These apply across all SAD projects:

- **Login is not a UC.** Authentication is an NFR precondition. A `LoginPanel` is a technical artifact. Do not add Login to UC diagrams or UC specs.
- **Wireframes belong inside the UC diagram modal**, not in separate files.
- **No ad-hoc SQL.** All DB operations use stored procedures.
- **No service layer.** Entity classes own their own DB methods.
- **Single window, panel navigation.** No additional forms or dialogs during normal operation.

---

## Document Map

Two layers of sources live in `docs/`: **extracted markdown (preferred)** and the **original PDFs (backup)**. Prefer the markdown; consult a PDF only when the markdown is unclear or you need to look at a diagram.

### Extracted markdown — primary source

| File | Purpose |
|---|---|
| `cloned/PATTERNS.md` | Shared architecture conventions — inlined verbatim above |
| `docs/org-analysis/01-organization.md` | Organization description + existing information system (Hebrew) |
| `docs/org-analysis/02-interviews.md` | Interview transcripts (Hebrew) |
| `docs/org-analysis/03-problems.md` | Problems table (Hebrew) — 11 rows, numbered 1–12 (no #5 in source) |
| `docs/org-analysis/04-business-processes.md` | The 4 existing business processes (Hebrew) |
| `docs/00-requirements.md` | Functional requirements F01–F25, NFRs NF01–NF09, AI-elicitation appendix, and the traceability matrix (English) |
| `docs/00e-use-cases.md` | **Primary UC reference** — the 6 in-scope UCs in two-layer format (5 flow UCs authoritative; View Open Trips brief-level; Implementation Notes are TODO) |
| `docs/design/class-diagram.md` | Entities, attributes, operations, enum literals, relationships + multiplicities, mediator rationale, design assumptions |

### Source PDFs — backup

| File | Purpose |
|---|---|
| `docs/group_17_part_a.pdf` | Part A: organization, 4 business processes, problems table, F01–F25, NF01–NF09, actors table, UC diagram, traceability section |
| `docs/PartB_Group17.pdf` | Part B: 6 UC brief descriptions + class diagram modeling rationale and design assumptions |
| `docs/Part_B_5UC_final.pdf` | 5 detailed UC specs — **image-only, not machine-readable** |
| `docs/PartB_Group17_UCs.pdf` | Readable export of the 5 detailed UC specs + Import Drivers from Excel (text-extractable; source of truth for Assign Trip to Region and Forward Offer) |
| `docs/SEND TRIP UC DETAILS group 17 a.pdf` | UC17 detail (F12, F13) |
| `docs/RESPOND TO TRIP UC DETAILS group 17 a.pdf` | UC07 detail (F14, F15, F17, F20, F21) |
| `docs/UPDATE RIDE UC DETAILS group 17 a.pdf` | UC08 detail (F16) |
| `docs/insightFromRealProject/` | Real production system files for the same company — **domain inspiration only, not architectural guidance** |

---

## Domain Entities and Load Order

### Load Order

```
Region → ExternalDriver → Trip → Offer
```

- **Region** — no FK dependencies; loads first.
- **ExternalDriver** — associated with Region; loads after Region.
- **Trip** — has `regionId` FK to Region; loads after Region.
- **Offer** — mediator class with FKs to both Trip and ExternalDriver; loads last.

### Entities

#### Region
Geographic operating zone used to filter drivers and queue trips for dispatch.

**Key fields:** `id`, `name`, `country`, `city`, `createdAt`

**Key methods:** `getActiveDrivers()`, `getOpenTrips()`, `getEligibleDrivers(trip)`

---

#### ExternalDriver
A freelance or external driver who receives trip offers via WhatsApp.

**Key fields:** `id`, `drivercode`, `name`, `phone`, `homeCity`, `vehicleType` (VehicleType enum), `worksShabbot`, `worksNights`, `worksLongDistance`, `active`

**Key methods:** `isEligibleForTrip(trip)`, `getMaxPassengers()`, `updateRegion(region)`

---

#### Trip
A transport job received from Ride Control. This system manages the assignment of this trip to an external driver; it does not manage trip execution.

**Key fields:** `id`, `externalBookingId`, `pickupAddress`, `dropoffAddress`, `pickupCity`, `dropoffCity`, `pickupTime`, `numPassengers`, `vehicleType` (VehicleType enum), `priceToDriver`, `status` (TripStatus enum), `createdAt`, `regionId` (FK → Region)

**Key methods:** `offer()`, `confirm()`, `requeue()`

---

#### Offer
Mediator class representing one outreach attempt: one driver contacted for one trip. Multiple Offer records may exist per Trip — one per driver approached during the forwarding chain.

**Design rationale (PartB decision #1):** Implemented as a separate mediator class, not an association class, because the system must retain the full history of which drivers were contacted per trip. Without this history the system cannot exclude already-contacted drivers from subsequent forwarding rounds, risking duplicate offers to the same driver.

**Key fields:** `id`, `tripId` (FK → Trip), `driverId` (FK → ExternalDriver), `createdAt`, `expiresAt`, `status` (OfferStatus enum), `driverReplyText`, `allInterpretation`, `rankPosition`

**Key methods:** `accept()`, `reject()`, `generateApprovalUrl()`

---

#### RideControlSystem (Interface)
Represents the external Ride Control system. Modeled as a C# interface because it is an external system — this project implements only the communication contract, not the system itself.

**Design rationale (PartB decision #2):** The interface represents the communication point with an external system. There is no need to implement Ride Control internals inside this project's class diagram.

**Methods:** `importTrips()`, `updateTripAssignment()`

---

### Enumerations

Three enumerations are defined (PartB decision #3 — fixed value sets prevent invalid data entry). For exact literals see `docs/design/class-diagram.md`.

| Enum | Purpose |
|---|---|
| `VehicleType` | Vehicle categories eligible for trips (sedan, minivan, minibus variants) |
| `TripStatus` | Lifecycle state of a trip (open → offered → confirmed / unassigned / cancelled) |
| `OfferStatus` | Lifecycle state of one driver outreach attempt (pending → accepted / rejected / timeout) |

---

## Use Cases Being Implemented

Six UCs are in scope. **Primary source: `docs/00e-use-cases.md`** (full two-layer specs). Five flow UCs are authoritative (from `docs/PartB_Group17_UCs.pdf` + the three UC-detail PDFs); **View Open Trips** is brief-level — its only source is the Part B brief in `docs/PartB_Group17.pdf`.

### UC Relationships

- **UC17 Send Trip Offer** `<<include>>` **Assign Trip to Region** — the trip must have a region before step 2 of UC17 can proceed; Assign Trip to Region is the prerequisite step.
- **UC07 Respond to Trip Offer** `<<include>>` **UC08 Update Ride Control** — step 8 of UC07 automatically triggers the Ride Control sync for each accepted trip.
- **Forward Offer to Next Driver** `<<extend>>` **UC07 Respond to Trip Offer** — at UC07 extension 6a (driver rejects all offered trips, or an offer times out), forwarding is triggered.

### Summary Table

| UC ID | Name | Primary Actor | One-line description |
|---|---|---|---|
| UC17 | Send Trip Offer | Dispatcher | Rank eligible drivers for the assigned region; send WhatsApp offer with unique confirmation link to top-ranked driver |
| — | Assign Trip to Region | Dispatcher | Associate an open trip with a geographic region, enabling driver matching to begin |
| UC07 | Respond to Trip Offer | External Driver - Supplier | Driver opens mobile approval page; accepts or rejects each offered trip individually |
| — | Forward Offer to Next Driver | External Driver - Supplier | When offer is rejected or timed out, re-queue trip and send new offer to next eligible driver |
| UC08 | Update Ride Control | Ride Control «System» | Automatically sync confirmed driver assignment back to Ride Control after driver acceptance |
| — | View Open Trips | Dispatcher | Display all open trips not yet assigned, with pickup time, destination, region, and status, so the dispatcher can identify which trips need action |

### UC17: Send Trip Offer
**Primary Actor:** Dispatcher  
**Preconditions:** ≥1 trip with status "Open"; dispatcher logged in; ≥1 active driver registered for the relevant region.  
**Postconditions:** WhatsApp offer sent to top-ranked driver; trip status → "Pending Approval"; system monitoring for driver response.  
**MSS (9 steps):** Dispatcher selects trips → assigns to region → system loads + filters by vehicle/capacity + ranks by proximity/workload/availability → packages trips for top driver (no time conflicts) → sends WhatsApp with unique confirmation link → status → "Pending Approval" → begin monitoring.  
**Key extensions:** 4a no eligible drivers → dispatcher alert; 7a WhatsApp fails → retry then alert; 9a timeout → auto-forward to next driver.  
**Notes:** Dispatcher may cancel before driver responds (trip returns to "Open"). If all ranked drivers exhausted without acceptance, trip flagged "Unassigned."

### Assign Trip to Region
**Primary Actor:** Dispatcher  
**Preconditions:** Trip exists with status "Open"; ≥1 active Region configured; dispatcher logged in.  
**Postconditions:** Trip associated with selected region; visible in region's dispatch queue; status remains "Open."  
**MSS (6 steps):** Dispatcher views open trips panel → drags trip onto region or selects region from dropdown → system validates region is active → updates Trip's region field → displays trip under region in dispatch queue.  
**Key extensions:** 3a inactive region → error message, trip stays unassigned; 2a supplier selected instead → routes to supplier dispatch path.

### UC07: Respond to Trip Offer
**Primary Actor:** External Driver - Supplier  
**Preconditions:** Driver received WhatsApp with valid confirmation link; offered trip(s) still in "Pending Approval."  
**Postconditions:** Accepted trips → "Assigned" + Ride Control updated; driver receives WhatsApp confirmation "Confirmed! Thank you."; rejected trips forwarded to next driver.  
**MSS (10 steps):** Driver receives WhatsApp → opens link → system validates link → system verifies trips still pending and not assigned elsewhere → approval page loads (each trip: time, pickup, destination, passengers) → driver selects Accept / Cannot per trip → system saves selections → for each accepted: status → "Assigned" + trigger UC08 → send confirmation WhatsApp → for each rejected: forward to next driver.  
**Key extensions:** 3a invalid link → session ends; 3b expired link → session ends; 4a trip already assigned → "no longer available"; 6a all rejected → forward all + notify dispatcher if no drivers remain. `«Extend»` Forward Offer to Next Driver.  
**Notes:** Driver may accept some trips and reject others in the same session; each decision is independent.

### Forward Offer to Next Driver
**Primary Actor:** External Driver - Supplier  
**Preconditions:** Offer rejected or timed out; trip still "Open" or "Pending Approval"; ranked driver list available for the trip's region.  
**Postconditions:** Trip re-offered to next eligible driver, OR escalated to manager if no drivers remain.  
**MSS (7 steps):** Driver rejects offer → system sets Offer status "Rejected" + Trip status back to "Open" → queries remaining ranked drivers in region, excluding already-contacted drivers → selects next highest-ranked → creates new Offer + generates unique confirmation URL → sends new WhatsApp offer → use case ends.  
**Key extension:** 3a no eligible drivers remain → Trip → "Unassigned," escalation WhatsApp sent to manager.

### UC08: Update Ride Control
**Primary Actor:** Ride Control «System»  
**Preconditions:** Driver accepted ≥1 trip; trip status "Assigned"; Ride Control available and reachable.  
**Postconditions:** Ride Control updated with driver name, phone number, vehicle details; both systems fully synchronized.  
**MSS (6 steps):** Driver confirms acceptance (via UC07) → system auto-initiates update to Ride Control (no manual action) → transmits driver name + phone + vehicle info → RC processes and confirms receipt → both systems show "Assigned" with full driver details → system logs sync with timestamp.  
**Key extensions:** 3a RC unavailable → retry; if exhausted, alert dispatcher; 4a RC returns error → log + alert dispatcher; 4b duplicate assignment detected → block overwrite + alert dispatcher.  
**Notes:** Triggered automatically after every successful driver acceptance. No dispatcher input required. If synchronization fails, trip remains "Assigned" in this system but must be manually verified in Ride Control.

### View Open Trips
**Source:** `docs/PartB_Group17.pdf` — Use Case 1.  
**Primary Actor:** Dispatcher  
**Preconditions:** Dispatcher is logged in to the dispatch dashboard.  
**Postconditions:** Dispatcher sees the current list of open trips and can identify which ones require action and begin the assignment process.  
**MSS:** System displays the list of open trips not yet assigned, including for each trip: trip details, pickup time, destination, activity region, and current status. The dispatcher reviews the list and selects a trip to begin the assignment process.  
**Notes:** This is the entry point of the driver assignment flow. From here the dispatcher proceeds to Assign Trip to Region.

---

## Project-Specific Decisions

Beyond the course-wide decisions in `PATTERNS.md`:

### System scope: overlay, not replacement
Ride Control is the source of truth for all trip records. This system manages only the external driver assignment lifecycle — finding the driver, sending the offer, receiving the response, syncing back. It adds no trip management or execution tracking features.

### Architectural position: single supplier
This system registers as one "supplier" in Ride Control's external supplier API. All freelance/external drivers managed here are invisible to Ride Control individually. After a driver accepts, `updateTripAssignment()` pushes the actual driver identity back to Ride Control.

### Offer is a mediator class, not an association class
`Offer` is a standalone C# class because multiple Offers may exist per Trip — one per driver approached in the forwarding chain. The history is required to exclude already-contacted drivers from subsequent forwarding rounds and to provide an audit trail of the assignment process.

### RideControlSystem is a C# interface
Only the communication contract is defined here (`importTrips`, `updateTripAssignment`). The concrete implementation class wraps the actual Ride Control API call. Do not implement Ride Control internals.

### Enumerations enforce data integrity
`TripStatus`, `OfferStatus`, and `VehicleType` are C# enums. This prevents invalid status transitions and invalid vehicle types from entering the system and makes the assignment lifecycle auditable in code.

### `docs/insightFromRealProject/` is domain inspiration only
Those files describe a real production system built for the same company. They provide business logic context but are **not** architectural or technical guidance. All conflicts resolve in favour of `cloned/PATTERNS.md`.

---

## Entry Flow

The C# WinForms app (`ExternalDriverDispatch/`) uses a **single-window, panel-navigation** model (`mainForm.showPanel(...)`). The chosen entry flow is **Login → dispatcher home**:

```
mainForm → LoginPanel → DispatcherHomePanel → { RegionPanel | ExternalDriverPanel | TripPanel | OfferPanel }
```

- **No credential-holding entity exists** in the class diagram (no email+password on any of Region / ExternalDriver / Trip / Offer). Authentication is an NFR, not a UC. `LoginPanel` therefore uses a **placeholder dev password (`1234`)** plus a "כניסת מפתח" dev-bypass button — clearly commented as a stand-in for a real auth source. It is **not** a domain entity and must not be modeled as a UC.
- The **Dispatcher** is the only human actor operating the desktop app. The External Driver acts through the WhatsApp mobile approval page (out of scope for WinForms); Ride Control is a «System» actor.
- `DispatcherHomePanel` is the role home: four buttons routing to the entity-management (CRUD) panels, plus logout back to `LoginPanel`.
- Every Form/UserControl with Hebrew text sets `RightToLeft = Yes`; `mainForm` also sets `RightToLeftLayout = true`.

## Implementation Status — Phases 5–11 (C# app)

Built under `ExternalDriverDispatch/` (solution `ExternalDriverDispatch.sln`):

- **Entities** (one file each, entity pattern): `Region`, `ExternalDriver`, `Trip`, `Offer`, `Message`. `ExternalDriverRegion.cs` is a **static junction loader only** (the many-to-many has no attributes → not an association class; it wires `ExternalDriver`↔`Region` object references in memory). `Enums.cs` holds `VehicleType` / `TripStatus` / `OfferStatus` / `MessageDirection` (literals match the DB tokens exactly, so `enum.ToString()` round-trips) plus English display helpers.
- **Load order** in `Program.initLists()`: `Region → ExternalDriver → ExternalDriverRegion → Trip → Offer → Message` (`Message` last: FKs to driver and offer).
- **CRUD panels**: `RegionPanel`, `ExternalDriverPanel`, `TripPanel`, `OfferPanel` — grid + edit fields + add/update/delete/clear/back, Hebrew RTL.
- **State machine (Phase 7 + refinement)** — state-bearing entities `Trip` and `Offer`. Domain-verb methods with inline guards + in-memory mirroring:
  - `Trip.assignRegion(region)` — open|assigned_to_region → **assigned_to_region** (region assigned; calls `sp_Trip_assign_region`)
  - `Trip.offer()` — assigned_to_region → **offered** (WhatsApp sent; calls `sp_Trip_offer`)
  - `Trip.confirm()` — offered → **confirmed** (driver accepted; called by `Offer.accept()`)
  - `Trip.requeue()` — offered → **assigned_to_region** (driver rejected/timed-out; re-queued to next driver)
  - `Trip.flagManualAssignment()` — {assigned_to_region|offered} → **manual_assignment** (all drivers exhausted, OR 6h-before-pickup deadline reached; calls `sp_Trip_manual_assignment`, then `notifyDispatcher()`)
  - `Trip.updateOfferCount()` — increments `offerCounter` (attractiveness metric; called by `DispatchService.SendOffer`; calls `sp_Trip_update_offer_count`)
  - `Offer.accept()` (→accepted, **trip→confirmed**), `Offer.reject()` (→rejected, **trip→assigned_to_region**), `Offer.markPendingApproval()` (pending→pending_approval), `Offer.timeout()` (→timeout, **trip→assigned_to_region**), `Offer.cancel()` (pending|pending_approval→cancelled; used during deadline escalation)
  - Matching transactional SPs in `scripts/stored_procedures.sql`: `sp_Trip_assign_region`, `sp_Trip_offer`, `sp_Trip_confirm`, `sp_Trip_requeue`, `sp_Trip_manual_assignment`, `sp_Trip_update_offer_count`, `sp_Offer_accept`, `sp_Offer_reject`, `sp_Offer_pending_approval`, `sp_Offer_timeout`, `sp_Offer_cancel`. (`sp_Trip_unassign` is **dropped** — replaced by `sp_Trip_manual_assignment`.)
  - **Deadline timer** (`mainForm.deadlineTimer`, `Forms.Timer`, 60s interval): fires `DispatchService.EscalateOverdueTrips()` every minute. If a trip's `pickupTime` is within 6 hours and it is still `assigned_to_region` or `offered`, all pending/pending_approval offers are cancelled and `flagManualAssignment()` is called. Fires on the UI thread (safe for MessageBox, DB, in-memory lists). Only active while the app is open.
  - **`TripStatus` literals**: `open`, `assigned_to_region`, `offered`, `confirmed`, `completed`, `cancelled`, `manual_assignment`. (`unassigned` has been removed.) CHECK constraint `CK_Trip_status` is named (not auto-generated) so migrations can find and drop it.
  - **`Trip.offerCounter`** (INT, default 0): counts total outreach attempts per trip — attractiveness metric. Backfilled from Offer history by the migration.
  - Verb buttons are on `TripPanel`/`OfferPanel` (CRUD buttons retained). Transition behaviour is grounded in `docs/insightFromRealProject/dispatch_flow.md`. State diagrams: `docs/design/state-diagram.md` + `docs/design/visual-paradigm-import/plantuml-diagrams/state_trip.puml` / `state_offer.puml`.

### Dispatch Board (the routine flow)
`DispatchBoardPanel` is the dispatcher's main screen (entry after login). It runs the end-to-end pipeline on one screen: open-trips queue → assign region (`Trip.assignRegion()` → status `assigned_to_region`) → ranked **eligible** drivers (`Region.getEligibleDrivers` = active + capacity; `isEligibleForTrip` no longer hard-filters vehicle type — that is a ranking preference, per `dispatch_flow.md` Stage 4) → **Send offer** (creates `Offer` pending, `Trip.offer()`, shows approval URL) → simulate driver response (yes / approve / decline / timeout) → on approve: `Offer.accept()` (trip confirmed + "Ride Control updated"); on decline/timeout: auto-**forward** to the next ranked driver excluding already-contacted ones, or `Trip.flagManualAssignment()` + manager escalation when none remain. `SQL_CON.SuppressSuccessMessages` lets the board run multi-step DB actions without a popup per write (it logs a narrative instead). The four CRUD panels are demoted to a secondary **Data Management** area reachable via the board's "⚙ Data management" button (`DispatcherHomePanel`).

### External services (Maps / Claude AI / WhatsApp) — behind interfaces, offline-first
Three real-world integrations live in `ExternalDriverDispatch/Services/`, each behind a C# interface with a **deterministic offline fallback**. Offline/live is decided **per service** (no global master switch): each service's own `*.Enabled` flag in `app.config` `<appSettings>` controls it, and all default to `false` — so the whole dispatch flow runs end-to-end with no keys and no internet. A missing key downgrades that one feature, it never crashes the app. `ServiceFactory` returns the real impl only when that service is `Enabled` *and* its credentials are present; otherwise the fallback.

- **Service 1 — Maps** (`IDriveInfoProvider`): `StaticDriveInfoProvider` (fallback → 60 min / 0 km) vs `GoogleMapsDriveInfoProvider` (Distance Matrix). Fills `Trip.distanceKm` + `Trip.estimatedDurationMinutes`; `distanceKm ≥ 100` gates the long-distance driver filter and feeds the AI ranking prompt.
- **Service 2 — Claude AI** (`IDriverRanker`, `IMessageComposer`, `IReplyInterpreter`, `IRestrictionParser`): fallbacks are a proximity-sort ranker, a template composer, a keyword interpreter, a keyword restriction parser; real impl is `ClaudeAiService` (Anthropic Messages API, model from `Ai.Model`). Fills `Offer.rankPosition` + `Offer.rankReason` + `Offer.aiInterpretation`; the interpreted intent **drives the Offer state machine** (yes→`markPendingApproval`, no→`reject`+forward, ambiguous→no change).
- **Service 3 — WhatsApp** (`IMessageChannel`, **provider-switchable**): `LoggingChannel` (fallback, returns a `LOCAL-…` id) vs a real provider chosen by `WhatsApp.Provider` = `meta` (`WhatsAppCloudChannel`, Graph API) or `twilio` (`TwilioWhatsAppChannel`, Twilio REST — `POST .../Messages.json`, Basic auth `AccountSid:AuthToken`, `whatsapp:` address prefix). `ServiceFactory.Channel()` picks the provider when `WhatsApp.Enabled` and that provider's creds are present. Inbound is **simulated** (no public webhook on a desktop app) via the board's free-text "Receive ←" box. Every send/receive writes a `Message` row (the conversation audit trail, viewable in **Data management → Messages**). The **Twilio CLI** is installed for setup/testing only (`twilio login`, `twilio api core messages create …`); the app sends via the REST API the CLI wraps — same Account SID/Auth Token. Note: business-initiated WhatsApp offers require a Meta-approved **template** (free-text `Body` only works inside the 24h window or the Twilio sandbox). Template support is wired in: `IMessageChannel.SendTemplate(phone, contentSid, variables, fallbackBody)` sends a Twilio Content template (`POST .../Messages.json` with `ContentSid` + `ContentVariables` JSON) when `Twilio.ContentSid` is set, else falls back to plain `Body`. `DispatchService.SendOffer` builds the 7 ordered variables (driver, pickup, time, destination, passengers, pay, link) matching the `edd_trip_offer` template. Create/submit the template with `scripts/twilio_create_template.ps1` (Content API — the CLI's `content:create` can't take the nested body); the resulting `HX…` sid goes in Settings (Twilio "Template SID") / `Twilio.ContentSid`.

`DispatchService` is the domain orchestration the board calls (the UI never touches a service or the network directly): `EnrichTrip` (Maps) → `RankEligible` (eligibility + long-distance gate + AI rank) → `SendOffer` (AI compose + WhatsApp send + `Offer` + outbound `Message`) → `HandleDriverReply` (inbound `Message` + AI interpret → state machine) → `Forward`. The board points `DispatchService.Log` at its activity log so the three services narrate each step. This "service layer for external APIs" is **not** a DAL — entities still own their own DB methods (the "No Service Layer" rule is about DB access, not external integrations; this mirrors `RideControlSystem` as an external interface). **Never hardcode keys**; they live in `app.config` and must stay empty in the repo.

**Settings screen** (`SettingsPanel`, in Data management): a per-service **"Live" toggle** (off = offline fallback), the key/token fields, and a WhatsApp **provider dropdown** (Meta / Twilio) that swaps the relevant credential fields; writes back to app.config via `Config.Save(...)` (`ConfigurationManager.OpenExeConfiguration` + `RefreshSection`). `Config` is reload-able (`Config.Reload()`), so saved values apply the next time the board is opened (the board rebuilds its services from `Config` each load). Two gotchas: it writes the **output** `*.config`, so an F5 rebuild copies the source `app.config` over it and resets values; and keys are plaintext on disk (masked in the UI, `chkShowKeys` toggles). `SettingsPanel` is a **technical/NFR screen like `LoginPanel`** — it is **not** a UC or an entity and must not appear in the class/UC diagrams.

### Reports (Phase 8)
A read-only **Driver Performance** report. `sp_DriverPerformance(@region_id, @from, @to)` (in `scripts/stored_procedures.sql`) is the only cross-table aggregation SP: per driver it `INNER JOIN`s `Offer ↔ ExternalDriver ↔ Trip`, `GROUP BY`s the driver, and returns offers received / accepted / rejected / timed-out (conditional `SUM(CASE…)`), acceptance rate, and **average response time** — the last derived from the inbound `Message` audit trail via `OUTER APPLY` (NULL when a driver never replied). Both filters are optional (NULL = all regions / no date bound; `@to` is inclusive of the whole day). `DriverPerformancePanel` is a read-only panel — region combo + two optional `DateTimePicker`s (`ShowCheckBox`, unchecked = no bound) + Generate + grid + Back, **no Save/Update/Delete**; it only gathers the filters and binds the SP's result set (`DataTable.Load`). Wired into Data management as the "📊 Driver Performance Report" button on `DispatcherHomePanel`. Reports are **DB-only** — they never call an external service (keep them that way). `scripts/seed_data.sql` seeds a few inbound `Message` rows so response time is populated on a fresh build. A report **is not a UC or an entity** — like Login/Settings it is a technical screen and must not appear in the class/UC diagrams.

### Complex UC flows (Phase 9) — bundling deferred
Phase 9's orchestrated-transaction requirement is **already satisfied** by the dual-entity transactional transitions (`sp_Offer_accept` updates `Offer` **and** `Trip` in one `BEGIN TRAN…COMMIT`, the C# verb method mirroring both in memory) plus the full board dispatch flow. The one remaining sub-flow — UC17 "packages trips for top driver (no time conflicts)" = **multi-ride bundling** (group an eligible driver's trips, one `Offer` per trip, on-time + ≤5h Maps-chaining feasibility) — is **deferred pending a course-director design decision** and must **not** be implemented without explicit user confirmation. Plan: `C:\Users\Dan Azaryad\.claude\plans\precious-wandering-cerf.md` (in-memory grouping, no schema change, offline fallback preserved).

### Visual design language (Phase 10)
One source of truth: **`UiTheme.cs`**. Every panel calls `UiTheme.Apply(this)` once after `InitializeComponent()`; it walks the control tree and styles by control type/role. It is **appearance only** — it never changes event wiring, control positions/sizes, or the data a control shows (no behaviour change). **New panels inherit the look for free:** build the Designer as usual, then add `UiTheme.Apply(this)` in the constructor.

| Token | Hex | Use |
|---|---|---|
| Primary | `#2563EB` | brand — titles, primary buttons, grid selection |
| PrimaryDark | `#1D4ED8` | primary hover/pressed |
| Accent | `#16A34A` | **positive** actions (Send offer, Accept, Confirm, Save) |
| Danger | `#DC2626` | **destructive** (Delete, Decline) |
| Warning | `#D97706` | **caution** (Requeue, Reject, Timeout) |
| PageBg | `#F4F6F8` | page background |
| Surface | `#FFFFFF` | cards, inputs, grids |
| TextDark | `#1F2937` | body text + grid-header background |
| TextMuted | `#6B7280` | secondary / summary text |
| Border | `#D1D5DB` | input + grid borders |

- **Fonts:** Segoe UI — Title 20 Bold, Strong/section 10 Bold, Body 10 Regular.
- **Buttons (flat):** Primary = filled Primary/white; Positive = filled Accent/white; Danger/Warning = white with coloured text+border; Secondary (Back/Clear/Refresh/Logout/Manage) = white, Primary text, neutral border. Hover/pressed tints applied. **Button role is inferred from each button's existing `ForeColor`** (`DarkGreen`→positive, `Firebrick`→danger, `DarkOrange`→warning), so the panels' original semantics are preserved without renaming — **for new colour-coded buttons keep setting those same `ForeColor`s in the Designer** and `Apply` maps them to the palette.
- **Grids:** dark header (`#1F2937`, white bold), zebra rows (`#F3F6FB`), flat single-horizontal cell borders, 28 px rows, blue selection. Use `UiTheme.StyleGrid(grid)` if styling a grid created at runtime.
- **Spacing:** 24 px page margin; panel title at `(24, 20)`.

### Shared database (Phase 11) — switch runbook (currently on localhost)
The app reads its connection from `app.config` › `connectionStrings` › **`DispatchDB`**. Local dev uses `localhost\SQLEXPRESS` (Integrated Security). To point the whole group at a shared server (BGU central **IEMDBS**, or an **Azure SQL free-tier** DB) with **SQL authentication** — config + DDL only, **no code change**:
1. Edit `DispatchDB` in `ExternalDriverDispatch/app.config` to SQL auth:
   `Server=<server>;Initial Catalog=ExternalDriverDispatch;User ID=<user>;Password=<pwd>;TrustServerCertificate=True;` (Azure: `Server=tcp:<name>.database.windows.net,1433;…;Encrypt=True`). **Never commit real credentials.**
2. Create the schema on the shared server, in order: `scripts/create_database.sql` → `scripts/stored_procedures.sql` → `scripts/seed_data.sql` (all ASCII English — never run a Hebrew `.sql` through `sqlcmd` without `-f 65001`). `scripts/migration_add_services.sql` is **only** for upgrading a pre-services DB; a fresh `create_database.sql` is already the full schema.
3. Rebuild (the build copies `app.config` → the output `*.config`) and run; verify the board loads trips and the Driver Performance report renders.

State of play: **deferred** — kept on `localhost\SQLEXPRESS`; shared-server credentials not yet provided.

### Language: English UI + data (group decision, overrides the Hebrew-UI convention)
For submission coherence with the English class diagram, the **app UI and seed data were switched to English (LTR)** — overriding the `PATTERNS.md`/Language-Conventions "UI Hebrew" rule for this project's app layer. All panels set `RightToLeft = No`; `mainForm` is LTR; enum display helpers and messages are English; `scripts/seed_data.sql` uses ASCII English values. (Background: the original Hebrew seed was loaded through `sqlcmd` with a codepage mismatch and stored mojibake in the NVARCHAR columns; the live rows were repaired in place with English values. Re-seed only via the MCP or an ASCII file — never re-run a UTF-8 Hebrew `.sql` through `sqlcmd` without `-f 65001`.) `mainForm` is sized 1150×680 to fit the board.

### MCP `execute_sql` — operational notes
The `mssql` MCP runs each call as one batch via `ExecuteNonQuery`-style execution and is connected to `master`:
- A batch that **starts with `USE ExternalDriverDispatch;`** returns no result rows (reports `Rows affected: -1`). To **read** and see rows, send a **single statement that starts with `SELECT`** using fully-qualified names, e.g. `SELECT ... FROM ExternalDriverDispatch.dbo.Trip ...`. This still targets the correct DB explicitly (honouring the no-wrong-DB rule).
- `CREATE PROCEDURE` must be the first statement in its batch and `GO` is not understood, so create procs via `USE ExternalDriverDispatch; EXEC(N'CREATE OR ALTER PROCEDURE ...')` (one `EXEC` per proc; double the single quotes inside).

### Build / run environment
The **.NET 8 SDK is now installed system-wide** and `dotnet` **is on PATH** (`C:\Program Files\dotnet\dotnet.exe`, `dotnet --version` → 8.0.x). The earlier note ("no system-wide install / bundled VS Code SDK only") is **obsolete** — the permanent fix was applied.

- Build: `dotnet build ExternalDriverDispatch.sln` (targets `net8.0-windows`; verified **0 warnings / 0 errors**). **Kill any running `ExternalDriverDispatch.exe` before rebuilding** (the exe locks its output files).
- Run: the built `.exe` lives at `ExternalDriverDispatch\bin\Debug\net8.0-windows\ExternalDriverDispatch.exe` and runs standalone (the system-wide Windows Desktop runtime is present).
- **`app.ico` (fixed):** `mainForm.Designer.cs` loads the window icon at runtime via `new Icon(BaseDirectory + "app.ico")`, so `app.ico` **must** be copied to the output folder. The `.csproj` `Content` item now sets `CopyToOutputDirectory=PreserveNewest`; without it the app threw `FileNotFoundException` at startup (before any panel code ran).
- The connection string lives in `ExternalDriverDispatch/app.config` (`DispatchDB` → `localhost\SQLEXPRESS`, `Initial Catalog=ExternalDriverDispatch`).

> Gotcha (fixed): in a `UserControl`/`Control` subclass, the entity type `Region` collides with the inherited `Control.Region` property. Call its statics fully-qualified: `ExternalDriverDispatch.Region.seekRegion(...)`.
