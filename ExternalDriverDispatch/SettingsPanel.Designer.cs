namespace ExternalDriverDispatch
{
    partial class SettingsPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) { components.Dispose(); }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.label_title = new System.Windows.Forms.Label();
            this.lblMode = new System.Windows.Forms.Label();
            this.lblMaps = new System.Windows.Forms.Label();
            this.chkMaps = new System.Windows.Forms.CheckBox();
            this.lblMapsKey = new System.Windows.Forms.Label();
            this.txtMapsKey = new System.Windows.Forms.TextBox();
            this.lblAi = new System.Windows.Forms.Label();
            this.chkAi = new System.Windows.Forms.CheckBox();
            this.lblAiKey = new System.Windows.Forms.Label();
            this.txtAiKey = new System.Windows.Forms.TextBox();
            this.lblAiModel = new System.Windows.Forms.Label();
            this.txtAiModel = new System.Windows.Forms.TextBox();
            this.lblWa = new System.Windows.Forms.Label();
            this.lblProvider = new System.Windows.Forms.Label();
            this.cmbProvider = new System.Windows.Forms.ComboBox();
            this.chkWhatsApp = new System.Windows.Forms.CheckBox();
            this.lblWaToken = new System.Windows.Forms.Label();
            this.txtWaToken = new System.Windows.Forms.TextBox();
            this.lblWaPhone = new System.Windows.Forms.Label();
            this.txtWaPhone = new System.Windows.Forms.TextBox();
            this.lblTwSid = new System.Windows.Forms.Label();
            this.txtTwSid = new System.Windows.Forms.TextBox();
            this.lblTwToken = new System.Windows.Forms.Label();
            this.txtTwToken = new System.Windows.Forms.TextBox();
            this.lblTwFrom = new System.Windows.Forms.Label();
            this.txtTwFrom = new System.Windows.Forms.TextBox();
            this.lblTwContent = new System.Windows.Forms.Label();
            this.txtTwContent = new System.Windows.Forms.TextBox();
            this.lblWaNote = new System.Windows.Forms.Label();
            this.chkShowKeys = new System.Windows.Forms.CheckBox();
            this.button_save = new System.Windows.Forms.Button();
            this.button_back = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // label_title
            //
            this.label_title.AutoSize = true;
            this.label_title.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.label_title.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label_title.Location = new System.Drawing.Point(24, 18);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(430, 37);
            this.label_title.TabIndex = 0;
            this.label_title.Text = "Settings — external services";
            //
            // lblMode
            //
            this.lblMode.AutoSize = true;
            this.lblMode.Font = new System.Drawing.Font("Consolas", 10F);
            this.lblMode.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblMode.Location = new System.Drawing.Point(26, 58);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(60, 18);
            this.lblMode.TabIndex = 1;
            this.lblMode.Text = "mode";
            //
            // lblMaps
            //
            this.lblMaps.AutoSize = true;
            this.lblMaps.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblMaps.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblMaps.Location = new System.Drawing.Point(28, 95);
            this.lblMaps.Name = "lblMaps";
            this.lblMaps.Size = new System.Drawing.Size(170, 21);
            this.lblMaps.TabIndex = 2;
            this.lblMaps.Text = "① Google Maps";
            //
            // chkMaps
            //
            this.chkMaps.AutoSize = true;
            this.chkMaps.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkMaps.Location = new System.Drawing.Point(52, 123);
            this.chkMaps.Name = "chkMaps";
            this.chkMaps.Size = new System.Drawing.Size(280, 23);
            this.chkMaps.TabIndex = 3;
            this.chkMaps.Text = "Live  (off = offline fallback)";
            this.chkMaps.UseVisualStyleBackColor = true;
            //
            // lblMapsKey
            //
            this.lblMapsKey.AutoSize = true;
            this.lblMapsKey.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblMapsKey.Location = new System.Drawing.Point(52, 156);
            this.lblMapsKey.Name = "lblMapsKey";
            this.lblMapsKey.Size = new System.Drawing.Size(58, 19);
            this.lblMapsKey.TabIndex = 4;
            this.lblMapsKey.Text = "API key:";
            //
            // txtMapsKey
            //
            this.txtMapsKey.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMapsKey.Location = new System.Drawing.Point(150, 153);
            this.txtMapsKey.Name = "txtMapsKey";
            this.txtMapsKey.Size = new System.Drawing.Size(390, 27);
            this.txtMapsKey.TabIndex = 5;
            this.txtMapsKey.UseSystemPasswordChar = true;
            //
            // lblAi
            //
            this.lblAi.AutoSize = true;
            this.lblAi.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblAi.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblAi.Location = new System.Drawing.Point(28, 205);
            this.lblAi.Name = "lblAi";
            this.lblAi.Size = new System.Drawing.Size(130, 21);
            this.lblAi.TabIndex = 6;
            this.lblAi.Text = "② Claude AI";
            //
            // chkAi
            //
            this.chkAi.AutoSize = true;
            this.chkAi.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkAi.Location = new System.Drawing.Point(52, 233);
            this.chkAi.Name = "chkAi";
            this.chkAi.Size = new System.Drawing.Size(280, 23);
            this.chkAi.TabIndex = 7;
            this.chkAi.Text = "Live  (off = offline fallback)";
            this.chkAi.UseVisualStyleBackColor = true;
            //
            // lblAiKey
            //
            this.lblAiKey.AutoSize = true;
            this.lblAiKey.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblAiKey.Location = new System.Drawing.Point(52, 266);
            this.lblAiKey.Name = "lblAiKey";
            this.lblAiKey.Size = new System.Drawing.Size(58, 19);
            this.lblAiKey.TabIndex = 8;
            this.lblAiKey.Text = "API key:";
            //
            // txtAiKey
            //
            this.txtAiKey.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtAiKey.Location = new System.Drawing.Point(150, 263);
            this.txtAiKey.Name = "txtAiKey";
            this.txtAiKey.Size = new System.Drawing.Size(390, 27);
            this.txtAiKey.TabIndex = 9;
            this.txtAiKey.UseSystemPasswordChar = true;
            //
            // lblAiModel
            //
            this.lblAiModel.AutoSize = true;
            this.lblAiModel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblAiModel.Location = new System.Drawing.Point(52, 302);
            this.lblAiModel.Name = "lblAiModel";
            this.lblAiModel.Size = new System.Drawing.Size(52, 19);
            this.lblAiModel.TabIndex = 10;
            this.lblAiModel.Text = "Model:";
            //
            // txtAiModel
            //
            this.txtAiModel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtAiModel.Location = new System.Drawing.Point(150, 299);
            this.txtAiModel.Name = "txtAiModel";
            this.txtAiModel.Size = new System.Drawing.Size(300, 27);
            this.txtAiModel.TabIndex = 11;
            //
            // lblWa
            //
            this.lblWa.AutoSize = true;
            this.lblWa.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblWa.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblWa.Location = new System.Drawing.Point(580, 95);
            this.lblWa.Name = "lblWa";
            this.lblWa.Size = new System.Drawing.Size(150, 21);
            this.lblWa.TabIndex = 12;
            this.lblWa.Text = "③ WhatsApp";
            //
            // lblProvider
            //
            this.lblProvider.AutoSize = true;
            this.lblProvider.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblProvider.Location = new System.Drawing.Point(604, 128);
            this.lblProvider.Name = "lblProvider";
            this.lblProvider.Size = new System.Drawing.Size(66, 19);
            this.lblProvider.TabIndex = 13;
            this.lblProvider.Text = "Provider:";
            //
            // cmbProvider
            //
            this.cmbProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProvider.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbProvider.Location = new System.Drawing.Point(700, 125);
            this.cmbProvider.Name = "cmbProvider";
            this.cmbProvider.Size = new System.Drawing.Size(160, 27);
            this.cmbProvider.TabIndex = 14;
            this.cmbProvider.SelectedIndexChanged += new System.EventHandler(this.cmbProvider_SelectedIndexChanged);
            //
            // chkWhatsApp
            //
            this.chkWhatsApp.AutoSize = true;
            this.chkWhatsApp.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkWhatsApp.Location = new System.Drawing.Point(604, 160);
            this.chkWhatsApp.Name = "chkWhatsApp";
            this.chkWhatsApp.Size = new System.Drawing.Size(280, 23);
            this.chkWhatsApp.TabIndex = 15;
            this.chkWhatsApp.Text = "Live  (off = offline fallback)";
            this.chkWhatsApp.UseVisualStyleBackColor = true;
            //
            // lblWaToken  (Meta)
            //
            this.lblWaToken.AutoSize = true;
            this.lblWaToken.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblWaToken.Location = new System.Drawing.Point(604, 198);
            this.lblWaToken.Name = "lblWaToken";
            this.lblWaToken.Size = new System.Drawing.Size(86, 19);
            this.lblWaToken.TabIndex = 16;
            this.lblWaToken.Text = "Meta token:";
            //
            // txtWaToken  (Meta)
            //
            this.txtWaToken.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtWaToken.Location = new System.Drawing.Point(730, 195);
            this.txtWaToken.Name = "txtWaToken";
            this.txtWaToken.Size = new System.Drawing.Size(390, 27);
            this.txtWaToken.TabIndex = 17;
            this.txtWaToken.UseSystemPasswordChar = true;
            //
            // lblWaPhone  (Meta)
            //
            this.lblWaPhone.AutoSize = true;
            this.lblWaPhone.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblWaPhone.Location = new System.Drawing.Point(604, 234);
            this.lblWaPhone.Name = "lblWaPhone";
            this.lblWaPhone.Size = new System.Drawing.Size(110, 19);
            this.lblWaPhone.TabIndex = 18;
            this.lblWaPhone.Text = "Meta phone id:";
            //
            // txtWaPhone  (Meta)
            //
            this.txtWaPhone.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtWaPhone.Location = new System.Drawing.Point(730, 231);
            this.txtWaPhone.Name = "txtWaPhone";
            this.txtWaPhone.Size = new System.Drawing.Size(390, 27);
            this.txtWaPhone.TabIndex = 19;
            //
            // lblTwSid  (Twilio)
            //
            this.lblTwSid.AutoSize = true;
            this.lblTwSid.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTwSid.Location = new System.Drawing.Point(604, 198);
            this.lblTwSid.Name = "lblTwSid";
            this.lblTwSid.Size = new System.Drawing.Size(90, 19);
            this.lblTwSid.TabIndex = 20;
            this.lblTwSid.Text = "Account SID:";
            //
            // txtTwSid  (Twilio)
            //
            this.txtTwSid.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTwSid.Location = new System.Drawing.Point(730, 195);
            this.txtTwSid.Name = "txtTwSid";
            this.txtTwSid.Size = new System.Drawing.Size(390, 27);
            this.txtTwSid.TabIndex = 21;
            //
            // lblTwToken  (Twilio)
            //
            this.lblTwToken.AutoSize = true;
            this.lblTwToken.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTwToken.Location = new System.Drawing.Point(604, 234);
            this.lblTwToken.Name = "lblTwToken";
            this.lblTwToken.Size = new System.Drawing.Size(80, 19);
            this.lblTwToken.TabIndex = 22;
            this.lblTwToken.Text = "Auth token:";
            //
            // txtTwToken  (Twilio)
            //
            this.txtTwToken.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTwToken.Location = new System.Drawing.Point(730, 231);
            this.txtTwToken.Name = "txtTwToken";
            this.txtTwToken.Size = new System.Drawing.Size(390, 27);
            this.txtTwToken.TabIndex = 23;
            this.txtTwToken.UseSystemPasswordChar = true;
            //
            // lblTwFrom  (Twilio)
            //
            this.lblTwFrom.AutoSize = true;
            this.lblTwFrom.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTwFrom.Location = new System.Drawing.Point(604, 270);
            this.lblTwFrom.Name = "lblTwFrom";
            this.lblTwFrom.Size = new System.Drawing.Size(48, 19);
            this.lblTwFrom.TabIndex = 24;
            this.lblTwFrom.Text = "From:";
            //
            // txtTwFrom  (Twilio)
            //
            this.txtTwFrom.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTwFrom.Location = new System.Drawing.Point(730, 267);
            this.txtTwFrom.Name = "txtTwFrom";
            this.txtTwFrom.Size = new System.Drawing.Size(390, 27);
            this.txtTwFrom.TabIndex = 25;
            //
            // lblTwContent  (Twilio)
            //
            this.lblTwContent.AutoSize = true;
            this.lblTwContent.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTwContent.Location = new System.Drawing.Point(604, 306);
            this.lblTwContent.Name = "lblTwContent";
            this.lblTwContent.Size = new System.Drawing.Size(90, 19);
            this.lblTwContent.TabIndex = 30;
            this.lblTwContent.Text = "Template SID:";
            //
            // txtTwContent  (Twilio)
            //
            this.txtTwContent.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTwContent.Location = new System.Drawing.Point(730, 303);
            this.txtTwContent.Name = "txtTwContent";
            this.txtTwContent.Size = new System.Drawing.Size(390, 27);
            this.txtTwContent.TabIndex = 31;
            //
            // lblWaNote
            //
            this.lblWaNote.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblWaNote.ForeColor = System.Drawing.Color.Gray;
            this.lblWaNote.Location = new System.Drawing.Point(604, 345);
            this.lblWaNote.Name = "lblWaNote";
            this.lblWaNote.Size = new System.Drawing.Size(516, 60);
            this.lblWaNote.TabIndex = 26;
            this.lblWaNote.Text = "Twilio: Account SID + Auth Token from the Twilio Console; From is your WhatsApp sender " +
                "(e.g. the sandbox whatsapp:+14155238886). Business-initiated offers require an approved template.";
            //
            // chkShowKeys
            //
            this.chkShowKeys.AutoSize = true;
            this.chkShowKeys.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.chkShowKeys.Location = new System.Drawing.Point(52, 360);
            this.chkShowKeys.Name = "chkShowKeys";
            this.chkShowKeys.Size = new System.Drawing.Size(160, 21);
            this.chkShowKeys.TabIndex = 27;
            this.chkShowKeys.Text = "Show keys / tokens";
            this.chkShowKeys.UseVisualStyleBackColor = true;
            this.chkShowKeys.CheckedChanged += new System.EventHandler(this.chkShowKeys_CheckedChanged);
            //
            // button_save
            //
            this.button_save.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_save.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.button_save.ForeColor = System.Drawing.Color.DarkGreen;
            this.button_save.Location = new System.Drawing.Point(28, 600);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(180, 44);
            this.button_save.TabIndex = 28;
            this.button_save.Text = "Save settings";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            //
            // button_back
            //
            this.button_back.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.button_back.Location = new System.Drawing.Point(1010, 600);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(110, 44);
            this.button_back.TabIndex = 29;
            this.button_back.Text = "← Back";
            this.button_back.UseVisualStyleBackColor = true;
            this.button_back.Click += new System.EventHandler(this.button_back_Click);
            //
            // SettingsPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button_back);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.chkShowKeys);
            this.Controls.Add(this.lblWaNote);
            this.Controls.Add(this.txtTwContent);
            this.Controls.Add(this.lblTwContent);
            this.Controls.Add(this.txtTwFrom);
            this.Controls.Add(this.lblTwFrom);
            this.Controls.Add(this.txtTwToken);
            this.Controls.Add(this.lblTwToken);
            this.Controls.Add(this.txtTwSid);
            this.Controls.Add(this.lblTwSid);
            this.Controls.Add(this.txtWaPhone);
            this.Controls.Add(this.lblWaPhone);
            this.Controls.Add(this.txtWaToken);
            this.Controls.Add(this.lblWaToken);
            this.Controls.Add(this.chkWhatsApp);
            this.Controls.Add(this.cmbProvider);
            this.Controls.Add(this.lblProvider);
            this.Controls.Add(this.lblWa);
            this.Controls.Add(this.txtAiModel);
            this.Controls.Add(this.lblAiModel);
            this.Controls.Add(this.txtAiKey);
            this.Controls.Add(this.lblAiKey);
            this.Controls.Add(this.chkAi);
            this.Controls.Add(this.lblAi);
            this.Controls.Add(this.txtMapsKey);
            this.Controls.Add(this.lblMapsKey);
            this.Controls.Add(this.chkMaps);
            this.Controls.Add(this.lblMaps);
            this.Controls.Add(this.lblMode);
            this.Controls.Add(this.label_title);
            this.Name = "SettingsPanel";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Size = new System.Drawing.Size(1150, 680);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label lblMaps;
        private System.Windows.Forms.CheckBox chkMaps;
        private System.Windows.Forms.Label lblMapsKey;
        private System.Windows.Forms.TextBox txtMapsKey;
        private System.Windows.Forms.Label lblAi;
        private System.Windows.Forms.CheckBox chkAi;
        private System.Windows.Forms.Label lblAiKey;
        private System.Windows.Forms.TextBox txtAiKey;
        private System.Windows.Forms.Label lblAiModel;
        private System.Windows.Forms.TextBox txtAiModel;
        private System.Windows.Forms.Label lblWa;
        private System.Windows.Forms.Label lblProvider;
        private System.Windows.Forms.ComboBox cmbProvider;
        private System.Windows.Forms.CheckBox chkWhatsApp;
        private System.Windows.Forms.Label lblWaToken;
        private System.Windows.Forms.TextBox txtWaToken;
        private System.Windows.Forms.Label lblWaPhone;
        private System.Windows.Forms.TextBox txtWaPhone;
        private System.Windows.Forms.Label lblTwSid;
        private System.Windows.Forms.TextBox txtTwSid;
        private System.Windows.Forms.Label lblTwToken;
        private System.Windows.Forms.TextBox txtTwToken;
        private System.Windows.Forms.Label lblTwFrom;
        private System.Windows.Forms.TextBox txtTwFrom;
        private System.Windows.Forms.Label lblTwContent;
        private System.Windows.Forms.TextBox txtTwContent;
        private System.Windows.Forms.Label lblWaNote;
        private System.Windows.Forms.CheckBox chkShowKeys;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_back;
    }
}
