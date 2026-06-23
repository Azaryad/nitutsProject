using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// Settings screen — edits the three external services' configuration and writes it back to
    /// app.config via <see cref="Config.Save"/>. Offline/live is PER service (each "Live" checkbox;
    /// off = offline fallback). WhatsApp has a provider dropdown (Meta / Twilio) that swaps the
    /// relevant credential fields.
    ///
    /// This is a technical / NFR screen (like LoginPanel): NOT a use case or a domain entity, and
    /// must not appear in the class or UC diagrams. Saved values take effect the next time the
    /// Dispatch Board is opened (the board rebuilds its services from Config each load).
    /// </summary>
    public partial class SettingsPanel : UserControl
    {
        public SettingsPanel()
        {
            InitializeComponent();
            UiTheme.Apply(this);
            cmbProvider.Items.AddRange(new object[] { "Meta", "Twilio" });
            loadFromConfig();
        }

        private void loadFromConfig()
        {
            chkMaps.Checked = Config.MapsEnabled;
            txtMapsKey.Text = Config.MapsApiKey;

            chkAi.Checked = Config.AiEnabled;
            txtAiKey.Text = Config.AiApiKey;
            txtAiModel.Text = Config.AiModel;

            chkWhatsApp.Checked = Config.WhatsAppEnabled;
            cmbProvider.SelectedIndex = Config.WhatsAppProvider == "twilio" ? 1 : 0;
            txtWaToken.Text = Config.WhatsAppToken;
            txtWaPhone.Text = Config.WhatsAppPhoneNumberId;
            txtTwSid.Text = Config.TwilioAccountSid;
            txtTwToken.Text = Config.TwilioAuthToken;
            txtTwFrom.Text = Config.TwilioWhatsAppFrom;
            txtTwContent.Text = Config.TwilioContentSid;

            updateProviderVisibility();
            refreshMode();
        }

        private void refreshMode()
        {
            lblMode.Text = ServiceFactory.ModeSummary();
        }

        // show the Meta credential fields or the Twilio ones, per the dropdown
        private void updateProviderVisibility()
        {
            bool twilio = cmbProvider.SelectedIndex == 1;
            lblWaToken.Visible = txtWaToken.Visible = !twilio;
            lblWaPhone.Visible = txtWaPhone.Visible = !twilio;
            lblTwSid.Visible = txtTwSid.Visible = twilio;
            lblTwToken.Visible = txtTwToken.Visible = twilio;
            lblTwFrom.Visible = txtTwFrom.Visible = twilio;
            lblTwContent.Visible = txtTwContent.Visible = twilio;
        }

        private void cmbProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateProviderVisibility();
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            string provider = cmbProvider.SelectedIndex == 1 ? "twilio" : "meta";
            var values = new Dictionary<string, string>
            {
                { "Maps.Enabled",           Lower(chkMaps.Checked) },
                { "Maps.ApiKey",            txtMapsKey.Text.Trim() },
                { "Ai.Enabled",             Lower(chkAi.Checked) },
                { "Ai.ApiKey",              txtAiKey.Text.Trim() },
                { "Ai.Model",               txtAiModel.Text.Trim() },
                { "WhatsApp.Enabled",       Lower(chkWhatsApp.Checked) },
                { "WhatsApp.Provider",      provider },
                { "WhatsApp.Token",         txtWaToken.Text.Trim() },
                { "WhatsApp.PhoneNumberId", txtWaPhone.Text.Trim() },
                { "Twilio.AccountSid",      txtTwSid.Text.Trim() },
                { "Twilio.AuthToken",       txtTwToken.Text.Trim() },
                { "Twilio.WhatsAppFrom",    txtTwFrom.Text.Trim() },
                { "Twilio.ContentSid",      txtTwContent.Text.Trim() }
            };

            try
            {
                Config.Save(values);
                refreshMode();
                MessageBox.Show(
                    "Settings saved. Changes take effect the next time you open the Dispatch Board.\n\n" +
                    ServiceFactory.ModeSummary(),
                    "Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not save settings: " + ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        // toggle masking on the secret fields (keys + auth tokens)
        private void chkShowKeys_CheckedChanged(object sender, EventArgs e)
        {
            bool show = chkShowKeys.Checked;
            txtMapsKey.UseSystemPasswordChar = !show;
            txtAiKey.UseSystemPasswordChar = !show;
            txtWaToken.UseSystemPasswordChar = !show;
            txtTwToken.UseSystemPasswordChar = !show;
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new DispatcherHomePanel());
        }

        private static string Lower(bool b) => b.ToString().ToLowerInvariant();
    }
}
