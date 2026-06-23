using System;
using System.Collections.Generic;

namespace ExternalDriverDispatch
{
    // =====================================================================
    // ENUMERATIONS
    // Literals match the DB tokens exactly, so enum.ToString() returns the
    // stored token and Enum.Parse round-trips. The Helper classes below map
    // each enum to a human-readable English label for the UI.
    // =====================================================================

    /// <summary>Vehicle categories eligible for trips.</summary>
    public enum VehicleType
    {
        sedan,
        executive_minivan,
        minivan,
        minibus_15,
        minibus_18
    }

    /// <summary>Lifecycle state of a trip.</summary>
    public enum TripStatus
    {
        open,
        assigned_to_region,
        offered,
        confirmed,
        completed,
        cancelled,
        manual_assignment
    }

    /// <summary>Lifecycle state of a single outreach attempt to a driver.</summary>
    public enum OfferStatus
    {
        pending,
        pending_approval,
        accepted,
        rejected,
        timeout,
        approval_timeout,
        cancelled
    }

    /// <summary>
    /// Direction of a WhatsApp message in the conversation audit trail.
    /// (Literals are 'inbound'/'outbound' rather than in/out because in/out are C# keywords.)
    /// </summary>
    public enum MessageDirection
    {
        inbound,
        outbound
    }

    // =====================================================================
    // Helpers — map enum (DB token) <-> English display label.
    // ToDb / FromDb     -> the English token stored in the DB (enum.ToString()).
    // ToDisplay / FromDisplay -> a readable English label for the UI.
    // =====================================================================

    public static class VehicleTypeHelper
    {
        private static readonly Dictionary<VehicleType, string> display = new Dictionary<VehicleType, string>
        {
            { VehicleType.sedan,             "Sedan" },
            { VehicleType.executive_minivan, "Executive Minivan" },
            { VehicleType.minivan,           "Minivan" },
            { VehicleType.minibus_15,        "Minibus 15" },
            { VehicleType.minibus_18,        "Minibus 18" }
        };

        public static string ToDb(VehicleType v) { return v.ToString(); }
        public static VehicleType FromDb(string s) { return (VehicleType)Enum.Parse(typeof(VehicleType), s); }

        public static string ToDisplay(VehicleType v) { return display[v]; }
        public static VehicleType FromDisplay(string label)
        {
            foreach (var kv in display)
                if (kv.Value == label) return kv.Key;
            return FromDb(label); // fallback: maybe the raw token was passed
        }
    }

    public static class TripStatusHelper
    {
        private static readonly Dictionary<TripStatus, string> display = new Dictionary<TripStatus, string>
        {
            { TripStatus.open,                "Open" },
            { TripStatus.assigned_to_region,  "Assigned to Region" },
            { TripStatus.offered,             "Offered" },
            { TripStatus.confirmed,           "Confirmed" },
            { TripStatus.completed,           "Completed" },
            { TripStatus.cancelled,           "Cancelled" },
            { TripStatus.manual_assignment,   "Manual Assignment" }
        };

        public static string ToDb(TripStatus v) { return v.ToString(); }
        public static TripStatus FromDb(string s) { return (TripStatus)Enum.Parse(typeof(TripStatus), s); }

        public static string ToDisplay(TripStatus v) { return display[v]; }
        public static TripStatus FromDisplay(string label)
        {
            foreach (var kv in display)
                if (kv.Value == label) return kv.Key;
            return FromDb(label);
        }
    }

    public static class OfferStatusHelper
    {
        private static readonly Dictionary<OfferStatus, string> display = new Dictionary<OfferStatus, string>
        {
            { OfferStatus.pending,          "Pending" },
            { OfferStatus.pending_approval, "Pending Approval" },
            { OfferStatus.accepted,         "Accepted" },
            { OfferStatus.rejected,         "Rejected" },
            { OfferStatus.timeout,          "Timeout" },
            { OfferStatus.approval_timeout, "Approval Timeout" },
            { OfferStatus.cancelled,        "Cancelled" }
        };

        public static string ToDb(OfferStatus v) { return v.ToString(); }
        public static OfferStatus FromDb(string s) { return (OfferStatus)Enum.Parse(typeof(OfferStatus), s); }

        public static string ToDisplay(OfferStatus v) { return display[v]; }
        public static OfferStatus FromDisplay(string label)
        {
            foreach (var kv in display)
                if (kv.Value == label) return kv.Key;
            return FromDb(label);
        }
    }

    public static class MessageDirectionHelper
    {
        private static readonly Dictionary<MessageDirection, string> display = new Dictionary<MessageDirection, string>
        {
            { MessageDirection.inbound,  "Inbound" },
            { MessageDirection.outbound, "Outbound" }
        };

        public static string ToDb(MessageDirection v) { return v.ToString(); }
        public static MessageDirection FromDb(string s) { return (MessageDirection)Enum.Parse(typeof(MessageDirection), s); }

        public static string ToDisplay(MessageDirection v) { return display[v]; }
        public static MessageDirection FromDisplay(string label)
        {
            foreach (var kv in display)
                if (kv.Value == label) return kv.Key;
            return FromDb(label);
        }
    }
}
