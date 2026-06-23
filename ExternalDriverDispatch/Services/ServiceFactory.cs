namespace ExternalDriverDispatch
{
    /// <summary>
    /// Chooses, per service, the real implementation vs. the offline fallback.
    /// Offline/live is decided **independently per service** (there is no global master switch):
    /// a service goes live only when its own `*.Enabled` flag is on AND its credentials are present.
    /// Otherwise the deterministic fallback is returned — so the dispatch flow is identical with or
    /// without keys, and the UI/domain never see a concrete API class (they depend on the interface;
    /// this factory injects the implementation).
    ///
    /// WhatsApp additionally picks a provider: Meta Cloud API or Twilio.
    /// </summary>
    public static class ServiceFactory
    {
        private static bool MapsOn => Config.MapsEnabled && !string.IsNullOrEmpty(Config.MapsApiKey);
        private static bool AiOn   => Config.AiEnabled   && !string.IsNullOrEmpty(Config.AiApiKey);

        private static bool TwilioReady =>
            !string.IsNullOrEmpty(Config.TwilioAccountSid) && !string.IsNullOrEmpty(Config.TwilioAuthToken);
        private static bool MetaReady =>
            !string.IsNullOrEmpty(Config.WhatsAppToken) && !string.IsNullOrEmpty(Config.WhatsAppPhoneNumberId);

        // one shared Claude instance implements all four AI roles
        private static ClaudeAiService claude;
        private static ClaudeAiService Claude =>
            claude ??= new ClaudeAiService(Config.AiApiKey, Config.AiModel);

        // Service 1
        public static IDriveInfoProvider DriveInfo() =>
            MapsOn ? new GoogleMapsDriveInfoProvider(Config.MapsApiKey) : new StaticDriveInfoProvider();

        // Service 2 (four roles)
        public static IDriverRanker Ranker() => AiOn ? (IDriverRanker)Claude : new ProximityDriverRanker();
        public static IMessageComposer Composer() => AiOn ? (IMessageComposer)Claude : new TemplateMessageComposer();
        public static IReplyInterpreter Interpreter() => AiOn ? (IReplyInterpreter)Claude : new KeywordReplyInterpreter();
        public static IRestrictionParser RestrictionParser() => AiOn ? (IRestrictionParser)Claude : new KeywordRestrictionParser();

        // Service 3 — provider-switchable WhatsApp
        public static IMessageChannel Channel()
        {
            if (!Config.WhatsAppEnabled) return new LoggingChannel();
            if (Config.WhatsAppProvider == "twilio" && TwilioReady)
                return new TwilioWhatsAppChannel(Config.TwilioAccountSid, Config.TwilioAuthToken, Config.TwilioWhatsAppFrom);
            if (Config.WhatsAppProvider == "meta" && MetaReady)
                return new WhatsAppCloudChannel(Config.WhatsAppToken, Config.WhatsAppPhoneNumberId);
            return new LoggingChannel();
        }

        /// <summary>What WhatsApp is actually doing right now (for the log / settings line).</summary>
        public static string WhatsAppMode()
        {
            if (!Config.WhatsAppEnabled) return "offline";
            if (Config.WhatsAppProvider == "twilio") return TwilioReady ? "twilio (live)" : "twilio (no creds → offline)";
            return MetaReady ? "meta (live)" : "meta (no creds → offline)";
        }

        /// <summary>Human-readable mode line for the activity log header / settings screen.</summary>
        public static string ModeSummary()
        {
            return "Services: Maps=" + (MapsOn ? "live" : "offline") +
                   ", AI=" + (AiOn ? "live" : "offline") +
                   ", WhatsApp=" + WhatsAppMode();
        }
    }
}
