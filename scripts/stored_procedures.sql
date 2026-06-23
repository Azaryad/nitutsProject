-- External Driver Dispatch System — Stored Procedures
-- Group 17, SAD course, Ben-Gurion University
-- Mechanical CRUD only. No business logic. No state transitions.
-- Every SP begins with USE ExternalDriverDispatch.

USE ExternalDriverDispatch;
GO

-- ============================================================
-- Region
-- ============================================================

CREATE OR ALTER PROCEDURE sp_Region_create
    @region_id  INT,
    @name       NVARCHAR(50),
    @country    NVARCHAR(50),
    @city       NVARCHAR(50),
    @createdAt  DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Region (region_id, name, country, city, createdAt)
    VALUES (@region_id, @name, @country, @city, @createdAt);
END
GO

CREATE OR ALTER PROCEDURE sp_Region_update
    @region_id  INT,
    @name       NVARCHAR(50),
    @country    NVARCHAR(50),
    @city       NVARCHAR(50),
    @createdAt  DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Region
    SET name      = @name,
        country   = @country,
        city      = @city,
        createdAt = @createdAt
    WHERE region_id = @region_id;
END
GO

CREATE OR ALTER PROCEDURE sp_Region_delete
    @region_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Region WHERE region_id = @region_id;
END
GO

CREATE OR ALTER PROCEDURE sp_Region_get_all
AS
BEGIN
    SET NOCOUNT ON;
    SELECT region_id, name, country, city, createdAt
    FROM Region;
END
GO

CREATE OR ALTER PROCEDURE sp_Region_get_by_id
    @region_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT region_id, name, country, city, createdAt
    FROM Region
    WHERE region_id = @region_id;
END
GO

-- ============================================================
-- ExternalDriver
-- ============================================================

CREATE OR ALTER PROCEDURE sp_ExternalDriver_create
    @driver_id          INT,
    @drivercode         NVARCHAR(50),
    @name               NVARCHAR(50),
    @phone              NVARCHAR(50),
    @homeCity           NVARCHAR(50),
    @vehicleType        NVARCHAR(20),
    @worksShabbat       BIT,
    @worksNights        BIT,
    @worksLongDistance  BIT,
    @active             BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ExternalDriver
        (driver_id, drivercode, name, phone, homeCity, vehicleType,
         worksShabbat, worksNights, worksLongDistance, active)
    VALUES
        (@driver_id, @drivercode, @name, @phone, @homeCity, @vehicleType,
         @worksShabbat, @worksNights, @worksLongDistance, @active);
END
GO

CREATE OR ALTER PROCEDURE sp_ExternalDriver_update
    @driver_id          INT,
    @drivercode         NVARCHAR(50),
    @name               NVARCHAR(50),
    @phone              NVARCHAR(50),
    @homeCity           NVARCHAR(50),
    @vehicleType        NVARCHAR(20),
    @worksShabbat       BIT,
    @worksNights        BIT,
    @worksLongDistance  BIT,
    @active             BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ExternalDriver
    SET drivercode        = @drivercode,
        name              = @name,
        phone             = @phone,
        homeCity          = @homeCity,
        vehicleType       = @vehicleType,
        worksShabbat      = @worksShabbat,
        worksNights       = @worksNights,
        worksLongDistance = @worksLongDistance,
        active            = @active
    WHERE driver_id = @driver_id;
END
GO

CREATE OR ALTER PROCEDURE sp_ExternalDriver_delete
    @driver_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ExternalDriver WHERE driver_id = @driver_id;
END
GO

CREATE OR ALTER PROCEDURE sp_ExternalDriver_get_all
AS
BEGIN
    SET NOCOUNT ON;
    SELECT driver_id, drivercode, name, phone, homeCity, vehicleType,
           worksShabbat, worksNights, worksLongDistance, active
    FROM ExternalDriver;
END
GO

CREATE OR ALTER PROCEDURE sp_ExternalDriver_get_by_id
    @driver_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT driver_id, drivercode, name, phone, homeCity, vehicleType,
           worksShabbat, worksNights, worksLongDistance, active
    FROM ExternalDriver
    WHERE driver_id = @driver_id;
END
GO

-- ============================================================
-- ExternalDriverRegion  (junction table)
-- ============================================================

