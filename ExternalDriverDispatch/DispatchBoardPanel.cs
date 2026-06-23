using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// Dispatch Board — the dispatcher's main operations screen (end-to-end find-a-driver flow).
    /// The board is pure presentation: it calls <see cref="DispatchService"/> domain methods, which
    /// in turn drive the three external services (Maps, Claude AI, WhatsApp) behind interfaces.
    /// The board never calls a service or the network directly.
    ///
    ///   ① pick a trip → ② assign region (Maps enriches distance/ETA) →
    ///   ③ ranked eligible drivers (AI) → ④ send offer (AI composes, WhatsApp sends) →
    ///   ⑥ simulate the driver reply (free text → AI interpret → Offer state machine),
    ///   forwarding to the next driver on decline/timeout or escalating when none remain.
    /// </summary>
    public partial class DispatchBoardPanel : UserControl
    {
        private readonly DispatchService svc = new DispatchService();
        private Trip selectedTrip;
        private List<RankedDriver> ranked = new List<RankedDriver>();

        // App-scoped (static): the inbound webhook + ngrok tunnel start ONCE and are reused across
        // board instances. showPanel() clears panels without disposing them, so a per-board listener
        // would never release its port — re-entering the board (e.g. from Settings) climbed ports and
        // churned ngrok/Twilio, breaking inbound. Static + a one-time guard keeps the setup stable;
        // each new board only re-points the reply handler at itself.
        private static WebhookServer _webhookServer;
        private static System.Diagnostics.Process _ngrokProcess;
        private static bool _hostStarted;
        private static string _tunnelUrl;

        public DispatchBoardPanel()
        {
            InitializeComponent();
            UiTheme.Apply(this);
            svc.Log = log;                       // the service narrates into the activity log
            refreshRegionCombo();
            loadTrips();
            log(ServiceFactory.ModeSummary());   // shows Maps/AI/WhatsApp = live or fallback
            log("Welcome to the Dispatch Board. Select a trip from the queue to begin.");

            startWebhookServer();                // starts once; always re-points OnReply at this board

            if (!_hostStarted)
            {
                _hostStarted = true;
                // auto-start the ngrok tunnel once, as soon as the handle exists
                EventHandler onHandleCreated = null;
                onHandleCreated = async (s, e) =>
                {
                    this.HandleCreated -= onHandleCreated;
                    await startTunnelInBackground();
                };
                this.HandleCreated += onHandleCreated;
            }
            else if (_tunnelUrl != null)
            {
                // tunnel already live from an earlier board — reflect it on this board's button
                log($"[Webhook] Reusing listener on port {_webhookServer?.Port} · tunnel {_tunnelUrl}");
                EventHandler reflect = null;
                reflect = (s, e) =>
                {
                    this.HandleCreated -= reflect;
                    btnTunnel.Text = "🌐 Tunnel: live";
                    btnTunnel.ForeColor = System.Drawing.Color.DarkGreen;
                };
                this.HandleCreated += reflect;
            }
        }

        // ===================== open-trips queue =====================
        private void loadTrips()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Booking", typeof(string));
            dt.Columns.Add("Origin", typeof(string));
            dt.Columns.Add("Destination", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("Pax", typeof(int));
            dt.Columns.Add("Vehicle", typeof(string));
            dt.Columns.Add("Region", typeof(string));
            dt.Columns.Add("Status", typeof(string));

            foreach (Trip t in Program.Trips)
            {
                // show only active trips: open / assigned_to_region / offered / manual_assignment
                TripStatus s = t.getStatus();
                if (s != TripStatus.open && s != TripStatus.assigned_to_region &&
                    s != TripStatus.offered && s != TripStatus.manual_assignment)
                    continue;
                string region = t.getRegion() != null ? t.getRegion().getName() : "";
                dt.Rows.Add(t.getId(), t.getExternalBookingId(), t.getPickupCity(), t.getDropoffCity(),
                    t.getPickupTime().ToString("dd/MM HH:mm"), t.getNumPassengers(),
                    VehicleTypeHelper.ToDisplay(t.getVehicleType()), region, TripStatusHelper.ToDisplay(s));
            }
            dgvTrips.DataSource = dt;
        }

        private void refreshRegionCombo()
        {
            comboRegion.Items.Clear();
            foreach (Region r in Program.Regions)
                comboRegion.Items.Add(r.getName());
        }

        private void dgvTrips_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int id = Convert.ToInt32(dgvTrips.Rows[e.RowIndex].Cells["ID"].Value);
            selectedTrip = Trip.seekTrip(id);
            if (selectedTrip == null) return;
            lblTrip.Text = "Trip #" + selectedTrip.getId() + " · " + selectedTrip.getExternalBookingId();
            if (selectedTrip.getRegion() != null)
                comboRegion.SelectedIndex = Program.Regions.IndexOf(selectedTrip.getRegion());
            refreshDrivers();
            refreshOffers();
        }

        // ===================== assign to region (Maps enrich) =====================
        private void btnAssignRegion_Click(object sender, EventArgs e)
        {
            if (selectedTrip == null) { warn("Select a trip first"); return; }
            if (comboRegion.SelectedIndex < 0) { warn("Select a region"); return; }
            Region region = Program.Regions[comboRegion.SelectedIndex];
            if (!runQuietBool(() => selectedTrip.assignRegion(region))) return;   // open -> assigned_to_region
            log("Trip assigned to region: " + region.getName());
            svc.EnrichTrip(selectedTrip);      // Service 1 — fill distance / ETA
            refreshDrivers();
            loadTrips();
        }

        // ===================== ranked eligible drivers (AI) =====================
        private void refreshDrivers()
        {
            ranked = (selectedTrip != null ? svc.RankEligible(selectedTrip) : new List<RankedDriver>()).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("Rank", typeof(int));
            dt.Columns.Add("Driver", typeof(string));
            dt.Columns.Add("Vehicle", typeof(string));
            dt.Columns.Add("City", typeof(string));
            dt.Columns.Add("Why", typeof(string));
            foreach (RankedDriver rd in ranked)
                dt.Rows.Add(rd.Rank, rd.Driver.getName(), VehicleTypeHelper.ToDisplay(rd.Driver.getVehicleType()),
                    rd.Driver.getHomeCity(), rd.Reason);
            dgvDrivers.DataSource = dt;
        }

        // ===================== send offer (AI compose + WhatsApp send) =====================
        private void btnSendOffer_Click(object sender, EventArgs e)
        {
            if (selectedTrip == null) { warn("Select a trip"); return; }
            if (selectedTrip.getStatus() != TripStatus.assigned_to_region)
            {
                warn("Offers can only be sent for a trip assigned to a region (current status: " +
                     TripStatusHelper.ToDisplay(selectedTrip.getStatus()) + ")");
                return;
            }
            RankedDriver rd = pickDriverFromGridOrTop();
            if (rd == null) { warn("No eligible driver available in this region"); return; }
            svc.SendOffer(selectedTrip, rd);
            loadTrips(); refreshDrivers(); refreshOffers();
        }

        private RankedDriver pickDriverFromGridOrTop()
        {
            // if a driver row is selected -> use it; otherwise the top-ranked driver
            if (dgvDrivers.CurrentRow != null && dgvDrivers.CurrentRow.Index >= 0 &&
                dgvDrivers.CurrentRow.Index < ranked.Count)
                return ranked[dgvDrivers.CurrentRow.Index];
            return ranked.Count > 0 ? ranked[0] : null;
        }

        // ===================== offers for the selected trip =====================
        private void refreshOffers()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Driver", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Rank", typeof(int));
            if (selectedTrip != null)
                foreach (Offer o in selectedTrip.getOffers())
                    dt.Rows.Add(o.getId(), o.getDriver() != null ? o.getDriver().getName() : "",
                        OfferStatusHelper.ToDisplay(o.getStatus()), o.getRankPosition());
            dgvOffers.DataSource = dt;
        }

        private Offer requireOffer()
        {
            if (selectedTrip == null) { warn("Select a trip"); return null; }
            if (dgvOffers.CurrentRow == null || dgvOffers.CurrentRow.Index < 0)
            { warn("Select an offer from the list"); return null; }
            object cell = dgvOffers.CurrentRow.Cells["ID"].Value;
            if (cell == null) { warn("Select an offer from the list"); return null; }
            return Offer.seekOffer(Convert.ToInt32(cell));
        }

        // ===================== driver response =====================

        // Free-text WhatsApp reply -> AI interpret -> Offer state machine (+ forward on "no").
        private void btnReceiveReply_Click(object sender, EventArgs e)
        {
            Offer o = requireOffer(); if (o == null) return;
            string text = txtReply.Text.Trim();
            if (text.Length == 0) { warn("Type the driver's reply text first"); return; }
            Trip t = o.getTrip();
            ReplyIntent intent = svc.HandleDriverReply(o, text);
            if (intent == ReplyIntent.No) svc.Forward(t);   // re-queue to the next driver
            txtReply.Clear();
            loadTrips(); refreshOffers(); refreshDrivers();
        }

        // driver replied "yes" on WhatsApp (soft intent) — manual shortcut
        private void btnWhatsappYes_Click(object sender, EventArgs e)
        {
            Offer o = requireOffer(); if (o == null) return;
            if (runQuietBool(() => o.markPendingApproval()))
            {
                log(o.getDriver().getName() + " replied 'yes' (intent). Reminder sent to click the approval link.");
                refreshOffers();
            }
        }

        // clicked the approval link — binding accept (triggers Ride Control update, UC08)
        private void btnApprove_Click(object sender, EventArgs e)
        {
            Offer o = requireOffer(); if (o == null) return;
            if (runQuietBool(() => o.accept()))
            {
                log("✓ " + o.getDriver().getName() + " approved! Trip confirmed · Ride Control updated (driver name, phone, vehicle).");
                loadTrips(); refreshOffers(); refreshDrivers();
            }
        }

        // decline — forward to next driver
        private void btnDecline_Click(object sender, EventArgs e)
        {
            Offer o = requireOffer(); if (o == null) return;
            Trip t = o.getTrip();
            if (runQuietBool(() => o.reject()))
            {
                log("✗ " + o.getDriver().getName() + " declined. Finding next driver...");
                svc.Forward(t);
                loadTrips(); refreshOffers(); refreshDrivers();
            }
        }

        // timeout — forward to next driver
        private void btnTimeout_Click(object sender, EventArgs e)
        {
            Offer o = requireOffer(); if (o == null) return;
            Trip t = o.getTrip();
            if (runQuietBool(() => o.timeout()))
            {
                log("⌛ Offer to " + o.getDriver().getName() + " timed out. Finding next driver...");
                svc.Forward(t);
                loadTrips(); refreshOffers(); refreshDrivers();
            }
        }

        // ===================== real inbound webhook =====================

        private void startWebhookServer()
        {
            // route inbound replies to THIS board (the visible one), marshaled to the UI thread
            Action<string, string> route = (phone, body) =>
            {
                if (this.IsHandleCreated && !this.IsDisposed)
                    this.Invoke((Action)(() => handleWebhookReply(phone, body)));
            };

            // reuse the already-running listener from a previous board — just swap the handler
            if (_webhookServer != null)
            {
                _webhookServer.OnReply = route;
                return;
            }

            try
            {
                _webhookServer = new WebhookServer();
                _webhookServer.Log = log;   // narrate the port scan
                _webhookServer.OnReply = route;
                _webhookServer.Start();
                log($"[Webhook] Listening on port {_webhookServer.Port} — inbound driver replies are live.");
            }
            catch (Exception ex)
            {
                log("[Webhook] Could not start server: " + ex.Message);
                _webhookServer = null;
            }
        }

        private void handleWebhookReply(string fromPhone, string body)
        {
            log($"[Webhook ←] {fromPhone}: \"{body}\"");

            string normalized = normalizePhone(fromPhone);
            Offer activeOffer = null;
            foreach (Offer o in Program.Offers)
            {
                if (o.getDriver() == null) continue;
                if (normalizePhone(o.getDriver().getPhone()) != normalized) continue;
                if (o.getStatus() != OfferStatus.pending && o.getStatus() != OfferStatus.pending_approval) continue;
                if (activeOffer == null || o.getSentAt() > activeOffer.getSentAt())
                    activeOffer = o;
            }

            if (activeOffer == null)
            {
                log($"[Webhook] No active offer found for {fromPhone} — message ignored.");
                return;
            }

            Trip t = activeOffer.getTrip();
            ReplyIntent intent = svc.HandleDriverReply(activeOffer, body);
            if (intent == ReplyIntent.No) svc.Forward(t);
            loadTrips(); refreshOffers(); refreshDrivers();
        }

        // strip whatsapp: prefix, spaces, dashes — same logic as TwilioWhatsAppChannel.ToWa in reverse
        private static string normalizePhone(string n)
        {
            if (string.IsNullOrWhiteSpace(n)) return "";
            n = n.Trim();
            if (n.StartsWith("whatsapp:")) n = n.Substring("whatsapp:".Length);
            return (n.StartsWith("+") ? "+" : "") +
                   System.Text.RegularExpressions.Regex.Replace(n.TrimStart('+'), @"\D", "");
        }

        // ===================== ngrok tunnel =====================

        private async void btnTunnel_Click(object sender, EventArgs e) =>
            await startTunnelInBackground();

        private async System.Threading.Tasks.Task startTunnelInBackground()
        {
            btnTunnel.Enabled = false;
            btnTunnel.Text = "🌐 Connecting...";
            try
            {
                string url = await System.Threading.Tasks.Task.Run(() => getOrStartNgrokTunnel());
                _tunnelUrl = url;
                log($"[Tunnel] Active: {url}");
                Clipboard.SetText(url);

                bool updated = await System.Threading.Tasks.Task.Run(() => tryUpdateTwilioWebhook(url));
                if (updated)
                    log("[Tunnel] Twilio webhook updated automatically.");
                else
                    log("[Tunnel] URL copied to clipboard — paste into Twilio Console → Messaging → " +
                        "WhatsApp Senders → " + Config.TwilioWhatsAppFrom.Replace("whatsapp:", "") + " → Webhook URL.");

                btnTunnel.Text = "🌐 Tunnel: live";
                btnTunnel.ForeColor = System.Drawing.Color.DarkGreen;
                btnTunnel.Enabled = true;
            }
            catch (Exception ex)
            {
                log("[Tunnel] " + ex.Message);
                btnTunnel.Text = "🌐 Start Tunnel";
                btnTunnel.ForeColor = System.Drawing.Color.DarkSlateBlue;
                btnTunnel.Enabled = true;
            }
        }

        private string getOrStartNgrokTunnel()
        {
            // ngrok must forward to the port the webhook server actually bound (the scanner may have
            // picked something other than the default if it was occupied).
            int port = _webhookServer?.Port ?? WebhookServer.BasePort;

            // reuse an existing tunnel ONLY if it already forwards to our port
            string url = getNgrokPublicUrl(port);
            if (url != null) return url;

            // a tunnel may be running on a stale port (a previous run picked 5051, we're now on 5052).
            // ngrok free-tier allows one agent session, so kill any running agent before starting fresh.
            if (ngrokIsRunning())
            {
                log($"[Tunnel] Existing ngrok tunnel is on the wrong port — restarting it on {port}.");
                killNgrok();
            }

            try
            {
                string ngrokExe = findNgrok();
                // --host-header=rewrite makes ngrok forward the request with Host "localhost:<port>"
                // instead of the public ngrok domain. Without it, http.sys rejects the inbound
                // Twilio POST with 400 because the Host doesn't match the listener's prefix.
                var psi = new System.Diagnostics.ProcessStartInfo(ngrokExe, $"http {port} --host-header=rewrite")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                _ngrokProcess = System.Diagnostics.Process.Start(psi);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                throw new Exception("ngrok not found. Install it with:  winget install ngrok.ngrok");
            }

            for (int i = 0; i < 16; i++)
            {
                System.Threading.Thread.Sleep(500);
                url = getNgrokPublicUrl(port);
                if (url != null) return url;
            }
            throw new Exception($"ngrok did not start within 8 s. Try: ngrok config add-authtoken <your-token>  then retry.");
        }

        private bool ngrokIsRunning()
        {
            try
            {
                using var http = new System.Net.Http.HttpClient { Timeout = TimeSpan.FromSeconds(2) };
                _ = http.GetStringAsync("http://127.0.0.1:4040/api/tunnels").Result;
                return true;
            }
            catch { return false; }
        }

        private void killNgrok()
        {
            try
            {
                foreach (var p in System.Diagnostics.Process.GetProcessesByName("ngrok"))
                {
                    try { p.Kill(); p.WaitForExit(2000); } catch { }
                }
                System.Threading.Thread.Sleep(800);   // let http.sys / the agent session free up
            }
            catch { }
        }

        private static string findNgrok()
        {
            // 1. ngrok on PATH (the normal case after a proper install)
            string onPath = findOnPath("ngrok");
            if (onPath != null) return onPath;

            // 2. winget packages directory (winget installs here but doesn't add to PATH)
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string wingetDir = System.IO.Path.Combine(localAppData, "Microsoft", "WinGet", "Packages");
            if (System.IO.Directory.Exists(wingetDir))
            {
                foreach (string dir in System.IO.Directory.GetDirectories(wingetDir, "Ngrok.*"))
                {
                    string exe = System.IO.Path.Combine(dir, "ngrok.exe");
                    if (System.IO.File.Exists(exe)) return exe;
                }
            }

            // 3. common manual install locations
            string[] candidates = {
                System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ngrok", "ngrok.exe"),
                System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "ngrok.exe"),
                System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "bin", "ngrok.exe"),
            };
            foreach (string c in candidates)
                if (System.IO.File.Exists(c)) return c;

            throw new System.ComponentModel.Win32Exception("ngrok executable not found");
        }

        private static string findOnPath(string name)
        {
            string pathExt = Environment.GetEnvironmentVariable("PATHEXT") ?? ".EXE";
            string[] exts = pathExt.Split(';');
            foreach (string dir in (Environment.GetEnvironmentVariable("PATH") ?? "").Split(';'))
            {
                if (string.IsNullOrWhiteSpace(dir)) continue;
                foreach (string ext in exts)
                {
                    string full = System.IO.Path.Combine(dir.Trim(), name + ext);
                    if (System.IO.File.Exists(full)) return full;
                }
            }
            return null;
        }

        // Return the https public URL of the running ngrok tunnel that forwards to <port>, or null.
        // Matching on the tunnel's local addr is essential: if the webhook moved to a new port, a
        // tunnel still pointing at the old port must NOT be reused (replies would 404 into the void).
        private string getNgrokPublicUrl(int port)
        {
            try
            {
                // Use 127.0.0.1 explicitly — on Windows, "localhost" resolves to ::1 (IPv6)
                // but ngrok's dashboard only binds to the IPv4 loopback.
                using var http = new System.Net.Http.HttpClient { Timeout = TimeSpan.FromSeconds(3) };
                string json = http.GetStringAsync("http://127.0.0.1:4040/api/tunnels").Result;
                using var doc = System.Text.Json.JsonDocument.Parse(json);
                foreach (var t in doc.RootElement.GetProperty("tunnels").EnumerateArray())
                {
                    if (t.GetProperty("proto").GetString() != "https") continue;
                    string addr = t.GetProperty("config").GetProperty("addr").GetString() ?? "";
                    // addr looks like "http://localhost:5051"
                    if (addr.EndsWith(":" + port))
                        return t.GetProperty("public_url").GetString();
                }
                return null;
            }
            catch (Exception ex)
            {
                log($"[Tunnel] Dashboard check failed: {ex.Message}");
                return null;
            }
        }

        // Re-point the Twilio WhatsApp Sender's inbound webhook at the current ngrok URL.
        // A WhatsApp Sender is NOT an IncomingPhoneNumber — its callback lives under the
        // Messaging v2 "Channels/Senders" resource. We look up the sender whose sender_id
        // matches Config.TwilioWhatsAppFrom, then PATCH its webhook.callback_url. This must
        // run on every launch because ngrok free-tier URLs change each restart.
        private bool tryUpdateTwilioWebhook(string ngrokUrl)
        {
            if (string.IsNullOrEmpty(Config.TwilioAccountSid) || string.IsNullOrEmpty(Config.TwilioAuthToken))
                return false;
            try
            {
                string fromAddr = Config.TwilioWhatsAppFrom.StartsWith("whatsapp:")
                    ? Config.TwilioWhatsAppFrom
                    : "whatsapp:" + Config.TwilioWhatsAppFrom.Replace("whatsapp:", "");

                using var http = new System.Net.Http.HttpClient();
                string basic = Convert.ToBase64String(
                    System.Text.Encoding.UTF8.GetBytes(Config.TwilioAccountSid + ":" + Config.TwilioAuthToken));
                http.DefaultRequestHeaders.Add("Authorization", "Basic " + basic);

                // 1. find the WhatsApp sender matching our From address
                string listResp = http.GetStringAsync(
                    "https://messaging.twilio.com/v2/Channels/Senders?Channel=whatsapp&PageSize=50").Result;
                using var doc = System.Text.Json.JsonDocument.Parse(listResp);
                string senderSid = null;
                foreach (var s in doc.RootElement.GetProperty("senders").EnumerateArray())
                {
                    if (s.TryGetProperty("sender_id", out var sidProp) &&
                        string.Equals(sidProp.GetString(), fromAddr, StringComparison.OrdinalIgnoreCase))
                    {
                        senderSid = s.GetProperty("sid").GetString();
                        break;
                    }
                }
                if (senderSid == null) { log($"[Tunnel] No WhatsApp sender matched {fromAddr}."); return false; }

                // 2. update its inbound webhook
                var payload = new { webhook = new { callback_url = ngrokUrl, callback_method = "POST" } };
                var content = new System.Net.Http.StringContent(
                    System.Text.Json.JsonSerializer.Serialize(payload),
                    System.Text.Encoding.UTF8, "application/json");
                var resp = http.PostAsync(
                    $"https://messaging.twilio.com/v2/Channels/Senders/{senderSid}", content).Result;
                if (!resp.IsSuccessStatusCode)
                    log($"[Tunnel] Sender webhook update returned {(int)resp.StatusCode}.");
                return resp.IsSuccessStatusCode;
            }
            catch (Exception ex) { log($"[Tunnel] Webhook update error: {ex.Message}"); return false; }
        }

        // ===================== navigation =====================
        private void btnManage_Click(object sender, EventArgs e) { mainForm.showPanel(new DispatcherHomePanel()); }
        private void btnLogout_Click(object sender, EventArgs e) { mainForm.showPanel(new LoginPanel()); }
        private void btnRefresh_Click(object sender, EventArgs e) { loadTrips(); log("Queue refreshed (pull trips from Ride Control)."); }

        // ===================== helpers =====================
        // runs DB actions with the per-write "success" popups suppressed (the log summarizes instead)
        private void runQuiet(Action act)
        {
            SQL_CON.SuppressSuccessMessages = true;
            try { act(); }
            finally { SQL_CON.SuppressSuccessMessages = false; }
        }

        private bool runQuietBool(Func<bool> act)
        {
            SQL_CON.SuppressSuccessMessages = true;
            try { return act(); }
            finally { SQL_CON.SuppressSuccessMessages = false; }
        }

        private void warn(string msg) { MessageBox.Show(msg, "Notice", MessageBoxButtons.OK); }
        private void log(string msg) { txtLog.AppendText(DateTime.Now.ToString("HH:mm:ss") + "  " + msg + Environment.NewLine); }
    }
}
