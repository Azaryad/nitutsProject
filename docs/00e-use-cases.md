# Use Case Specifications

Six use cases are in scope for the External Supplier Dispatch System. Each specification has two layers:

1. **Behavioral spec** — analysis level, technology-neutral (no class, method, or stored-procedure names).
2. **Implementation Notes** — design level, mapping behavioral steps to classes, methods, and stored procedures.

The Implementation Notes layer is **TODO** for every use case: none of the source documents contain design-level mappings yet.

## Relationships

- **Send Trip Offer** `«include»` **Assign Trip to Region** — a trip must have a region before offers can be ranked and sent.
- **Respond to Trip Offer** `«include»` **Update Ride Control** — step 8 triggers the Ride Control sync for each accepted trip.
- **Forward Offer to Next Driver** `«extend»` **Respond to Trip Offer** — triggered at extension 6a when the driver rejects all trips or fails to respond in time.

## Source coverage

| UC | Behavioral spec source | Confidence |
|---|---|---|
| View Open Trips | Brief only — `PartB_Group17.pdf` (Use Case 1) | Brief; detailed flow TODO |
| Assign Trip to Region | `PartB_Group17_UCs.pdf` (full MSS) | Authoritative |
| Send Trip Offer (UC17) | `SEND TRIP UC DETAILS … .pdf` + VP model (identical) | Authoritative |
| Respond to Trip Offer (UC07) | `RESPOND TO TRIP UC DETAILS … .pdf` + VP model (identical) | Authoritative |
| Forward Offer to Next Driver | `PartB_Group17_UCs.pdf` (full MSS) | Authoritative |
| Update Ride Control (UC08) | `UPDATE RIDE UC DETAILS … .pdf` + VP model (identical) | Authoritative |

> All five flow use cases now come from a readable source. `Part_B_5UC_final.pdf` itself is image-only, but the team-supplied `PartB_Group17_UCs.pdf` is text-extractable and carries the full MSS for UC17, Assign Trip to Region, UC07, Forward Offer to Next Driver, and UC08 (the three detail PDFs match UC17/UC07/UC08 verbatim). **View Open Trips** remains brief-level — its only source is the Part B brief (`PartB_Group17.pdf`).

---

## View Open Trips

- **ID:** UC02 (Part B Use Case 1)
- **Primary actor:** Dispatcher
- **Requirements:** F08 (Display Open Trips)
- **Preconditions:** Dispatcher is logged in to the dispatch dashboard.
- **Postconditions:** Dispatcher sees the current list of open trips and can identify which ones require action and begin the assignment process.

### Behavioral spec

The system displays a list of open trips that have not yet been assigned. For each trip it shows the trip details, pickup time, destination, activity region, and current status. The dispatcher reviews the list and selects a trip to begin the assignment process. This is the entry point of the driver-assignment flow; from here the dispatcher proceeds to *Assign Trip to Region*.

*Extensions: TODO — not specified in source (brief only).*

### Implementation Notes

*TODO — design-level mapping not present in source.*

---

## Assign Trip to Region

- **ID:** UC05
- **Primary actor:** Dispatcher
- **Requirements:** F09 (Load Active Drivers by Area), F10 (Filter Drivers by Qualification), F11 (AI Driver Ranking)
- **Preconditions:** Trip exists with status "Open"; at least one active Region is configured; dispatcher logged in.
- **Postconditions:** (1) Trip is associated with the selected region. (2) Trip is visible in the region's dispatch queue. (3) Trip remains with status "Open," ready for driver assignment.

### Behavioral spec

*The dispatcher assigns an open trip to a geographic region, making it visible in the dispatch queue for that region and enabling driver matching to begin.*

**MSS**

1. Dispatcher views the open-trips panel in the dashboard.
2. Dispatcher drags a trip onto a region or selects a region from the dropdown.
3. System validates that the selected region exists and is active.
4. System updates the Trip's region field.
5. System displays the trip under the selected region in the dispatch queue.
6. Use case ends.

**Extensions**

- 2a. A supplier is selected instead of a region → routes to the supplier dispatch path.
- 3a. Region is inactive → error message; the trip stays unassigned.

### Implementation Notes

*TODO — design-level mapping not present in source.*

---

## Send Trip Offer

- **ID:** UC17
- **Primary actor:** Dispatcher
- **Requirements:** F12 (Automatic WhatsApp Message), F13 (Secure Link Creation)
- **Preconditions:** (1) At least one trip exists with status "Open." (2) Dispatcher is logged in to the dispatch dashboard. (3) At least one active driver is registered for the relevant region.
- **Postconditions:** (1) A WhatsApp offer has been sent to the highest-ranked eligible driver. (2) Trip status changed to "Pending Approval." (3) System is actively monitoring for driver response.

### Behavioral spec

**MSS**

1. Dispatcher selects one or more open trips from the dashboard.
2. Dispatcher assigns the trips to a driver region or group.
3. System loads all active drivers registered for that region.
4. System filters drivers by vehicle type and passenger capacity.
5. System ranks the filtered drivers by proximity, current workload, and availability.
6. System packages the selected trips for the top-ranked driver, verifying there are no time conflicts between trips.
7. System sends a WhatsApp message to the top-ranked driver containing full trip details and a unique confirmation link.
8. Trip status is updated to "Pending Approval."
9. System begins monitoring for driver response.

**Extensions**

- 4a. No eligible drivers found → System displays "No available driver" alert; Dispatcher may reassign to a supplier instead.
- 7a. WhatsApp delivery fails → System retries; if retry fails, Dispatcher receives an alert to handle manually.
- 9a. Driver does not respond within the allowed time → System automatically forwards the offer to the next driver in the ranked list.