CREATE OR ALTER PROCEDURE sp_ExternalDriverRegion_create
    @driver_id INT,
    @region_id INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ExternalDriverRegion (driver_id, region_id)
    VALUES (@driver_id, @region_id);
END
GO

CREATE OR ALTER PROCEDURE sp_ExternalDriverRegion_delete
    @driver_id INT,
    @region_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ExternalDriverRegion
    WHERE driver_id = @driver_id AND region_id = @region_id;
END
GO

CREATE OR ALTER PROCEDURE sp_ExternalDriverRegion_get_all
AS
BEGIN
    SET NOCOUNT ON;
    SELECT driver_id, region_id FROM ExternalDriverRegion;
END
GO

CREATE OR ALTER PROCEDURE sp_ExternalDriverRegion_get_by_driver
    @driver_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT driver_id, region_id
    FROM ExternalDriverRegion
    WHERE driver_id = @driver_id;
END
GO

-- ============================================================
-- Trip
-- ============================================================

CREATE OR ALTER PROCEDURE sp_Trip_create
    @trip_id            INT,
    @externalBookingId  NVARCHAR(50),
    @pickupAddress      NVARCHAR(50),
    @dropoffAddress     NVARCHAR(50),
    @pickupCity         NVARCHAR(50),
    @dropoffCity        NVARCHAR(50),
    @pickupTime         DATETIME2,
    @numPassengers      INT,
    @vehicleType        NVARCHAR(20),
    @priceToDriver      DECIMAL(10,2),
    @status             NVARCHAR(20),
    @createdAt          DATETIME2,
    @region_id          INT = NULL,
    -- defaults so callers that predate Maps enrichment (e.g. seed_data.sql) still work;
    -- the C# always passes explicit values.
    @distanceKm                FLOAT = 0,
    @estimatedDurationMinutes  INT   = 60,
    @offerCounter              INT   = 0
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Trip
        (trip_id, externalBookingId, pickupAddress, dropoffAddress,
         pickupCity, dropoffCity, pickupTime, numPassengers, vehicleType,
         priceToDriver, status, createdAt, region_id,
         distanceKm, estimatedDurationMinutes, offerCounter)
    VALUES
        (@trip_id, @externalBookingId, @pickupAddress, @dropoffAddress,
         @pickupCity, @dropoffCity, @pickupTime, @numPassengers, @vehicleType,
         @priceToDriver, @status, @createdAt, @region_id,
         @distanceKm, @estimatedDurationMinutes, @offerCounter);
END
GO

CREATE OR ALTER PROCEDURE sp_Trip_update
    @trip_id            INT,
    @externalBookingId  NVARCHAR(50),
    @pickupAddress      NVARCHAR(50),
    @dropoffAddress     NVARCHAR(50),
    @pickupCity         NVARCHAR(50),
    @dropoffCity        NVARCHAR(50),
    @pickupTime         DATETIME2,
    @numPassengers      INT,
    @vehicleType        NVARCHAR(20),
    @priceToDriver      DECIMAL(10,2),
    @status             NVARCHAR(20),
    @createdAt          DATETIME2,
    @region_id          INT = NULL,
    @distanceKm                FLOAT,
    @estimatedDurationMinutes  INT,
    @offerCounter              INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Trip
    SET externalBookingId = @externalBookingId,
        pickupAddress     = @pickupAddress,
        dropoffAddress    = @dropoffAddress,
        pickupCity        = @pickupCity,
        dropoffCity       = @dropoffCity,
        pickupTime        = @pickupTime,
        numPassengers     = @numPassengers,
        vehicleType       = @vehicleType,
        priceToDriver     = @priceToDriver,
        status            = @status,
        createdAt         = @createdAt,
        region_id         = @region_id,
        distanceKm                = @distanceKm,
        estimatedDurationMinutes  = @estimatedDurationMinutes,
        offerCounter              = @offerCounter
    WHERE trip_id = @trip_id;
END
GO

CREATE OR ALTER PROCEDURE sp_Trip_delete
    @trip_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Trip WHERE trip_id = @trip_id;
END
GO

CREATE OR ALTER PROCEDURE sp_Trip_get_all
AS
BEGIN
    SET NOCOUNT ON;
    SELECT trip_id, externalBookingId, pickupAddress, dropoffAddress,
           pickupCity, dropoffCity, pickupTime, numPassengers, vehicleType,
           priceToDriver, status, createdAt, region_id,
           distanceKm, estimatedDurationMinutes, offerCounter
    FROM Trip;
