# 7 User Requirements Specification

> Translated to English from the team's Hebrew Part A document. Functional requirement names follow the wording recorded in the Visual Paradigm model; user stories, priorities, and sources are translated from the Part A requirements table.

**About the system:** The system we want to build is a tool that supports Transfers TLV's existing "Ride Control" system. Our system is an independent system that manages a full data store on every driver — activity areas, vehicle type, availability, and personal constraints such as working hours or days. Ride Control recently exposed an interface for external suppliers, to whom many bookings are pushed so they can report back which driver performs each booking. We plug into that interface and, architecturally, act as the supplier of all the freelance drivers who work with the company. When a new trip arrives from the existing Ride Control system, an AI agent ranks the drivers by how well they fit the trip and starts contacting them one by one on WhatsApp until a driver confirms. After a driver confirms, the system updates Ride Control accordingly.

**Priority scale:** High = must be in the MVP; Medium = second phase; Low = nice-to-have.

## 7.1 Functional Requirements

32 functional requirements were elicited: 5 CRUD requirements, 2 complex reports, and 25 core functional requirements.

| ID | Description | User Story | Priority | Source |
|-----|-------------|-----------|----------|--------|
| F01 | Driver Management (CRUD) – create, view, update and delete drivers, including personal details, vehicle type, activity areas and availability. | As a dispatcher, I want to manage the driver list (create, view, update, delete), so that I always have an up-to-date roster. | High | Team |
| F02 | Activity Area Management (CRUD) – create, view, update and delete geographic areas. | As a dispatcher, I want to define activity areas, so that the system can filter drivers by area. | Medium | Team |
| F03 | System Settings Management (CRUD) – update global parameters: timeout durations, API keys, AI configuration. | As a system administrator, I want to update global parameters, so that the system behaves according to changing needs. | Medium | Team |
| F04 | Trip Management (View/Update) – view and update the details of a trip captured from Ride Control. | As a dispatcher, I want to view and edit an existing trip's details, so that I can correct errors before assignment. | High | Michel |
| F05 | Monthly Driver Report – a PDF document with all trips performed during the month, by week and day, including the payment amount per trip. | As a driver, I want to receive a clear monthly report on WhatsApp, so that I can verify the payment due to me. | Medium | Michel (AI) |
| F06 | Performance Report for Dispatcher – a summary report for a selected period: approval rates per driver, average response times, unassigned trips. | As a dispatcher, I want a periodic performance report, so that I can identify non-responsive drivers and improve processes. | Low | Team |
| F07 | Manual Trip Entry – the dispatcher adds a new trip (not from an automatic source) with all trip details. | As a dispatcher, I want to enter a trip manually, so that there is a uniform entry point for it. | High | Michel |
| F08 | Display Open Trips – a main screen showing all trips not yet assigned, with key details. | As a dispatcher, I want to see all open trips in real time, so that no trip slips past me. | High | Michel |
| F09 | Load Active Drivers by Area – the system shows available drivers by geographic proximity to the trip. | As a dispatcher, I want to load the active drivers in the relevant area, so that I see realistic options. | High | Team |
| F10 | Filter Drivers by Qualification – filter by vehicle type, passenger capacity and activity area. | As a dispatcher, I want to filter out drivers unsuited to the trip, so that irrelevant offers are not sent. | High | Team |
| F11 | AI Driver Ranking (Claude) – the agent ranks the filtered drivers by proximity, workload and availability. | As a dispatcher, I want drivers ranked by specific fit to the trip, so that the optimal driver is offered first. | High | Team |
| F12 | Automatic WhatsApp Message – a structured message with the trip details and a unique link per offer. | As a dispatcher, I want an automatic WhatsApp message sent, so that the driver gets complete, accurate information. | High | Michel (AI) |
| F13 | Secure Link Creation – a unique link per offer with a TTL and cryptographic verification. | As a dispatcher, I want every trip link to be signed and time-limited, so that approvals cannot be forged. | High | Team |
| F14 | Mobile Trip Approval Page – a Hebrew, mobile-friendly interface displaying all of the driver's offered trips. | As a driver, I want to open a convenient page on my phone showing all my trips, so that I can approve easily. | High | Michel (AI) |
| F15 | Per-Trip Approve / Reject – the driver can approve some trips and reject others individually. | As a driver, I want to approve each trip separately, so that I can accept some and decline others. | High | Michel (AI) |
| F16 | Appoint Driver in Ride Control – an API PUSH call after driver approval. | As a dispatcher, I want to update Ride Control with the approving driver's identity, so that both systems stay synchronized. | High | Team |
| F17 | Send Confirmation to Driver – a "Confirmed! Thank you!" WhatsApp message after a successful approval. | As a driver, I want an immediate confirmation that the trip is registered to me, so that I know I need not wait for further approval. | Medium | Michel (AI) |
| F18 | Driver Timeout Management – after one hour with no response, automatically move to the next driver on the priority list. | As a dispatcher, I want automatic timeout handling, so that a trip does not hang waiting indefinitely for a response. | High | Team |
| F19 | Link Timeout Management – after one hour in which a driver confirmed on WhatsApp but did not click the link, the trip is reported to the dispatcher on WhatsApp. | As the system, I want to suspend an offer that did not receive digital approval in time, so that the trip does not get stuck. | High | Michel |
| F20 | AI Interpretation of WhatsApp Replies – identify driver intent (approve / reject / availability change) from a free-text reply. | As the system, I want the AI to interpret free-text messages from drivers, so that not every reply requires manual parsing. | High | Michel (AI) |
| F21 | Update Driver Availability from WhatsApp – when a driver writes "off tomorrow" or similar, the system updates availability automatically. | As a dispatcher, I want to update driver availability from spontaneous messages, so that the roster stays current without manual work. | Medium | Michel (AI) |
| F22 | Real-time Trip Monitoring – a "monitor" screen showing every active trip with its current status. | As a dispatcher, I want a real-time status picture, so that I immediately know if a trip needs intervention. | High | Michel |
| F23 | Import Drivers from Excel – load a list of new drivers from an Excel file directly into the database. | As a dispatcher, I want to import drivers from Excel, so that I need not enter each driver manually. | Medium | Michel |
| F24 | Unassigned Trip Alert – when all drivers in an area are exhausted, the dispatcher receives an active alert. | As a dispatcher, I want an alert when no driver is available, so that I can act immediately. | High | Michel |
| F25 | Send Monthly Report via WhatsApp – the system sends each driver's PDF report via the WhatsApp API. | As a dispatcher, I want each driver's monthly report sent automatically, so that I need not send it manually. | Medium | Michel (AI) |

