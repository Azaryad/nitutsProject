-- External Driver Dispatch System — DDL
-- Group 17, SAD course, Ben-Gurion University
-- Run against: ExternalDriverDispatch
-- Load order enforced: Region → ExternalDriver → ExternalDriverRegion → Trip → Offer

USE ExternalDriverDispatch;

-- ============================================================
-- Region
-- No FK dependencies; must exist before ExternalDriver and Trip.
-- ============================================================
CREATE TABLE Region (
    region_id    INT           NOT NULL PRIMARY KEY,
    name         NVARCHAR(50)  NOT NULL,
    country      NVARCHAR(50)  NOT NULL,
    city         NVARCHAR(50)  NOT NULL,
    createdAt    DATETIME2     NOT NULL
);

-- ============================================================
-- ExternalDriver
-- No FK to Region directly; the many-to-many is in ExternalDriverRegion.
-- ============================================================
CREATE TABLE ExternalDriver (
    driver_id         INT           NOT NULL PRIMARY KEY,
    drivercode        NVARCHAR(50)  NOT NULL UNIQUE,   -- unique code assigned per driver
    name              NVARCHAR(50)  NOT NULL,
    phone             NVARCHAR(50)  NOT NULL,
    homeCity          NVARCHAR(50)  NOT NULL,
    vehicleType       NVARCHAR(20)  NOT NULL
        CHECK (vehicleType IN ('sedan', 'executive_minivan', 'minivan', 'minibus_15', 'minibus_18')),
    worksShabbat      BIT           NOT NULL,
    worksNights       BIT           NOT NULL,
    worksLongDistance BIT           NOT NULL,
    active            BIT           NOT NULL
);

-- ============================================================
-- ExternalDriverRegion  (junction: ExternalDriver * Region)
-- class-diagram: ExternalDriver *..*  Region
-- ============================================================
CREATE TABLE ExternalDriverRegion (
    driver_id  INT  NOT NULL,
    region_id  INT  NOT NULL,
    PRIMARY KEY (driver_id, region_id),
    FOREIGN KEY (driver_id) REFERENCES ExternalDriver (driver_id)
        ON DELETE NO ACTION ON UPDATE NO ACTION,
    FOREIGN KEY (region_id) REFERENCES Region (region_id)
        ON DELETE NO ACTION ON UPDATE NO ACTION
);

-- ============================================================
-- Trip
-- FK to Region (each trip belongs to exactly one region).
-- ============================================================
CREATE TABLE Trip (
    trip_id           INT            NOT NULL PRIMARY KEY,
    externalBookingId NVARCHAR(50)   NOT NULL UNIQUE,  -- comes from Ride Control; should be unique
    -- TODO: verify 50 chars is enough for RC booking IDs
    pickupAddress     NVARCHAR(50)   NOT NULL,
    -- TODO: street addresses may exceed 50 chars; bump to NVARCHAR(200) if needed
    dropoffAddress    NVARCHAR(50)   NOT NULL,
    pickupCity        NVARCHAR(50)   NOT NULL,
    dropoffCity       NVARCHAR(50)   NOT NULL,
    pickupTime        DATETIME2      NOT NULL,
    numPassengers     INT            NOT NULL,
    vehicleType       NVARCHAR(20)   NOT NULL
        CHECK (vehicleType IN ('sedan', 'executive_minivan', 'minivan', 'minibus_15', 'minibus_18')),
    priceToDriver     DECIMAL(10, 2) NOT NULL,
    status            NVARCHAR(20)   NOT NULL
        CHECK (status IN ('open', 'assigned_to_region', 'offered', 'confirmed', 'completed', 'cancelled', 'manual_assignment')),
    createdAt         DATETIME2      NOT NULL,
    region_id         INT            NOT NULL,  -- nullable would mean unassigned-to-region; domain says assign before dispatch
    -- Maps-derived (Service 1). Default to the offline fallback (0 km / 60 min) until enriched.
    distanceKm                FLOAT NOT NULL DEFAULT 0,
    estimatedDurationMinutes  INT   NOT NULL DEFAULT 60,
    -- How many offers were sent for this trip (attractiveness metric); 0 until first offer.
    offerCounter              INT   NOT NULL DEFAULT 0,
    FOREIGN KEY (region_id) REFERENCES Region (region_id)
        ON DELETE NO ACTION ON UPDATE NO ACTION
);

-- ============================================================
-- Offer
-- Mediator class: one outreach attempt, one driver, one trip.
-- Multiple Offer rows may exist per trip (forwarding chain).
-- FKs to Trip and ExternalDriver; loads last.
-- ============================================================
CREATE TABLE Offer (
    offer_id        INT           NOT NULL PRIMARY KEY,
    trip_id         INT           NOT NULL,
    driver_id       INT           NOT NULL,
    sentAt          DATETIME2     NOT NULL,
    expiresAt       DATETIME2     NOT NULL,
    status          NVARCHAR(20)  NOT NULL
        CHECK (status IN ('pending', 'pending_approval', 'accepted', 'rejected', 'timeout', 'approval_timeout', 'cancelled')),
    driverReplyText NVARCHAR(MAX) NULL,   -- null until driver responds; free text from WhatsApp reply
    aiInterpretation NVARCHAR(MAX) NULL,  -- null until AI processes the driver reply
    rankPosition    INT           NOT NULL,
    rankReason      NVARCHAR(200) NULL,   -- one-line AI justification for the rank (null until ranked)
    FOREIGN KEY (trip_id)   REFERENCES Trip           (trip_id)   ON DELETE NO ACTION ON UPDATE NO ACTION,
    FOREIGN KEY (driver_id) REFERENCES ExternalDriver (driver_id) ON DELETE NO ACTION ON UPDATE NO ACTION
);

-- ============================================================
-- Message  (WhatsApp conversation audit trail)
-- One row per outbound offer text / inbound (simulated) driver reply.
-- FKs to ExternalDriver and Offer; loads LAST.
-- ============================================================
CREATE TABLE Message (
    message_id       INT           NOT NULL PRIMARY KEY,
    driver_id        INT           NOT NULL,
    direction        NVARCHAR(20)  NOT NULL
        CHECK (direction IN ('inbound', 'outbound')),
    waMessageId      NVARCHAR(100) NULL,            -- WhatsApp id, or a local id from the fallback channel
    body             NVARCHAR(MAX) NOT NULL,
    timestamp        DATETIME2     NOT NULL,
    related_offer_id INT           NULL,            -- the offer this message concerns (if any)
    FOREIGN KEY (driver_id)        REFERENCES ExternalDriver (driver_id) ON DELETE NO ACTION ON UPDATE NO ACTION,
    FOREIGN KEY (related_offer_id) REFERENCES Offer          (offer_id)  ON DELETE NO ACTION ON UPDATE NO ACTION
);