END
GO

CREATE OR ALTER PROCEDURE sp_Trip_get_by_id
    @trip_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT trip_id, externalBookingId, pickupAddress, dropoffAddress,
           pickupCity, dropoffCity, pickupTime, numPassengers, vehicleType,
           priceToDriver, status, createdAt, region_id,
           distanceKm, estimatedDurationMinutes, offerCounter
    FROM Trip
    WHERE trip_id = @trip_id;
END
GO

-- ============================================================
-- Offer
-- ============================================================

CREATE OR ALTER PROCEDURE sp_Offer_create
    @offer_id         INT,
    @trip_id          INT,
    @driver_id        INT,
    @sentAt           DATETIME2,
    @expiresAt        DATETIME2,
    @status           NVARCHAR(20),
    @driverReplyText  NVARCHAR(MAX),
    @aiInterpretation NVARCHAR(MAX),
    @rankPosition     INT,
    @rankReason       NVARCHAR(200) = NULL   -- default so seed_data.sql (pre-ranking) still works
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Offer
        (offer_id, trip_id, driver_id, sentAt, expiresAt, status,
         driverReplyText, aiInterpretation, rankPosition, rankReason)
    VALUES
        (@offer_id, @trip_id, @driver_id, @sentAt, @expiresAt, @status,
         @driverReplyText, @aiInterpretation, @rankPosition, @rankReason);
END
GO

CREATE OR ALTER PROCEDURE sp_Offer_update
    @offer_id         INT,
    @trip_id          INT,
    @driver_id        INT,
    @sentAt           DATETIME2,
    @expiresAt        DATETIME2,
    @status           NVARCHAR(20),
    @driverReplyText  NVARCHAR(MAX),
    @aiInterpretation NVARCHAR(MAX),
    @rankPosition     INT,
    @rankReason       NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Offer
    SET trip_id          = @trip_id,
        driver_id        = @driver_id,
        sentAt           = @sentAt,
        expiresAt        = @expiresAt,
        status           = @status,
        driverReplyText  = @driverReplyText,
        aiInterpretation = @aiInterpretation,
        rankPosition     = @rankPosition,
        rankReason       = @rankReason
    WHERE offer_id = @offer_id;
END
GO

CREATE OR ALTER PROCEDURE sp_Offer_delete
    @offer_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Offer WHERE offer_id = @offer_id;
END
GO

CREATE OR ALTER PROCEDURE sp_Offer_get_all
AS
BEGIN
    SET NOCOUNT ON;
    SELECT offer_id, trip_id, driver_id, sentAt, expiresAt, status,
           driverReplyText, aiInterpretation, rankPosition, rankReason
    FROM Offer;
END
GO

CREATE OR ALTER PROCEDURE sp_Offer_get_by_id
    @offer_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT offer_id, trip_id, driver_id, sentAt, expiresAt, status,
           driverReplyText, aiInterpretation, rankPosition, rankReason
    FROM Offer
    WHERE offer_id = @offer_id;
END
GO

-- ============================================================
-- State-transition stored procedures (Phase 7 — state machine)
-- Behavior grounded in docs/insightFromRealProject/dispatch_flow.md.
-- Each runs inside BEGIN TRAN ... COMMIT with ROLLBACK on error.
-- Guards (allowed source states) are also enforced in C# before the call.
-- These do NOT replace the mechanical CRUD SPs above.
-- ============================================================

-- Trip.assignRegion(): open|assigned_to_region -> assigned_to_region  (dispatcher assigned the region)
CREATE OR ALTER PROCEDURE sp_Trip_assign_region
    @trip_id   INT,
    @region_id INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;
        UPDATE Trip SET region_id = @region_id, status = N'assigned_to_region'
        WHERE trip_id = @trip_id AND status IN (N'open', N'assigned_to_region');
        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

-- Trip.updateOfferCount(): bump the per-trip outreach counter (attractiveness metric)
CREATE OR ALTER PROCEDURE sp_Trip_update_offer_count
    @trip_id      INT,
    @offerCounter INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Trip SET offerCounter = @offerCounter WHERE trip_id = @trip_id;
END
GO

