-- External Driver Dispatch System — Seed Test Data
-- Group 17, SAD course, Ben-Gurion University
-- Load order: Region -> ExternalDriver -> ExternalDriverRegion -> Trip -> Offer
-- English (ASCII) values, plausible Israeli data, all enum values covered at least once.

USE ExternalDriverDispatch;
GO

-- ============================================================
-- Region  (6 rows — Israel's main tourist transport areas)
-- ============================================================
EXEC sp_Region_create @region_id=1, @name='Tel Aviv & Center',  @country='Israel', @city='Tel Aviv',   @createdAt='2025-01-10';
EXEC sp_Region_create @region_id=2, @name='Jerusalem',          @country='Israel', @city='Jerusalem',  @createdAt='2025-01-10';
EXEC sp_Region_create @region_id=3, @name='Haifa & North',      @country='Israel', @city='Haifa',      @createdAt='2025-01-10';
EXEC sp_Region_create @region_id=4, @name='Eilat & South',      @country='Israel', @city='Eilat',      @createdAt='2025-01-10';
EXEC sp_Region_create @region_id=5, @name='Netanya & Sharon',   @country='Israel', @city='Netanya',    @createdAt='2025-01-10';
EXEC sp_Region_create @region_id=6, @name='Beer Sheva & Negev', @country='Israel', @city='Beer Sheva', @createdAt='2025-01-10';
GO

-- ============================================================
-- ExternalDriver  (10 rows — all vehicle types covered, mix of flags)
-- ============================================================
EXEC sp_ExternalDriver_create @driver_id=1,  @drivercode='DRV-001', @name='Yossi Cohen',     @phone='050-1234567', @homeCity='Tel Aviv',   @vehicleType='sedan',             @worksShabbat=0, @worksNights=1, @worksLongDistance=0, @active=1;
EXEC sp_ExternalDriver_create @driver_id=2,  @drivercode='DRV-002', @name='David Levi',      @phone='052-2345678', @homeCity='Ramat Gan',  @vehicleType='minivan',           @worksShabbat=0, @worksNights=1, @worksLongDistance=1, @active=1;
EXEC sp_ExternalDriver_create @driver_id=3,  @drivercode='DRV-003', @name='Rachel Israeli',  @phone='054-3456789', @homeCity='Jerusalem',  @vehicleType='sedan',             @worksShabbat=1, @worksNights=0, @worksLongDistance=0, @active=1;
EXEC sp_ExternalDriver_create @driver_id=4,  @drivercode='DRV-004', @name='Moshe Avraham',   @phone='058-4567890', @homeCity='Haifa',      @vehicleType='executive_minivan', @worksShabbat=0, @worksNights=1, @worksLongDistance=1, @active=1;
EXEC sp_ExternalDriver_create @driver_id=5,  @drivercode='DRV-005', @name='Sara Biton',      @phone='050-5678901', @homeCity='Netanya',    @vehicleType='minibus_15',        @worksShabbat=1, @worksNights=1, @worksLongDistance=1, @active=1;
EXEC sp_ExternalDriver_create @driver_id=6,  @drivercode='DRV-006', @name='Amir Solomon',    @phone='052-6789012', @homeCity='Ashdod',     @vehicleType='minibus_18',        @worksShabbat=0, @worksNights=0, @worksLongDistance=1, @active=1;
EXEC sp_ExternalDriver_create @driver_id=7,  @drivercode='DRV-007', @name='Miriam Hadad',    @phone='054-7890123', @homeCity='Beer Sheva', @vehicleType='sedan',             @worksShabbat=1, @worksNights=0, @worksLongDistance=0, @active=1;
EXEC sp_ExternalDriver_create @driver_id=8,  @drivercode='DRV-008', @name='Nir Peretz',      @phone='058-8901234', @homeCity='Eilat',      @vehicleType='minivan',           @worksShabbat=1, @worksNights=1, @worksLongDistance=1, @active=1;
EXEC sp_ExternalDriver_create @driver_id=9,  @drivercode='DRV-009', @name='Tamar Golan',     @phone='050-9012345', @homeCity='Tel Aviv',   @vehicleType='executive_minivan', @worksShabbat=0, @worksNights=1, @worksLongDistance=0, @active=1;
EXEC sp_ExternalDriver_create @driver_id=10, @drivercode='DRV-010', @name='Eliyahu Mizrahi', @phone='052-0123456', @homeCity='Jerusalem',  @vehicleType='minibus_15',        @worksShabbat=0, @worksNights=0, @worksLongDistance=1, @active=0; -- inactive driver
GO

-- ============================================================
-- ExternalDriverRegion  (14 rows — drivers may cover multiple regions)
-- ============================================================
EXEC sp_ExternalDriverRegion_create @driver_id=1,  @region_id=1;  -- Yossi -> Tel Aviv
EXEC sp_ExternalDriverRegion_create @driver_id=1,  @region_id=5;  -- Yossi -> Netanya
EXEC sp_ExternalDriverRegion_create @driver_id=2,  @region_id=1;  -- David -> Tel Aviv
EXEC sp_ExternalDriverRegion_create @driver_id=2,  @region_id=5;  -- David -> Netanya
EXEC sp_ExternalDriverRegion_create @driver_id=3,  @region_id=2;  -- Rachel -> Jerusalem
EXEC sp_ExternalDriverRegion_create @driver_id=4,  @region_id=3;  -- Moshe -> Haifa
EXEC sp_ExternalDriverRegion_create @driver_id=4,  @region_id=5;  -- Moshe -> Netanya
EXEC sp_ExternalDriverRegion_create @driver_id=5,  @region_id=5;  -- Sara -> Netanya
EXEC sp_ExternalDriverRegion_create @driver_id=6,  @region_id=1;  -- Amir -> Tel Aviv
EXEC sp_ExternalDriverRegion_create @driver_id=6,  @region_id=6;  -- Amir -> Beer Sheva
EXEC sp_ExternalDriverRegion_create @driver_id=7,  @region_id=6;  -- Miriam -> Beer Sheva
EXEC sp_ExternalDriverRegion_create @driver_id=8,  @region_id=4;  -- Nir -> Eilat
EXEC sp_ExternalDriverRegion_create @driver_id=9,  @region_id=1;  -- Tamar -> Tel Aviv
EXEC sp_ExternalDriverRegion_create @driver_id=10, @region_id=2;  -- Eliyahu -> Jerusalem (inactive)
GO

-- ============================================================
-- Trip  (10 rows — all status values covered)
-- ============================================================
EXEC sp_Trip_create @trip_id=1,  @externalBookingId='RC-2026-0001', @pickupAddress='Ben Gurion Airport',       @dropoffAddress='Dan Hotel Tel Aviv',      @pickupCity='Lod',        @dropoffCity='Tel Aviv',     @pickupTime='2026-06-20 06:30:00', @numPassengers=2,  @vehicleType='sedan',             @priceToDriver=180.00, @status='open',       @createdAt='2026-06-14', @region_id=1;
EXEC sp_Trip_create @trip_id=2,  @externalBookingId='RC-2026-0002', @pickupAddress='Ben Gurion Airport',       @dropoffAddress='Mamilla Hotel Jerusalem', @pickupCity='Lod',        @dropoffCity='Jerusalem',    @pickupTime='2026-06-20 08:00:00', @numPassengers=4,  @vehicleType='minivan',           @priceToDriver=250.00, @status='open',       @createdAt='2026-06-14', @region_id=2;
EXEC sp_Trip_create @trip_id=3,  @externalBookingId='RC-2026-0003', @pickupAddress='Hilton Tel Aviv',          @dropoffAddress='Ashdod Port',             @pickupCity='Tel Aviv',   @dropoffCity='Ashdod',       @pickupTime='2026-06-20 10:00:00', @numPassengers=1,  @vehicleType='sedan',             @priceToDriver=140.00, @status='offered',    @createdAt='2026-06-14', @region_id=1;
EXEC sp_Trip_create @trip_id=4,  @externalBookingId='RC-2026-0004', @pickupAddress='Tel Aviv Central Station', @dropoffAddress='Ben Gurion Airport',      @pickupCity='Tel Aviv',   @dropoffCity='Lod',          @pickupTime='2026-06-20 14:00:00', @numPassengers=3,  @vehicleType='minivan',           @priceToDriver=175.00, @status='confirmed',  @createdAt='2026-06-14', @region_id=1;
EXEC sp_Trip_create @trip_id=5,  @externalBookingId='RC-2026-0005', @pickupAddress='King David Hotel',         @dropoffAddress='Yad Vashem',              @pickupCity='Jerusalem',  @dropoffCity='Jerusalem',    @pickupTime='2026-06-20 09:30:00', @numPassengers=6,  @vehicleType='executive_minivan', @priceToDriver=220.00, @status='open',       @createdAt='2026-06-14', @region_id=2;
EXEC sp_Trip_create @trip_id=6,  @externalBookingId='RC-2026-0006', @pickupAddress='Leonardo Hotel Haifa',     @dropoffAddress='Ben Gurion Airport',      @pickupCity='Haifa',      @dropoffCity='Lod',          @pickupTime='2026-06-21 05:00:00', @numPassengers=2,  @vehicleType='sedan',             @priceToDriver=310.00, @status='open',       @createdAt='2026-06-14', @region_id=3;
EXEC sp_Trip_create @trip_id=7,  @externalBookingId='RC-2026-0007', @pickupAddress='U Hotel Eilat',            @dropoffAddress='Ramon Airport',           @pickupCity='Eilat',      @dropoffCity='Mitzpe Ramon', @pickupTime='2026-06-21 07:00:00', @numPassengers=12, @vehicleType='minibus_15',        @priceToDriver=450.00, @status='unassigned', @createdAt='2026-06-13', @region_id=4;
EXEC sp_Trip_create @trip_id=8,  @externalBookingId='RC-2026-0008', @pickupAddress='Ben Gurion Airport',       @dropoffAddress='Crowne Plaza Netanya',    @pickupCity='Lod',        @dropoffCity='Netanya',      @pickupTime='2026-06-21 11:00:00', @numPassengers=5,  @vehicleType='executive_minivan', @priceToDriver=210.00, @status='open',       @createdAt='2026-06-14', @region_id=5;
EXEC sp_Trip_create @trip_id=9,  @externalBookingId='RC-2026-0009', @pickupAddress='Ben Gurion University',    @dropoffAddress='Ben Gurion Airport',      @pickupCity='Beer Sheva', @dropoffCity='Lod',          @pickupTime='2026-06-21 13:30:00', @numPassengers=18, @vehicleType='minibus_18',        @priceToDriver=550.00, @status='open',       @createdAt='2026-06-14', @region_id=6;
EXEC sp_Trip_create @trip_id=10, @externalBookingId='RC-2026-0010', @pickupAddress='Hilton Jerusalem',         @dropoffAddress='Ben Gurion Airport',      @pickupCity='Jerusalem',  @dropoffCity='Lod',          @pickupTime='2026-06-19 16:00:00', @numPassengers=2,  @vehicleType='sedan',             @priceToDriver=240.00, @status='cancelled',  @createdAt='2026-06-12', @region_id=2;
GO

-- ============================================================
-- Offer  (7 rows — all status values covered; trips 3,4,7,10 have history)
-- ============================================================
-- Trip 3 (offered): one pending_approval offer
EXEC sp_Offer_create @offer_id=1, @trip_id=3,  @driver_id=1, @sentAt='2026-06-14 08:00:00', @expiresAt='2026-06-14 09:00:00', @status='pending_approval', @driverReplyText=NULL,             @aiInterpretation=NULL,        @rankPosition=1;
-- Trip 4 (confirmed): driver 2 accepted after driver 1 rejected
EXEC sp_Offer_create @offer_id=2, @trip_id=4,  @driver_id=1, @sentAt='2026-06-14 07:00:00', @expiresAt='2026-06-14 08:00:00', @status='rejected',         @driverReplyText='Can''t today',    @aiInterpretation='rejection',  @rankPosition=1;
EXEC sp_Offer_create @offer_id=3, @trip_id=4,  @driver_id=2, @sentAt='2026-06-14 08:05:00', @expiresAt='2026-06-14 09:05:00', @status='accepted',         @driverReplyText='OK, I''ll take it',@aiInterpretation='acceptance', @rankPosition=2;
-- Trip 7 (unassigned): driver timed out
EXEC sp_Offer_create @offer_id=4, @trip_id=7,  @driver_id=8, @sentAt='2026-06-13 10:00:00', @expiresAt='2026-06-13 11:00:00', @status='timeout',          @driverReplyText=NULL,             @aiInterpretation=NULL,        @rankPosition=1;
-- Trip 10 (cancelled): one offer that was later cancelled
EXEC sp_Offer_create @offer_id=5, @trip_id=10, @driver_id=3, @sentAt='2026-06-12 12:00:00', @expiresAt='2026-06-12 13:00:00', @status='cancelled',        @driverReplyText=NULL,             @aiInterpretation=NULL,        @rankPosition=1;
-- Additional offers covering pending and approval_timeout statuses
EXEC sp_Offer_create @offer_id=6, @trip_id=2,  @driver_id=3, @sentAt='2026-06-14 09:00:00', @expiresAt='2026-06-14 10:00:00', @status='pending',          @driverReplyText=NULL,             @aiInterpretation=NULL,        @rankPosition=1;
EXEC sp_Offer_create @offer_id=7, @trip_id=5,  @driver_id=10,@sentAt='2026-06-14 07:30:00', @expiresAt='2026-06-14 09:00:00', @status='approval_timeout', @driverReplyText=NULL,             @aiInterpretation=NULL,        @rankPosition=1;
GO

-- ============================================================
-- Message  (WhatsApp conversation audit trail)
-- One outbound offer text + one inbound reply for each offer the driver
-- actually answered (offers 2 and 3). The inbound timestamps are what the
-- Driver Performance report uses to compute average response time.
-- ============================================================
-- Offer 2 (trip 4, driver 1): rejected ~5 min after the offer was sent
EXEC sp_Message_create @message_id=1, @driver_id=1, @direction='outbound', @waMessageId='SEED-OUT-01', @body='New trip offer: BGU Airport -> Lod, 14/06 14:00, 3 pax, 175 NIS. Reply YES to confirm.', @timestamp='2026-06-14 07:00:00', @related_offer_id=2;
EXEC sp_Message_create @message_id=2, @driver_id=1, @direction='inbound',  @waMessageId='SEED-IN-01',  @body='Can''t today',        @timestamp='2026-06-14 07:05:00', @related_offer_id=2;
-- Offer 3 (trip 4, driver 2): accepted ~8 min after the offer was sent
EXEC sp_Message_create @message_id=3, @driver_id=2, @direction='outbound', @waMessageId='SEED-OUT-02', @body='New trip offer: BGU Airport -> Lod, 14/06 14:00, 3 pax, 175 NIS. Reply YES to confirm.', @timestamp='2026-06-14 08:05:00', @related_offer_id=3;
EXEC sp_Message_create @message_id=4, @driver_id=2, @direction='inbound',  @waMessageId='SEED-IN-02',  @body='OK, I''ll take it',   @timestamp='2026-06-14 08:13:00', @related_offer_id=3;
GO
