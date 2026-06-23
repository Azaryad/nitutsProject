# Class Diagram

![class diagram](class-diagram.png)

> Source of truth: the diagram itself lives in the Visual Paradigm model (`docs/VPPARTA.vpp PartB_Group17.vpp`) and the `Class Diagram1` page exported in `project.xml`. This file is the textual companion. The `.png` above is exported from the modeling tool.

## Design assumptions

The system does not manage all of the organization's transport activity; it focuses on the process of forwarding trips to drivers and external suppliers. Trips arrive from the external **Ride Control** system, and the developed system is responsible for managing the information and processes related to assigning those trips — sending offers, receiving driver responses, and updating the assignment result back to Ride Control.

## Entities

### Trip
A transport job received from Ride Control. The system manages the assignment of this trip to an external driver; it does not manage trip execution.

**Attributes**

| Name | Type | Visibility |
|---|---|---|
| id | int | private |
| externalBookingId | String | private |
| pickupAddress | String | private |
| dropoffAddress | String | private |
| pickupCity | String | private |
| dropoffCity | String | private |
| pickupTime | DateTime | private |
| numPassengers | int | private |
| vehicleType | VehicleType (enum) | private |
| priceToDriver | float | private |
| status | TripStatus (enum) | private |
| createdAt | DateTime | private |
| distanceKm | double | private |
| estimatedDurationMinutes | int | private |

> `distanceKm` and `estimatedDurationMinutes` are populated by the Maps service (`«service» MapsService`) when a trip is enriched. They default to the offline fallback (0 km / 60 min) until enriched. `distanceKm` drives the long-distance driver filter and feeds the AI ranking prompt.

**Operations**

| Signature | Returns |
|---|---|
| offer() | void |
| confirm() | void |
| requeue() | void |

### ExternalDriver
A freelance or external driver who receives trip offers via WhatsApp.

**Attributes**

| Name | Type | Visibility |
|---|---|---|
| id | int | private |
| drivercode | String | private |
| name | String | private |
| phone | String | private |
| homeCity | String | private |
| vehicleType | VehicleType (enum) | private |
| worksShabbat | boolean | private |
| worksNights | boolean | private |
| worksLongDistance | boolean | private |
| active | boolean | private |

**Operations**

| Signature | Returns |
|---|---|
| isEligibleForTrip(trip: Trip) | boolean |
| getMaxPassengers() | int |
| updateRegion(region: String) | void |

### Offer
Mediator class representing one outreach attempt: one driver contacted for one trip. Multiple Offer records may exist per Trip — one per driver approached during the forwarding chain.

**Attributes**

| Name | Type | Visibility |
|---|---|---|
| id | int | private |
| sentAt | DateTime | private |
| expiresAt | DateTime | private |
| status | OfferStatus (enum) | private |
| driverReplyText | String | private |
| aiInterpretation | String | private |
| rankPosition | int | private |
| rankReason | String | private |

> `rankReason` holds the one-line justification produced by the AI ranking role (`«service» AiAgentService`); `aiInterpretation` holds the yes/no/ambiguous classification of `driverReplyText` produced by the AI interpret role — which is what drives the Offer state machine (`pending → pending_approval` on yes, `→ rejected` on no).

**Operations**

| Signature | Returns |
|---|---|
| accept() | void |
| reject() | void |
| generateApprovalUrl() | String |

### Region
Geographic operating zone used to filter drivers and queue trips for dispatch.

**Attributes**

| Name | Type | Visibility |
|---|---|---|
| id | int | private |
| name | String | private |
| country | String | private |
| city | String | private |
| createdAt | DateTime | private |

**Operations**

| Signature | Returns |
|---|---|
| getActiveDrivers() | List |
| getOpenTrips() | List |
| getEligibleDrivers(trip: Trip) | List |

### RideControlSystem «Interface»
Represents the external Ride Control system. Modeled as an interface because it is an external system — this project implements only the communication contract, not the system itself.

**Operations**

| Signature | Returns |
|---|---|
| importTrips() | (unspecified) |
| updateTripAssignment() | (unspecified) |

### Message
One message in the WhatsApp conversation audit trail: an outbound offer text or an inbound (simulated) driver reply. Created by the dispatch flow whenever the `«service» MessageChannel` sends a message or a reply arrives. Links a driver to the offer the message concerns.

**Attributes**

| Name | Type | Visibility |
|---|---|---|
| id | int | private |
| direction | MessageDirection (enum) | private |
| waMessageId | String | private |
| body | String | private |
| timestamp | DateTime | private |

> `driver` (→ ExternalDriver) and `offer` (→ Offer, optional) are held as object references, not ids.

## External Service Classes «service»