-- Trip.offer(): assigned_to_region -> offered  (dispatcher sent the WhatsApp offer)
CREATE OR ALTER PROCEDURE sp_Trip_offer
    @trip_id INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;
        UPDATE Trip SET status = N'offered'
        WHERE trip_id = @trip_id AND status = N'assigned_to_region';
        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

-- Trip.confirm(): offered -> confirmed  (a driver accepted)
CREATE OR ALTER PROCEDURE sp_Trip_confirm
    @trip_id INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;
        UPDATE Trip SET status = N'confirmed'
        WHERE trip_id = @trip_id AND status = N'offered';
        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

-- Trip.requeue(): offered -> assigned_to_region  (offer rejected/timed out, re-queue to next driver; region kept)
CREATE OR ALTER PROCEDURE sp_Trip_requeue
    @trip_id INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;
        UPDATE Trip SET status = N'assigned_to_region'
        WHERE trip_id = @trip_id AND status = N'offered';
        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

-- Offer.accept(): offer -> accepted AND its trip -> confirmed  (binding approval)
CREATE OR ALTER PROCEDURE sp_Offer_accept
    @offer_id INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;
        DECLARE @trip_a INT;
        SELECT @trip_a = trip_id FROM Offer WHERE offer_id = @offer_id;
        UPDATE Offer SET status = N'accepted' WHERE offer_id = @offer_id;
        UPDATE Trip  SET status = N'confirmed' WHERE trip_id = @trip_a;
        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

-- Offer.reject(): offer -> rejected AND its trip -> open  (driver declined; re-queue follows)
CREATE OR ALTER PROCEDURE sp_Offer_reject
    @offer_id INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;
        DECLARE @trip_r INT;
        SELECT @trip_r = trip_id FROM Offer WHERE offer_id = @offer_id;
        UPDATE Offer SET status = N'rejected'            WHERE offer_id = @offer_id;
        UPDATE Trip  SET status = N'assigned_to_region'  WHERE trip_id  = @trip_r;
        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

-- Trip.flagManualAssignment(): {assigned_to_region | offered} -> manual_assignment
-- (all eligible drivers exhausted, or the 6h-before-pickup deadline reached without assignment)
CREATE OR ALTER PROCEDURE sp_Trip_manual_assignment
    @trip_id INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;
        UPDATE Trip SET status = N'manual_assignment'
        WHERE trip_id = @trip_id AND status IN (N'assigned_to_region', N'offered');
        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

-- Trip.moveToArchive(): confirmed -> completed
CREATE OR ALTER PROCEDURE sp_Trip_archive
    @trip_id INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;
        UPDATE Trip SET status = N'completed'
        WHERE trip_id = @trip_id AND status = N'confirmed';
        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

-- Offer.cancel(): pending|pending_approval -> cancelled  (trip escalated to manual assignment)
CREATE OR ALTER PROCEDURE sp_Offer_cancel
    @offer_id INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;
        UPDATE Offer SET status = N'cancelled'
        WHERE offer_id = @offer_id AND status IN (N'pending', N'pending_approval');
        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

-- Offer.markPendingApproval(): pending -> pending_approval  (Stage 5: WhatsApp YES = soft intent; trip stays offered)
CREATE OR ALTER PROCEDURE sp_Offer_pending_approval
    @offer_id INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;
        UPDATE Offer SET status = N'pending_approval'
        WHERE offer_id = @offer_id AND status = N'pending';
        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

-- Offer.timeout(): offer -> timeout AND its trip -> open  (Stage 7: no reply in time; re-queue follows)
CREATE OR ALTER PROCEDURE sp_Offer_timeout
    @offer_id INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;
        DECLARE @trip_t INT;
        SELECT @trip_t = trip_id FROM Offer WHERE offer_id = @offer_id;
        UPDATE Offer SET status = N'timeout'             WHERE offer_id = @offer_id;
        UPDATE Trip  SET status = N'assigned_to_region'  WHERE trip_id  = @trip_t;
        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

-- ============================================================
-- Message  (WhatsApp conversation audit trail) — mechanical CRUD
-- ============================================================

