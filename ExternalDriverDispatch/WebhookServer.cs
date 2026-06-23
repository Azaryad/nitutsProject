using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// Lightweight HTTP server that receives Twilio's inbound WhatsApp webhook POSTs and
    /// fires <see cref="OnReply"/> for each message. Runs on a background thread so it
    /// never blocks the UI. Twilio requires a 200 response — we return a minimal TwiML
    /// empty Response so Twilio doesn't retry or flag an error.
    ///
    /// Port selection is dynamic: <see cref="Start"/> scans <see cref="BasePort"/>..(BasePort+ScanRange)
    /// and binds the first port it can actually open, so a stale http.sys registration or another
    /// process holding the default port can no longer wedge the app. The chosen <see cref="Port"/>
    /// is then used to start ngrok and update the Twilio webhook.
    ///
    /// Expose locally via:  ngrok http &lt;Port&gt; --host-header=rewrite
    /// </summary>
    public class WebhookServer : IDisposable
    {
        /// <summary>First port to try; the scanner walks upward from here.</summary>
        public const int BasePort = 5051;
        /// <summary>How many consecutive ports to try before giving up.</summary>
        public const int ScanRange = 30;

        /// <summary>The port actually bound after <see cref="Start"/> (0 until then).</summary>
        public int Port { get; private set; }

        /// <summary>True if bound to the wildcard prefix (accepts any Host header).</summary>
        public bool BoundWildcard { get; private set; }

        /// <summary>Narrative sink for the port scan — the board points this at its activity log.</summary>
        public Action<string> Log = _ => { };

        /// <summary>
        /// Fired on a background thread for each inbound message.
        /// Consumer must marshal to the UI thread (this.Invoke) before touching Controls.
        /// Parameters: (fromPhone, messageBody) — fromPhone is already stripped of "whatsapp:" prefix.
        /// </summary>
        public Action<string, string> OnReply;

        private HttpListener _listener;
        private Thread _thread;
        private bool _wildcardUnavailable;   // set once we learn "+" needs elevation we don't have

        public void Start()
        {
            // Scan upward from BasePort and bind the first port that actually opens.
            for (int p = BasePort; p < BasePort + ScanRange; p++)
            {
                if (TryBind(p))
                {
                    Port = p;
                    string mode = BoundWildcard ? "any-host" : "localhost (needs ngrok --host-header=rewrite)";
                    Log($"[Webhook] Bound port {p} [{mode}].");
                    _thread = new Thread(Listen) { IsBackground = true, Name = "WebhookListener" };
                    _thread.Start();
                    return;
                }
                Log($"[Webhook] Port {p} unavailable — trying {p + 1}.");
            }
            throw new Exception($"No free port found in range {BasePort}-{BasePort + ScanRange - 1} for the webhook listener.");
        }

        // Try to bind one port. Prefer the strong-wildcard prefix "+": it matches ANY Host header,
        // so an inbound request whose Host is the ngrok domain (or 127.0.0.1) is accepted. A
        // "localhost" prefix makes http.sys answer 400 for any other Host (hence --host-header=rewrite).
        // The wildcard needs a urlacl reservation or elevation; if that's denied (error 5) we stop
        // attempting it and use "localhost" for the rest of the scan.
        private bool TryBind(int port)
        {
            if (!_wildcardUnavailable)
            {
                if (TryBindPrefix($"http://+:{port}/", out bool denied))
                {
                    BoundWildcard = true;
                    return true;
                }
                // if "+" was refused for lack of privilege, don't try it again on later ports
                if (denied) _wildcardUnavailable = true;
            }

            if (TryBindPrefix($"http://localhost:{port}/", out _))
            {
                BoundWildcard = false;
                return true;
            }
            return false;
        }

        private bool TryBindPrefix(string prefix, out bool accessDenied)
        {
            accessDenied = false;
            try
            {
                var listener = new HttpListener();
                listener.Prefixes.Add(prefix);
                listener.Start();
                _listener = listener;
                return true;
            }
            catch (HttpListenerException ex)
            {
                // 5 = ERROR_ACCESS_DENIED (no admin/urlacl); 183 = ERROR_ALREADY_EXISTS (port/prefix taken)
                accessDenied = (ex.ErrorCode == 5);
                return false;
            }
        }

        /// <summary>
        /// Read-only probe of the scan range — reports which ports a webhook listener could bind
        /// right now. Used to surface "open ports" to the operator without side effects.
        /// </summary>
        public static List<(int Port, bool Free)> ScanPorts()
        {
            var result = new List<(int, bool)>();
            for (int p = BasePort; p < BasePort + ScanRange; p++)
            {
                bool free = false;
                try
                {
                    var l = new HttpListener();
                    l.Prefixes.Add($"http://localhost:{p}/");
                    l.Start();
                    l.Stop(); l.Close();
                    free = true;
                }
                catch { free = false; }
                result.Add((p, free));
            }
            return result;
        }

        public void Stop()
        {
            try { _listener?.Stop(); _listener?.Close(); } catch { }
        }

        public void Dispose() => Stop();

        private void Listen()
        {
            while (_listener.IsListening)
            {
                HttpListenerContext ctx;
                try { ctx = _listener.GetContext(); }
                catch (HttpListenerException) { break; }
                catch { break; }
                ThreadPool.QueueUserWorkItem(_ => Handle(ctx));
            }
        }

        private void Handle(HttpListenerContext ctx)
        {
            try
            {
                string body = "", from = "";
                using (var reader = new StreamReader(ctx.Request.InputStream, Encoding.UTF8))
                {
                    // Twilio POSTs application/x-www-form-urlencoded
                    foreach (string pair in reader.ReadToEnd().Split('&'))
                    {
                        int eq = pair.IndexOf('=');
                        if (eq < 0) continue;
                        string key = WebUtility.UrlDecode(pair.Substring(0, eq));
                        string val = WebUtility.UrlDecode(pair.Substring(eq + 1));
                        if (key == "Body") body = val;
                        else if (key == "From") from = val;
                    }
                }

                // Twilio requires a 200 with valid TwiML — otherwise it retries
                byte[] twiml = Encoding.UTF8.GetBytes("<?xml version=\"1.0\"?><Response/>");
                ctx.Response.StatusCode = 200;
                ctx.Response.ContentType = "text/xml";
                ctx.Response.ContentLength64 = twiml.Length;
                ctx.Response.OutputStream.Write(twiml, 0, twiml.Length);
                ctx.Response.Close();

                if (!string.IsNullOrWhiteSpace(from) && !string.IsNullOrWhiteSpace(body))
                {
                    // strip "whatsapp:" prefix before handing to the board
                    if (from.StartsWith("whatsapp:")) from = from.Substring("whatsapp:".Length);
                    OnReply?.Invoke(from, body);
                }
            }
            catch { }
        }
    }
}