External services are modeled as `«service»` classes, **not** business entities: they hold no persistent state, talk to the outside world behind a C# interface, and each has a deterministic offline fallback so the system runs end-to-end with all three disabled. They are excluded from the entity count. (This mirrors how `RideControlSystem` is modeled as an external `«Interface»`.)

| Class | Interface(s) | Operations | Fills |
|---|---|---|---|
| MapsService | IDriveInfoProvider | getDriveInfo(origin, destination, time) : DriveInfo | Trip.distanceKm, Trip.estimatedDurationMinutes |
| AiAgentService | IDriverRanker, IMessageComposer, IReplyInterpreter, IRestrictionParser | rank(...), composeOffer(...), interpretReply(...), parseAvailability(...) | Offer.rankPosition, Offer.rankReason, Offer.aiInterpretation, Offer.status; ExternalDriver availability flags |
| MessageChannel | IMessageChannel | sendText(phone, body) : String, sendDocument(...) : String | (transports text; the flow records each Message) |

## Enumerations

| Enum | Literals |
|---|---|
| VehicleType | sedan, executive_minivan, minivan, minibus_15, minibus_18 |
| TripStatus | open, offered, confirmed, completed, cancelled, unassigned |
| OfferStatus | pending, pending_approval, accepted, rejected, timeout, approval_timeout, cancelled |
| MessageDirection | inbound, outbound |

## Relationships

| From | Mult. | To | Mult. | Type | Meaning |
|---|---|---|---|---|---|
| Trip | 1 | Offer | 0..* | Association | A trip may have many outreach attempts; each Offer is for exactly one trip. |
| ExternalDriver | 1..* | Offer | 0..* | Association | An Offer is directed at the driver(s); a driver may receive many offers. |
| ExternalDriver | * | Region | 1..* | Association | A driver operates in one or more regions; a region has many drivers. |
| Trip | * | Region | 1 | Association | Each trip is associated with exactly one region; a region queues many trips. |
| Trip | — | VehicleType | — | Dependency | Trip uses the VehicleType enum. |
| Trip | — | TripStatus | — | Dependency | Trip uses the TripStatus enum. |
| Trip | — | RideControlSystem | — | Dependency | Trip assignment is pushed back through the Ride Control interface. |
| ExternalDriver | — | VehicleType | — | Dependency | Driver uses the VehicleType enum. |
| Offer | — | OfferStatus | — | Dependency | Offer uses the OfferStatus enum. |
| ExternalDriver | 1 | Message | 0..* | Association | A message is to/from one driver; a driver has many messages. |
| Offer | 1 | Message | 0..* | Association | A message may concern one offer; an offer accumulates its conversation. |
| Message | — | MessageDirection | — | Dependency | Message uses the MessageDirection enum. |
| Trip | — | MapsService | — | Dependency | Trip distance/duration are filled by the Maps service. |
| Offer | — | AiAgentService | — | Dependency | Offer rank/interpretation are filled by the AI service. |
| Message | — | MessageChannel | — | Dependency | Messages are sent through the WhatsApp channel. |

## Modeling-choice rationale

1. **`Offer` is a mediator class, not an association class.** It is implemented as a separate mediator class because the system must retain the history of outreach to drivers across the assignment process. Several offers may be sent to different drivers for a single trip until an accepting driver is found. Without retaining this information the system could not know which drivers were already approached, risking duplicate offers to the same driver and undermining the orderly management of the assignment process.

2. **`RideControlSystem` is an «Interface».** It is represented as an interface because it is an external system that our system only communicates with: it receives trips from Ride Control and updates Ride Control after a driver is assigned. The interface therefore represents the communication point with an external system, with no need to implement the external system itself inside the class diagram.

3. **`TripStatus`, `OfferStatus`, `VehicleType`, `MessageDirection` are «Enumeration»s.** They are defined as enumerations because they are fixed, predefined sets of values. Using enumerations keeps the data uniform and prevents invalid values from being entered.

4. **The three external services are `«service»` classes, not business entities.** Maps, the AI agent, and WhatsApp each sit behind a C# interface and hold no persistent state, so they are modeled as stereotyped service classes and excluded from the entity count. Each has a deterministic offline fallback (static distance, proximity-sort ranking + template text + keyword interpretation, logging channel), so the full dispatch flow runs with all three disabled — a missing API key downgrades a feature, it never crashes the system. The business classes gain *data* from the services (`Trip.distanceKm`, `Offer.rankReason`, `Offer.aiInterpretation`, the `Message` audit trail) but do not *depend* on the concrete API classes — they depend on the interfaces.

---

*Rationale and assumptions above are translated from the team's Hebrew Part B document (`docs/PartB_Group17.pdf`). Attribute names, types, operation signatures, enum literals, and multiplicities are transcribed verbatim from the Visual Paradigm model.*