CREATE OR ALTER PROCEDURE sp_Message_create
    @message_id       INT,
    @driver_id        INT,
    @direction        NVARCHAR(20),
    @waMessageId      NVARCHAR(100),
    @body             NVARCHAR(MAX),
    @timestamp        DATETIME2,
    @related_offer_id INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Message
        (message_id, driver_id, direction, waMessageId, body, timestamp, related_offer_id)
    VALUES
        (@message_id, @driver_id, @direction, @waMessageId, @body, @timestamp, @related_offer_id);
END
GO

CREATE OR ALTER PROCEDURE sp_Message_update
    @message_id       INT,
    @driver_id        INT,
    @direction        NVARCHAR(20),
    @waMessageId      NVARCHAR(100),
    @body             NVARCHAR(MAX),
    @timestamp        DATETIME2,
    @related_offer_id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Message
    SET driver_id        = @driver_id,
        direction        = @direction,
        waMessageId      = @waMessageId,
        body             = @body,
        timestamp        = @timestamp,
        related_offer_id = @related_offer_id
    WHERE message_id = @message_id;
END
GO

CREATE OR ALTER PROCEDURE sp_Message_delete
    @message_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Message WHERE message_id = @message_id;
END
GO

CREATE OR ALTER PROCEDURE sp_Message_get_all
AS
BEGIN
    SET NOCOUNT ON;
    SELECT message_id, driver_id, direction, waMessageId, body, timestamp, related_offer_id
    FROM Message;
END
GO

CREATE OR ALTER PROCEDURE sp_Message_get_by_id
    @message_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT message_id, driver_id, direction, waMessageId, body, timestamp, related_offer_id
    FROM Message
    WHERE message_id = @message_id;
END
GO

-- ============================================================
-- Reports (Phase 8) — read-only aggregations.
-- These are the only SPs that JOIN across tables and GROUP BY.
-- No writes, no business logic, no external-service calls (DB-only).
-- ============================================================

-- sp_DriverPerformance
-- Per-driver outreach funnel over a period, optionally narrowed to one
-- dispatch region. Joins each outreach attempt (Offer) to the driver and to
-- the trip's region; avg response time is derived from the inbound WhatsApp
-- audit trail (Message), so drivers who never replied show NULL (not zero).
--   @region_id  NULL = all regions; otherwise the trip's region_id
--   @from       NULL = no lower bound on Offer.sentAt
--   @to         NULL = no upper bound; otherwise inclusive of the whole @to day
CREATE OR ALTER PROCEDURE sp_DriverPerformance
    @region_id INT      = NULL,
    @from      DATETIME2 = NULL,
    @to        DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        d.drivercode                                                            AS DriverCode,
        d.name                                                                  AS DriverName,
        d.homeCity                                                              AS HomeCity,
        COUNT(o.offer_id)                                                       AS OffersReceived,
        SUM(CASE WHEN o.status = 'accepted' THEN 1 ELSE 0 END)                  AS Accepted,
        SUM(CASE WHEN o.status = 'rejected' THEN 1 ELSE 0 END)                  AS Rejected,
        SUM(CASE WHEN o.status IN ('timeout','approval_timeout') THEN 1 ELSE 0 END) AS TimedOut,
        CAST(100.0 * SUM(CASE WHEN o.status = 'accepted' THEN 1 ELSE 0 END)
             / NULLIF(COUNT(o.offer_id), 0) AS DECIMAL(5,1))                    AS AcceptanceRatePct,
        CAST(AVG(CAST(reply.response_minutes AS FLOAT)) AS DECIMAL(6,1))        AS AvgResponseMinutes
    FROM ExternalDriver d
    INNER JOIN Offer o ON o.driver_id = d.driver_id
    INNER JOIN Trip  t ON t.trip_id   = o.trip_id
    OUTER APPLY (
        -- first inbound reply for this offer; NULL when the driver never replied
        SELECT DATEDIFF(MINUTE, o.sentAt, MIN(m.timestamp)) AS response_minutes
        FROM Message m
        WHERE m.related_offer_id = o.offer_id
          AND m.direction = 'inbound'
    ) reply
    WHERE (@region_id IS NULL OR t.region_id = @region_id)
      AND (@from IS NULL OR o.sentAt >= @from)
      AND (@to   IS NULL OR o.sentAt <  DATEADD(DAY, 1, CAST(@to AS DATE)))
    GROUP BY d.driver_id, d.drivercode, d.name, d.homeCity
    ORDER BY AcceptanceRatePct DESC, OffersReceived DESC;
END
GO
