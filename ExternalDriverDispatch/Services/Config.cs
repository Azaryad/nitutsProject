using System.Collections.Generic;
using System.Configuration;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// Reads (and writes) external-service configuration in app.config (&lt;appSettings&gt;).
    ///
    /// THE ONE RULE THAT PROTECTS THE DEMO: every external service has a deterministic
    /// offline fallback, and the system must run end-to-end with all of them disabled.
    /// There is NO global master switch — offline/live is decided **per service** by its own
    /// `*.Enabled` flag (off = offline fallback). All flags default to false, so with no keys
    /// and no internet (the lab) the whole dispatch flow still works on the fallbacks. A missing
    /// key downgrades that one feature; it never crashes the program.
    ///
    /// WhatsApp has two providers, selectable via <see cref="WhatsAppProvider"/>: "meta"
    /// (Cloud API, graph.facebook.com) or "twilio" (Twilio REST API). Each has its own creds.
    ///
    /// Values can be edited from the in-app Settings screen (SettingsPanel) → <see cref="Save"/>.
    /// NOTE: Settings is a technical/NFR screen (like Login) — not a use case or entity, and must
    /// not appear in the class/UC diagrams.
    /// </summary>
    public static class Config
    {
        // Service 1 — Google Maps (Distance Matrix)
        public static bool   MapsEnabled;
        public static string MapsApiKey;

        // Service 2 — Claude AI agent
        public static bool   AiEnabled;
        public static string AiApiKey;
        public static string AiModel;

        // Service 3 — WhatsApp (provider-switchable)
        public static bool   WhatsAppEnabled;
        public static string WhatsAppProvider;        // "meta" | "twilio"
        // Meta WhatsApp Cloud API
        public static string WhatsAppToken;
        public static string WhatsAppPhoneNumberId;
        // Twilio
        public static string TwilioAccountSid;
        public static string TwilioAuthToken;
        public static string TwilioWhatsAppFrom;       // e.g. "whatsapp:+14155238886"
        public static string TwilioContentSid;         // approved WhatsApp template "HX..."; empty = plain Body

        static Config() { Reload(); }

        /// <summary>Re-read every value from app.config (call after a Save or at startup).</summary>
        public static void Reload()
        {
            ConfigurationManager.RefreshSection("appSettings");
            MapsEnabled           = GetBool("Maps.Enabled", false);
            MapsApiKey            = GetString("Maps.ApiKey", "");
            AiEnabled             = GetBool("Ai.Enabled", false);
            AiApiKey              = GetString("Ai.ApiKey", "");
            AiModel               = GetString("Ai.Model", "claude-sonnet-4-6");
            WhatsAppEnabled       = GetBool("WhatsApp.Enabled", false);
            WhatsAppProvider      = GetString("WhatsApp.Provider", "meta").ToLowerInvariant();
            WhatsAppToken         = GetString("WhatsApp.Token", "");
            WhatsAppPhoneNumberId = GetString("WhatsApp.PhoneNumberId", "");
            TwilioAccountSid      = GetString("Twilio.AccountSid", "");
            TwilioAuthToken       = GetString("Twilio.AuthToken", "");
            TwilioWhatsAppFrom    = GetString("Twilio.WhatsAppFrom", "whatsapp:+14155238886");
            TwilioContentSid      = GetString("Twilio.ContentSid", "");
        }

        /// <summary>
        /// Persist the given keys back to the running app's .config and reload.
        /// (Writes the output &lt;exe&gt;.config; an F5 rebuild copies the source app.config over it.)
        /// </summary>
        public static void Save(IDictionary<string, string> values)
        {
            Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            foreach (KeyValuePair<string, string> kv in values)
            {
                if (cfg.AppSettings.Settings[kv.Key] == null)
                    cfg.AppSettings.Settings.Add(kv.Key, kv.Value);
                else
                    cfg.AppSettings.Settings[kv.Key].Value = kv.Value;
            }
            cfg.Save(ConfigurationSaveMode.Modified);
            Reload();
        }

        // ---- helpers ----
        private static string GetString(string key, string dflt)
        {
            string v = ConfigurationManager.AppSettings[key];
            return string.IsNullOrWhiteSpace(v) ? dflt : v;
        }

        private static bool GetBool(string key, bool dflt)
        {
            string v = ConfigurationManager.AppSettings[key];
            return bool.TryParse(v, out bool b) ? b : dflt;
        }
    }
}