**Notes**

1. Dispatcher may cancel the assignment at any point before the driver responds; trip returns to "Open."
2. If all ranked drivers are exhausted without acceptance, the trip is flagged "Unassigned" and Dispatcher is notified.

### Implementation Notes

*TODO — design-level mapping not present in source.*

---

## Respond to Trip Offer

- **ID:** UC07
- **Primary actor:** External Driver - Supplier
- **Requirements:** F14 (Mobile Trip Approval Page), F15 (Per-Trip Approve / Reject), F17 (Send Confirmation to Driver), F20 (AI Interpretation of WhatsApp Replies), F21 (Update Driver Availability from WhatsApp)
- **Preconditions:** (1) Driver received a WhatsApp message containing a confirmation link. (2) The confirmation link is still valid. (3) The offered trip(s) are still in "Pending Approval" status.
- **Postconditions:** (1) Accepted trips are updated to status "Assigned." (2) Ride Control System is updated for each accepted trip. (3) Driver received a WhatsApp confirmation for accepted trips. (4) Rejected trips are forwarded to the next available driver in the ranked list.

### Behavioral spec

**MSS**

1. Driver receives a WhatsApp message with trip details and a confirmation link.
2. Driver opens the confirmation link.
3. System validates the link.
4. System verifies the offered trips are still pending and have not been assigned to another driver.
5. Confirmation page loads, displaying each offered trip with: time, pickup address, destination, and number of passengers.
6. Driver reviews each trip individually and selects "Accept" or "Cannot" for each one.
7. System saves the driver's selections.
8. For each accepted trip: status is updated to "Assigned" and Ride Control is updated (triggers *Update Ride Control*).
9. Driver receives a WhatsApp confirmation: "Confirmed! Thank you."
10. For each rejected trip: offer is automatically forwarded to the next driver in the ranked list.

**Extensions**

- 3a. Link is invalid → Page displays "Invalid link" message; session ends.
- 3b. Link has expired → Page displays "This link is no longer active" message; session ends.
- 4a. Trip already assigned to another driver → Page displays "This offer is no longer available."
- 6a. Driver rejects all trips → All trips forwarded to next driver; Dispatcher notified if no drivers remain in the list. `«extend»` *Forward Offer to Next Driver* — triggered when driver selects "Cannot" for a trip or fails to respond within the allowed time.

**Notes**

1. Driver may accept some trips and reject others in the same session; each decision is handled independently. A supplier receiving a batch may accept on behalf of their drivers and internally redistribute the trips.

### Implementation Notes

*TODO — design-level mapping not present in source.*

---

## Forward Offer to Next Driver

- **ID:** UC19
- **Primary actor:** External Driver - Supplier
- **Requirements:** F18 (Driver Timeout Management), F19 (Link Timeout Management), F24 (Unassigned Trip Alert)
- **Preconditions:** Offer rejected or timed out; trip still "Open" or "Pending Approval"; a ranked driver list is available for the trip's region.
- **Postconditions:** (1) Trip is re-offered to the next eligible driver, OR (2) Trip is escalated to the manager if no eligible drivers remain.

### Behavioral spec

*When a driver rejects an offer or an offer times out, the system automatically re-queues the trip and sends a new offer to the next eligible driver in the ranked list. If no drivers remain, the trip is escalated to the manager.*

**MSS**

1. Driver rejects the trip offer (via WhatsApp reply or Decline button on the confirmation page).
2. System sets the current Offer status to "Rejected" and the Trip status back to "Open."
3. System queries the remaining ranked drivers in the region, excluding drivers who already received an offer for this trip.
4. System selects the next highest-ranked eligible driver.
5. System creates a new Offer and generates a unique confirmation URL for driver approval.
6. System sends a new WhatsApp offer message to the next driver.
7. Use case ends.

**Extensions**

- 3a. No eligible drivers remain → Trip status set to "Unassigned"; an escalation WhatsApp is sent to the manager.

### Implementation Notes

*TODO — design-level mapping not present in source.*

---

## Update Ride Control

- **ID:** UC08
- **Primary actor:** Ride Control «System»
- **Requirements:** F16 (Appoint Driver in Ride Control)
- **Preconditions:** (1) A driver has accepted one or more trips. (2) Trip status in the dispatch system is "Assigned." (3) Ride Control System is available and reachable.
- **Postconditions:** (1) Ride Control System reflects the assigned driver for each trip. (2) Trip records in Ride Control are updated with driver name, phone number, and vehicle details. (3) Both systems are fully synchronized on the trip assignment.

### Behavioral spec

**MSS**

1. Driver confirms acceptance of a trip (via *Respond to Trip Offer*).
2. System automatically initiates an update request to Ride Control — no manual action required.
3. System transmits driver assignment details to Ride Control System: driver name, phone number, and vehicle information.
4. Ride Control System processes the update and confirms receipt.
5. Trip record in both systems reflects status "Assigned" with full driver details.
6. System logs the successful synchronization with a timestamp.

**Extensions**

- 3a. Ride Control System is temporarily unavailable → System retries the update; if retries are exhausted, Dispatcher receives an alert to verify manually.
- 4a. Ride Control returns an error response → System logs the error and alerts Dispatcher to manually confirm the assignment inside Ride Control.
- 4b. Duplicate assignment detected in Ride Control → System blocks the overwrite and alerts Dispatcher to resolve the conflict.

**Notes**

1. This UC is triggered automatically following every successful driver acceptance and requires no input from the Dispatcher. If synchronization fails, the trip remains "Assigned" in the dispatch system but must be manually verified in Ride Control.

### Implementation Notes

*TODO — design-level mapping not present in source.*
