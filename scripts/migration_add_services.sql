-- Migration — add the three external-service fields + the Message audit table.
-- Run ONCE against an existing ExternalDriverDispatch database (create_database.sql already
-- reflects the final schema for fresh builds). After this, re-run stored_procedures.sql so the
-- changed/added procedures match the new columns.
--
-- Adds:
--   Trip.distanceKm, Trip.estimatedDurationMinutes   (Service 1 — Google Maps)
--   Offer.rankReason                                 (Service 2 — Claude AI ranking)
--   Message table + MessageDirection check           (Service 3 — WhatsApp audit trail)

USE ExternalDriverDispatch;
GO

-- Trip: Maps-derived columns (default to the offline fallback so existing rows stay valid)
IF COL_LENGTH('dbo.Trip', 'distanceKm') IS NULL
    ALTER TABLE Trip ADD distanceKm FLOAT NOT NULL DEFAULT 0;
GO
IF COL_LENGTH('dbo.Trip', 'estimatedDurationMinutes') IS NULL
    ALTER TABLE Trip ADD estimatedDurationMinutes INT NOT NULL DEFAULT 60;
GO

-- Offer: one-line AI rank justification
IF COL_LENGTH('dbo.Offer', 'rankReason') IS NULL
    ALTER TABLE Offer ADD rankReason NVARCHAR(200) NULL;
GO

-- Message: WhatsApp conversation audit trail
IF OBJECT_ID('dbo.Message', 'U') IS NULL
CREATE TABLE Message (
    message_id       INT           NOT NULL PRIMARY KEY,
    driver_id        INT           NOT NULL,
    direction        NVARCHAR(20)  NOT NULL
        CHECK (direction IN ('inbound', 'outbound')),
    waMessageId      NVARCHAR(100) NULL,
    body             NVARCHAR(MAX) NOT NULL,
    timestamp        DATETIME2     NOT NULL,
    related_offer_id INT           NULL,
    FOREIGN KEY (driver_id)        REFERENCES ExternalDriver (driver_id) ON DELETE NO ACTION ON UPDATE NO ACTION,
    FOREIGN KEY (related_offer_id) REFERENCES Offer          (offer_id)  ON DELETE NO ACTION ON UPDATE NO ACTION
);
GO