## 7.2 Non-Functional Requirements

| ID | Description | User Story | Priority |
|-----|-------------|-----------|----------|
| NF01 | Security – every link sent to a driver is signed with HMAC-SHA256 and has a TTL of 2.5 hours. An expired link is rejected. | As a security manager, I want approval links cryptographically protected, so that approvals cannot be forged. | High |
| NF02 | Performance – the approval page loads within 2 seconds on 3G, even for a trip with 5 offers. | As a driver, I want the page to open fast even on a slow connection, so that I do not wait. | High |
| NF03 | Availability – the system is available 24/7 with at least 99.5% uptime. Active monitoring. | As a dispatcher, I want the system working at any hour, so that trips entered before dawn are handled. | High |
| NF04 | RTL support – all interfaces are designed for right-to-left Hebrew, including buttons, texts and displays. | As a Hebrew-speaking driver, I want a perfect Hebrew RTL interface, so that I am not confused. | High |
| NF05 | Mobile compatibility – the approval page fits iOS/Android screens at 375px and up. Buttons at least 44px. | As a driver using a smartphone, I want a convenient interface with no zoom, so that I can approve easily. | High |
| NF06 | Architectural independence – the server does not depend on Ride Control availability. A failure in RC does not prevent sending messages. | As a dispatcher, I want the system to keep functioning even if RC is temporarily unavailable. | Medium |
| NF07 | Upgradeability – the system supports migrating from SQLite to PostgreSQL with no application-code change (SQLAlchemy). | As a developer, I want migrating to a stronger DB to be simple, so that we can grow without refactoring. | Low |
| NF08 | Reliability – on a WhatsApp send failure, the system retries 3 times with backoff. A failure is logged. | As a dispatcher, I want the system to retry a failed message, so that an offer is not lost. | Medium |
| NF09 | Logging – every critical action (send message, approval, timeout, API call) is logged with a timestamp and user identity. | As a system administrator, I want a full log, so that I can investigate incidents and check performance. | Medium |

## 7.3 Appendix – Record of AI-assisted requirements elicitation

> The requirement IDs cited inside this dialogue are the *preliminary* IDs used during elicitation and do not all match the final F01–F25 numbering in §7.1. They are preserved as recorded.

**Role definition for the AI:** We assigned Claude the role of Michel Azaryad — chief dispatcher and founder of Transfers TLV. The organization's goals, existing problems and operational challenges were presented to it.

**Initial prompt:**

> You are Michel Azaryad, chief dispatcher and founder of Transfers TLV. The company provides transport services for inbound tourism in Israel. Today, every driver assignment is done manually on WhatsApp — a process that consumes hours. Our team is building a new system for managing assignments. We want to understand your needs before we start developing. Please answer our questions as a genuine stakeholder in the organization.

**Q1: What are the biggest challenges in the current assignment process?**
A1 (AI as Michel): The biggest problem is that every assignment requires me to write a WhatsApp message manually to each driver. I have to open each chat, type the details, wait for a reply, and then update Ride Control by hand. On busy days with 10–15 trips I spend 3–4 hours just on communication.
*Requirements elicited: F15 (automatic messaging), F09 (display open trips)*

