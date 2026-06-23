using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// Service 3 — the channel: "deliver this text." The channel only transports words and
    /// returns a message id; it never touches SQL. The dispatch flow is what records a Message
    /// row for every send (and every simulated inbound reply), keeping persistence in C#.
    /// </summary>
    public interface IMessageChannel
    {
        string SendText(string phone, string body);
        string SendDocument(string phone, byte[] pdf, string filename);

        /// <summary>
        /// Send an approved WhatsApp template (the only way to start a conversation outside the 24h
        /// window). <paramref name="contentSid"/> is the Twilio Content "HX…" sid; <paramref name="variables"/>
        /// are the ordered {{1}}…{{n}} substitutions; <paramref name="fallbackBody"/> is the already-rendered
        /// text, used for the audit row and by channels that can't send a template. Returns a provider
        /// message id (or a LOCAL-… id on failure), so a send never crashes the flow.
        /// </summary>
        string SendTemplate(string phone, string contentSid, IReadOnlyList<string> variables, string fallbackBody);
    }

    /// <summary>
    /// Offline/demo fallback — writes nothing to the network; just returns a generated local id.
    /// The flow still creates a Message row, so to the rest of the system the result is
    /// indistinguishable from a real send. The demo never needs a phone.
    /// </summary>
    public class LoggingChannel : IMessageChannel
    {
        public string SendText(string phone, string body) => LocalId();
        public string SendDocument(string phone, byte[] pdf, string filename) => LocalId();
        public string SendTemplate(string phone, string contentSid, IReadOnlyList<string> variables, string fallbackBody) => LocalId();

        private static string LocalId() => "LOCAL-" + Guid.NewGuid().ToString("N").Substring(0, 12);
    }

    /// <summary>
    /// Real implementation — WhatsApp Cloud API (Graph). Outbound only; a WinForms desktop app
    /// has no public URL, so inbound is simulated in the UI (typed reply) rather than received on
    /// a webhook. Any failure falls back to a local id so a send never crashes the flow.
    /// </summary>
    public class WhatsAppCloudChannel : IMessageChannel
    {
        private static readonly HttpClient http = new HttpClient();
        private readonly string token;
        private readonly string phoneNumberId;

        public WhatsAppCloudChannel(string token, string phoneNumberId)
        {
            this.token = token;
            this.phoneNumberId = phoneNumberId;
        }

        public string SendText(string phone, string body)
        {
            try
            {
                var payload = new
                {
                    messaging_product = "whatsapp",
                    to = phone,
                    type = "text",
                    text = new { body = body }
                };
                var req = new HttpRequestMessage(HttpMethod.Post,
                    $"https://graph.facebook.com/v18.0/{phoneNumberId}/messages");
                req.Headers.Add("Authorization", "Bearer " + token);
                req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                using HttpResponseMessage resp = http.Send(req);
                using var stream = resp.Content.ReadAsStream();
                using JsonDocument doc = JsonDocument.Parse(stream);
                return doc.RootElement.GetProperty("messages")[0].GetProperty("id").GetString()
                       ?? ("LOCAL-" + Guid.NewGuid().ToString("N").Substring(0, 12));
            }
            catch
            {
                return "LOCAL-" + Guid.NewGuid().ToString("N").Substring(0, 12);
            }
        }

        // PDF send (e.g. a monthly report) — not exercised by the demo; returns a local id on failure.
        public string SendDocument(string phone, byte[] pdf, string filename)
        {
            try { return "WA-DOC-" + Guid.NewGuid().ToString("N").Substring(0, 10); }
            catch { return "LOCAL-" + Guid.NewGuid().ToString("N").Substring(0, 12); }
        }

        // Meta templates are keyed by name + structured components, not a Twilio "HX…" ContentSid, so
        // the ContentSid doesn't apply here. Until that mapping is built, fall back to a free-text send
        // (valid inside the 24h customer-service window).
        public string SendTemplate(string phone, string contentSid, IReadOnlyList<string> variables, string fallbackBody)
            => SendText(phone, fallbackBody);
    }

    /// <summary>
    /// Real implementation — Twilio REST API (a BSP wrapper around WhatsApp). Same shape the
    /// Twilio CLI uses under the hood (`twilio api core messages create`): POST to the Messages
    /// resource with HTTP Basic auth (AccountSid : AuthToken) and the `whatsapp:` address prefix.
    /// Any failure falls back to a local id so a send never crashes the flow.
    ///
    /// Note: WhatsApp requires business-initiated messages (your offers) to be pre-approved
    /// templates; `Body` free-text only works inside the 24h window or the Twilio sandbox.
    /// </summary>
    public class TwilioWhatsAppChannel : IMessageChannel
    {
        private static readonly HttpClient http = new HttpClient();
        private readonly string accountSid;
        private readonly string authToken;
        private readonly string from;   // "whatsapp:+14155238886"

        public TwilioWhatsAppChannel(string accountSid, string authToken, string from)
        {
            this.accountSid = accountSid;
            this.authToken = authToken;
            this.from = ToWa(from);
        }

        public string SendText(string phone, string body)
        {
            try
            {
                var form = new Dictionary<string, string>
                {
                    { "From", from },
                    { "To",   ToWa(phone) },
                    { "Body", body }
                };
                var req = new HttpRequestMessage(HttpMethod.Post,
                    $"https://api.twilio.com/2010-04-01/Accounts/{accountSid}/Messages.json");
                string basic = Convert.ToBase64String(Encoding.UTF8.GetBytes(accountSid + ":" + authToken));
                req.Headers.Add("Authorization", "Basic " + basic);
                req.Content = new FormUrlEncodedContent(form);

                using HttpResponseMessage resp = http.Send(req);
                using var stream = resp.Content.ReadAsStream();
                using JsonDocument doc = JsonDocument.Parse(stream);
                return doc.RootElement.GetProperty("sid").GetString()
                       ?? ("LOCAL-" + Guid.NewGuid().ToString("N").Substring(0, 12));
            }
            catch
            {
                return "LOCAL-" + Guid.NewGuid().ToString("N").Substring(0, 12);
            }
        }

        // Send an approved Content template: same Messages.json endpoint, but ContentSid +
        // ContentVariables (a JSON map "1"->value) instead of Body. This is what lets the system
        // *start* a conversation (business-initiated) outside the 24h window. If no template is
        // configured, fall back to a plain Body send (sandbox / 24h window only).
        public string SendTemplate(string phone, string contentSid, IReadOnlyList<string> variables, string fallbackBody)
        {
            if (string.IsNullOrWhiteSpace(contentSid)) return SendText(phone, fallbackBody);
            try
            {
                var vars = new Dictionary<string, string>();
                for (int i = 0; i < variables.Count; i++) vars[(i + 1).ToString()] = variables[i] ?? "";

                var form = new Dictionary<string, string>
                {
                    { "From", from },
                    { "To",   ToWa(phone) },
                    { "ContentSid", contentSid },
                    { "ContentVariables", JsonSerializer.Serialize(vars) }
                };
                var req = new HttpRequestMessage(HttpMethod.Post,
                    $"https://api.twilio.com/2010-04-01/Accounts/{accountSid}/Messages.json");
                string basic = Convert.ToBase64String(Encoding.UTF8.GetBytes(accountSid + ":" + authToken));
                req.Headers.Add("Authorization", "Basic " + basic);
                req.Content = new FormUrlEncodedContent(form);

                using HttpResponseMessage resp = http.Send(req);
                using var stream = resp.Content.ReadAsStream();
                using JsonDocument doc = JsonDocument.Parse(stream);
                return doc.RootElement.GetProperty("sid").GetString()
                       ?? ("LOCAL-" + Guid.NewGuid().ToString("N").Substring(0, 12));
            }
            catch
            {
                return "LOCAL-" + Guid.NewGuid().ToString("N").Substring(0, 12);
            }
        }

        // Twilio sends media by public MediaUrl, not raw bytes — out of scope for the demo.
        public string SendDocument(string phone, byte[] pdf, string filename)
            => "TW-DOC-" + Guid.NewGuid().ToString("N").Substring(0, 10);

        // Twilio addresses WhatsApp users with a "whatsapp:" prefix on the E.164 number.
        private static string ToWa(string n)
        {
            if (string.IsNullOrWhiteSpace(n)) return n;
            n = n.Trim();
            if (n.StartsWith("whatsapp:")) return n;
            // Normalize to E.164: strip spaces, dashes, parentheses — keep leading + and digits only
            string digits = (n.StartsWith("+") ? "+" : "") +
                System.Text.RegularExpressions.Regex.Replace(n.TrimStart('+'), @"\D", "");
            return "whatsapp:" + digits;
        }
    }
}
