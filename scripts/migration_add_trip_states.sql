-- ============================================================
-- Migration: Trip state-machine refinement
--   * status: add 'assigned_to_region', replace 'unassigned' -> 'manual_assignment'
--   * new column offerCounter INT NOT NULL DEFAULT 0 (per-trip outreach count)
-- Safe to run once on an existing ExternalDriverDispatch DB. For a fresh install,
-- create_database.sql already contains the final schema (do not run this as well).
-- Run the updated stored_procedures.sql afterwards to refresh the SPs.
-- ============================================================
USE ExternalDriverDispatch;
GO

-- 1) drop the existing inline CHECK on Trip.status (auto-generated name)
DECLARE @ck SYSNAME;
SELECT @ck = cc.name
FROM sys.check_constraints cc
WHERE cc.parent_object_id = OBJECT_ID('dbo.Trip')
  AND cc.definition LIKE '%status%'
  AND cc.definition LIKE '%unassigned%';
IF @ck IS NOT NULL
    EXEC('ALTER TABLE dbo.Trip DROP CONSTRAINT ' + @ck);
GO

-- 2) add the offerCounter column if it does not exist yet
IF COL_LENGTH('dbo.Trip', 'offerCounter') IS NULL
    ALTER TABLE dbo.Trip ADD offerCounter INT NOT NULL DEFAULT 0;
GO

-- 3) migrate existing rows: old give-up state -> new manual-assignment state
UPDATE dbo.Trip SET status = N'manual_assignment' WHERE status = N'unassigned';
GO

-- 4) backfill offerCounter from existing offer history (one row per outreach attempt)
UPDATE t
SET t.offerCounter = x.cnt
FROM dbo.Trip t
JOIN (SELECT trip_id, COUNT(*) AS cnt FROM dbo.Offer GROUP BY trip_id) x
  ON x.trip_id = t.trip_id;
GO

-- 5) re-add the CHECK with the new value set (named, so future migrations can find it)
ALTER TABLE dbo.Trip WITH CHECK ADD CONSTRAINT CK_Trip_status
    CHECK (status IN ('open', 'assigned_to_region', 'offered', 'confirmed', 'completed', 'cancelled', 'manual_assignment'));
GO
