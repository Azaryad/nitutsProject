# New-session prompt — finish Phases 8–11 and wrap up the project

> Paste everything in the block below into a fresh Claude Code session opened at the project root
> (`...\Documents\Claude\Projects\nitutsProject`).

---

```
You are continuing the Group 17 SAD course project: the External Driver Dispatch System
(Wholestay / Transfers TLV), a C# WinForms + SQL Server app. Phases 5–7 are done and we
built well beyond them (Dispatch Board + three external-service integrations). Your job is to
complete the remaining homework phases 8–11 and finish the project as a whole.

== READ FIRST (context, in this order) ==
1. CLAUDE.md (project root) — conventions, entity pattern, load order, the dispatch flow, the
   three external services, the English-UI override, the build/MCP notes. This is authoritative.
2. cloned/LESSON_STEPS.md lines ~699–898 — the exact definitions of Phase 8 (Reports), Phase 9
   (Complex UC Flows), Phase 10 (UI Polish), Phase 11 (Shared DB). cloned/PROMPTS_CHEATSHEET.md
   summarizes them.
3. docs/design/class-diagram.md and docs/00e-use-cases.md — the modeled entities + the 6 UCs.
4. docs/insightFromRealProject/ — the REAL production system for the same company. Mine it for
   DOMAIN INSPIRATION ONLY (e.g. report ideas, the michel/monthly report, the dispatch flow) —
   it is NOT architectural guidance; any conflict resolves in favour of cloned/PATTERNS.md.

== CURRENT STATUS ==
- Done: Phases 5–7 (entities Region/ExternalDriver/Trip/Offer/Message, CRUD panels, state
  machine with transactional SPs), plus the Dispatch Board and the 3 services.
- Phase 8 (Reports): MISSING — no aggregated report SP/panel exists yet. (Biggest gap.)
- Phase 9 (Complex UC Flows): LARGELY done via the dispatch flow + dual-entity transactional
  transitions (sp_Offer_accept = Offer+Trip in one TRAN). The one open sub-flow is UC17
  "packages trips for top driver (no time conflicts)" = multi-ride BUNDLING + Maps chaining
  feasibility. A plan exists at C:\Users\<user>\.claude\plans\precious-wandering-cerf.md, but it
  is PENDING a design decision with the course directors — DO NOT implement bundling until the
  user explicitly confirms.
- Phase 10 (UI Polish): PARTIAL — panels are functional/decent but there's no deliberate polish
  pass and no documented design language.
- Phase 11 (Shared DB): SET UP, NOT EXERCISED — app.config has a commented-out BGU "IEMDBS"
  connection string; we've only run against localhost\SQLEXPRESS.

== NON-NEGOTIABLES — preserve these ==
- API / OFFLINE-FIRST NUANCE (critical): the three services (Google Maps, Claude AI, WhatsApp
  with Meta+Twilio providers) each sit behind a C# interface in ExternalDriverDispatch/Services/
  with a DETERMINISTIC OFFLINE FALLBACK. Offline/live is PER SERVICE (each *.Enabled flag in
  app.config; there is NO global master switch), all default OFF, so the whole app must run
  end-to-end with no keys and no internet. ServiceFactory returns the real impl only when that
  service is Enabled AND its creds are present; otherwise the fallback. WhatsApp.Provider selects
  meta | twilio. Settings (SettingsPanel) edits these via Config.Save → app.config. Any new
  feature must keep this guarantee intact: a missing key downgrades a feature, never crashes the
  app. (Reports are DB-only and don't call external APIs — keep them that way.)
- ENGLISH UI + DATA (group override): the LESSON_STEPS prompts say "Hebrew, RTL" — IGNORE that
  for this project. All UI/data is English, LTR (RightToLeft=No). Generate new panels English+LTR,
  matching the existing MessagePanel/board style. Do NOT reintroduce Hebrew.
- Course architecture: entity pattern (one file per entity, is_new constructor, init/seek/
  getNext); PKs assigned in C# (max+1, no IDENTITY); ALL DB access via stored procedures (no
  ad-hoc SQL); single-window panel nav (mainForm.showPanel, every panel has Back, no extra Forms);
  Settings and Login are NFR/technical screens — NOT UCs or entities, keep them out of the class/
  UC diagrams. Every SQL batch begins with USE ExternalDriverDispatch;.
- DB name ExternalDriverDispatch. Schema source scripts/create_database.sql (don't re-run on the
  live DB — write a migration like scripts/migration_add_services.sql). SPs in
  scripts/stored_procedures.sql (CREATE OR ALTER, re-runnable).
- Build: `dotnet build ExternalDriverDispatch.sln` (dotnet is on PATH; targets net8.0-windows).
  Kill any running ExternalDriverDispatch.exe before rebuilding. Aim for 0 warnings / 0 errors.
- MCP mssql (connected to master): to READ, send a single SELECT with fully-qualified names
  (SELECT ... FROM ExternalDriverDispatch.dbo.X). To create SPs, use
  USE ExternalDriverDispatch; EXEC(N'CREATE OR ALTER PROCEDURE ...') (one EXEC per proc, double
  the inner single quotes).

== WORK TO DO ==
Phase 8 — Reports (do this first; it's the clear miss):
  - Pick 1–2 manager/dispatcher reports (mine docs/insightFromRealProject/ for ideas). Strong
    candidates: a Driver Performance report (per driver: offers received / accepted / rejected /
    timed-out / acceptance-rate / avg response time) and/or a Dispatch Summary by region+date
    range (trip counts by status). These need JOINs across Offer↔ExternalDriver↔Trip↔Region and
    GROUP BY — real aggregation, not single-table SELECTs.
  - Build sp_<reportName> (append to scripts/stored_procedures.sql, run via MCP) returning the
    aggregated rows. Then a read-only <ReportName>Panel (English, LTR): filter controls at top
    (DateTimePickers/ComboBoxes), a Generate button, a DataGridView below, a Back button — NO
    Save/Update/Delete. Wire it into Data management (DispatcherHomePanel) as a new button.
  - Verify: spot-check a report row by re-deriving it from raw rows via MCP.
Phase 9 — only the UC17 bundling sub-flow remains, and it is BLOCKED pending the course-director
  decision. Confirm with the user before touching it; the plan is precious-wandering-cerf.md
  (in-memory grouping, one Offer per trip, on-time + ≤5h chaining via a second Maps use).
Phase 10 — UI polish: one deliberate pass. Improve visual hierarchy/spacing/fonts/color across
  the panels (Designer.cs only — no behavior change), keep English+LTR, then document the design
  language (hex palette, fonts, spacing, button styling) as a "Visual Design" section in CLAUDE.md
  so future panels inherit it. Show proposed Designer diffs before applying big changes.
Phase 11 — exercise the shared-DB switch: help switch app.config's DispatchDB to a shared server
  (BGU central "IEMDBS" with SQL authentication — username/password, not Integrated Security; or
  Azure SQL free tier per LESSON_STEPS Option B). You'll need credentials from the user. Create
  the schema there via create_database.sql + stored_procedures.sql + migration_add_services.sql +
  seed_data.sql (ASCII English only — never run a Hebrew .sql through sqlcmd without -f 65001),
  then run the app against it and verify the board + a report load.
Final wrap-up (finish the project as a whole):
  - Coherence/traceability pass: UC specs (docs/00e-use-cases.md) ↔ class diagram
    (docs/design/class-diagram.md) ↔ code ↔ SPs all agree. Make sure the 3 services, the Message
    entity, MessageDirection, and the Trip.distanceKm/estimatedDurationMinutes + Offer.rankReason
    fields are reflected everywhere, and that Settings/Login are absent from the UC/class diagrams.
  - Final clean build (0/0) and an offline end-to-end smoke test (login → board → assign region →
    rank → send offer → reply → accept/forward → a report renders).
  - Produce a short submission checklist of what was delivered per phase.

== METHOD ==
Work phase by phase. Use a todo list. Prefer AskUserQuestion for genuine decisions (report choice,
shared-DB credentials/path, polish direction). Keep the offline-first guarantee and the English-LTR
rule intact at every step. Build and verify after each phase. Don't start bundling without explicit
confirmation.
```

---

*Notes for you (not part of the paste):* the bundling plan lives at
`C:\Users\Dan Azaryad\.claude\plans\precious-wandering-cerf.md`. Phase 8 (Reports) is the cleanest
win and a real gap, so the prompt orders it first.
