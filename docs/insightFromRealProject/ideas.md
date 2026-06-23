# Ideas for Future Consideration

These were noted during MVP build but deliberately excluded from scope.

---

## ⚠️ Open Decision — Billing & Monthly Driver Reports (2026-05-07)

**Status: Unresolved. No code changes on our side until decision is finalized.**

During the MVP review, the operations team indicated that **billing and driver payment reporting should remain within Ride Control (RC)**, not in this system. RC's development team has been asked to build a per-driver monthly report within their platform.

**Our side:** The monthly report feature (`app/api/reports.py`, `app/services/monthly_report.py`) was built, is functional, and generates Hebrew PDF reports per driver. It remains in the codebase untouched — do not remove it until a final decision is made.

**RC side:** RC dev team has been tasked with building equivalent per-driver reports. Timeline and spec not yet confirmed.

**Possible outcomes:**
- RC ships their report → we deprecate our PDF feature; keep the 7-day forwarding window logic for driver replies.
- RC ships partial → we keep ours as a supplement.
- Decision reversed → our feature becomes the primary path with no changes needed.

**Do not build further billing/payment UI** on our side until ops team finalizes.

---

- **Telegram/email fallback for Michel** — currently WhatsApp only; hook exists in `dispatch.py:_notify_michel`
- **Driver rating / performance score** — track acceptance rate, punctuality, customer feedback to improve Claude ranking
- **Multi-language driver messages** — currently Hebrew only; could detect driver language preference and generate in Russian/Arabic/English
- **Booking.com / HolidayTaxis status sync** — `POST /reportstatus` is available in supplier API; wire it to trip lifecycle events
- **Web push notifications for dispatcher** — replace poll-based dashboard refresh with WebSocket or SSE
- **Driver self-registration portal** — form that creates a driver record and pushes to supplier API
- **Postgres migration** — SQLAlchemy is already Postgres-ready; just swap `DATABASE_URL`
- **Rate limiting on `/api/trips/ingest`** — add API key auth so only the main OS can push trips
- **Conflict detection improvement** — currently uses fixed 90-min buffer; improve with actual trip duration from Maps API
- **Shabbat exact times** — integrate `hdate` library for precise astronomical sunset per date/location