**Q2: What would you want the system to do for you?**
A2: I'd want the system to send the message automatically — without me writing it each time. And that the driver can confirm through a clear link, not through free-text I have to interpret.
*Requirements elicited: F15 (messaging), F17 (mobile approval page)*

**Q3: What happens when a driver does not respond in time?**
A3: Today I wait half an hour to an hour and then approach the next driver — every time manually. I'd want the system to do this automatically: after a defined interval, move to the next driver without my intervention.
*Requirements elicited: F22 (driver timeout)*

**Q4: What matters in choosing the right driver?**
A4: It's important the driver fits the vehicle type — don't offer a 7-passenger trip to a driver with a sedan. Also, priority to drivers near the trip rather than on the other side of the country.
*Requirements elicited: F12 (filter by qualification), F13 (AI ranking)*

**Q5: Are there problems tracking trip status?**
A5: Yes — at any moment I don't know the status of each trip. Did the driver approve? Still waiting? A problem? I'd want one screen showing me an immediate status picture.
*Requirements elicited: F27 (real-time trip monitoring)*

**Q6: Do you need reports?**
A6: At the end of each month I need to total, per driver, how many trips they did and what they are owed. Today it's a manual process that takes hours. I'd want the system to produce an orderly report and send it to the driver on WhatsApp.
*Requirements elicited: F06 (monthly report), F31 (send via WhatsApp)*

**Q7: What matters about the interface drivers will see?**
A7: Drivers always work from mobile. The page must be in Hebrew, right-to-left, with large clear buttons. I don't want a driver to approve a trip by mistake or get confused.
*Requirements elicited: F17 (mobile RTL interface), NF04 (RTL support), NF05 (mobile compatibility)*

**Q8: What happens when a driver confirms on WhatsApp but does not click the link?**
A8: That's a real problem. The driver writes "OK" but doesn't click the link. Today I assume he approves, and sometimes it turns out he didn't see all the details. I'd want link approval to be mandatory — and if it doesn't happen within an hour, the system moves to the next driver.
*Requirements elicited: F24 (link timeout)*

**Q9: How do you work with external suppliers?**
A9: Abroad I work with large suppliers that have dozens of drivers. I send them on WhatsApp "I have 5 trips to Amsterdam on the 15th" and they approve them one by one. I'd want an orderly mechanism for this — that the supplier receives a list and approves each trip separately.
*Requirements elicited: F29 (batch of trips to supplier), F32 (per-trip supplier approval)*

**Q10: Are there other needs we haven't discussed?**
A10: Yes — drivers sometimes write messages like "off tomorrow" in the middle of things, and someone has to update availability. I'd want the system to automatically understand this is an unavailability declaration and update the roster.
*Requirements elicited: F25 (AI reply interpretation), F26 (automatic availability update)*

**Summary:** The 10 requirements elicited from the dialogue (F06, F09, F12, F13, F15, F17, F22, F24, F25, F26, F27, F29, F31, F32) were merged into the functional requirements table above.

---

# 9 Traceability Matrix

The matrix links each functional requirement to one or more use cases. Its purpose is to verify that every requirement is covered by at least one use case and that no use case is "floating" without a requirement; it also helps identify complex use cases touching many requirements, which deserve special attention in design and testing. (The authoritative matrix is also exported from Visual Paradigm.)

The six **in-scope** use cases (the Trip Assignment Process) are shown in **bold**.

| Requirement | Use Case(s) |
|---|---|
| F01 | Manage External Drivers |
| F02 | Manage Activity Areas |
| F03 | Manage System Settings |
| F04 | Import External Trips; Update Trip Details |
| F05 | Generate Monthly Driver Report |
| F06 | Generate Performance Report |
| F07 | Create Manual Trip |
| F08 | **View Open Trips** |
| F09 | **Assign Trip to Region** |
| F10 | **Assign Trip to Region** |
| F11 | **Assign Trip to Region** |
| F12 | **Send Trip Offer** |
| F13 | **Send Trip Offer** |
| F14 | **Respond to Trip Offer** |
| F15 | **Respond to Trip Offer** |
| F16 | **Update Ride Control** |
| F17 | **Respond to Trip Offer** |
| F18 | **Forward Offer to Next Driver** |
| F19 | **Forward Offer to Next Driver** |
| F20 | **Respond to Trip Offer** |
| F21 | **Respond to Trip Offer** |
| F22 | — *(no use case in the current model — traceability gap)* |
| F23 | Import Drivers from Excel |
| F24 | **Forward Offer to Next Driver** |
| F25 | Send Monthly Report |

> **Gap:** F22 (Real-time Trip Monitoring) is not linked to any use case in the Visual Paradigm model. Either add a "Monitor Trips" use case or attach F22 to an existing one. NFRs (NF01–NF09) are cross-cutting and are not mapped to individual use cases.
