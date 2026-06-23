namespace ExternalDriverDispatch
{
    partial class DispatchBoardPanel
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnTunnel = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnManage = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.lblTripsHeader = new System.Windows.Forms.Label();
            this.dgvTrips = new System.Windows.Forms.DataGridView();
            this.lblTrip = new System.Windows.Forms.Label();
            this.lblRegion = new System.Windows.Forms.Label();
            this.comboRegion = new System.Windows.Forms.ComboBox();
            this.btnAssignRegion = new System.Windows.Forms.Button();
            this.lblDriversHeader = new System.Windows.Forms.Label();
            this.dgvDrivers = new System.Windows.Forms.DataGridView();
            this.btnSendOffer = new System.Windows.Forms.Button();
            this.lblOffersHeader = new System.Windows.Forms.Label();
            this.dgvOffers = new System.Windows.Forms.DataGridView();
            this.lblResp = new System.Windows.Forms.Label();
            this.btnWhatsappYes = new System.Windows.Forms.Button();
            this.btnApprove = new System.Windows.Forms.Button();
            this.btnDecline = new System.Windows.Forms.Button();
            this.btnTimeout = new System.Windows.Forms.Button();
            this.lblReply = new System.Windows.Forms.Label();
            this.txtReply = new System.Windows.Forms.TextBox();
            this.btnReceiveReply = new System.Windows.Forms.Button();
            this.lblLog = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrips)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDrivers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOffers)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblTitle.Location = new System.Drawing.Point(20, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(270, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Dispatch Board";
            //
            // btnTunnel
            //
            this.btnTunnel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTunnel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTunnel.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnTunnel.Location = new System.Drawing.Point(470, 16);
            this.btnTunnel.Name = "btnTunnel";
            this.btnTunnel.Size = new System.Drawing.Size(220, 34);
            this.btnTunnel.TabIndex = 25;
            this.btnTunnel.Text = "🌐 Start Tunnel";
            this.btnTunnel.UseVisualStyleBackColor = true;
            this.btnTunnel.Click += new System.EventHandler(this.btnTunnel_Click);
            //
            // btnRefresh
            //
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(700, 16);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(140, 34);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "⟳ Pull trips";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // btnManage
            //
            this.btnManage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnManage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnManage.Location = new System.Drawing.Point(850, 16);
            this.btnManage.Name = "btnManage";
            this.btnManage.Size = new System.Drawing.Size(160, 34);
            this.btnManage.TabIndex = 2;
            this.btnManage.Text = "⚙ Data management";
            this.btnManage.UseVisualStyleBackColor = true;
            this.btnManage.Click += new System.EventHandler(this.btnManage_Click);
            //
            // btnLogout
            //
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnLogout.Location = new System.Drawing.Point(1020, 16);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(110, 34);
            this.btnLogout.TabIndex = 3;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            //
            // lblTripsHeader
            //
            this.lblTripsHeader.AutoSize = true;
            this.lblTripsHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTripsHeader.Location = new System.Drawing.Point(20, 62);
            this.lblTripsHeader.Name = "lblTripsHeader";
            this.lblTripsHeader.Size = new System.Drawing.Size(200, 21);
            this.lblTripsHeader.TabIndex = 4;
            this.lblTripsHeader.Text = "① Trips queue (active)";
            //
            // dgvTrips
            //
            this.dgvTrips.AllowUserToAddRows = false;
            this.dgvTrips.AllowUserToDeleteRows = false;
            this.dgvTrips.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTrips.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTrips.Location = new System.Drawing.Point(20, 90);
            this.dgvTrips.MultiSelect = false;
            this.dgvTrips.Name = "dgvTrips";
            this.dgvTrips.ReadOnly = true;
            this.dgvTrips.RowHeadersVisible = false;
            this.dgvTrips.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTrips.Size = new System.Drawing.Size(450, 560);
            this.dgvTrips.TabIndex = 5;
            this.dgvTrips.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTrips_CellClick);
            //
            // lblTrip
            //
            this.lblTrip.AutoSize = true;
            this.lblTrip.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTrip.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblTrip.Location = new System.Drawing.Point(485, 62);
            this.lblTrip.Name = "lblTrip";
            this.lblTrip.Size = new System.Drawing.Size(160, 21);
            this.lblTrip.TabIndex = 6;
            this.lblTrip.Text = "② Select a trip";
            //
            // lblRegion
            //
            this.lblRegion.AutoSize = true;
            this.lblRegion.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblRegion.Location = new System.Drawing.Point(485, 98);
            this.lblRegion.Name = "lblRegion";
            this.lblRegion.Size = new System.Drawing.Size(56, 20);
            this.lblRegion.TabIndex = 7;
            this.lblRegion.Text = "Region:";
            //
            // comboRegion
            //
            this.comboRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRegion.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.comboRegion.Location = new System.Drawing.Point(545, 95);
            this.comboRegion.Name = "comboRegion";
            this.comboRegion.Size = new System.Drawing.Size(165, 28);
            this.comboRegion.TabIndex = 8;
            //
            // btnAssignRegion
            //
            this.btnAssignRegion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAssignRegion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAssignRegion.Location = new System.Drawing.Point(485, 128);
            this.btnAssignRegion.Name = "btnAssignRegion";
            this.btnAssignRegion.Size = new System.Drawing.Size(225, 32);
            this.btnAssignRegion.TabIndex = 9;
            this.btnAssignRegion.Text = "Assign to region";
            this.btnAssignRegion.UseVisualStyleBackColor = true;
            this.btnAssignRegion.Click += new System.EventHandler(this.btnAssignRegion_Click);
            //
            // lblDriversHeader
            //
            this.lblDriversHeader.AutoSize = true;
            this.lblDriversHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblDriversHeader.Location = new System.Drawing.Point(485, 175);
            this.lblDriversHeader.Name = "lblDriversHeader";
            this.lblDriversHeader.Size = new System.Drawing.Size(190, 21);
            this.lblDriversHeader.TabIndex = 10;
            this.lblDriversHeader.Text = "③ Ranked drivers";
            //
            // dgvDrivers
            //
            this.dgvDrivers.AllowUserToAddRows = false;
            this.dgvDrivers.AllowUserToDeleteRows = false;
            this.dgvDrivers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDrivers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDrivers.Location = new System.Drawing.Point(485, 205);
            this.dgvDrivers.MultiSelect = false;
            this.dgvDrivers.Name = "dgvDrivers";
            this.dgvDrivers.ReadOnly = true;
            this.dgvDrivers.RowHeadersVisible = false;
            this.dgvDrivers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDrivers.Size = new System.Drawing.Size(275, 360);
            this.dgvDrivers.TabIndex = 11;
            //
            // btnSendOffer
            //
            this.btnSendOffer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSendOffer.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSendOffer.ForeColor = System.Drawing.Color.DarkGreen;
            this.btnSendOffer.Location = new System.Drawing.Point(485, 575);
            this.btnSendOffer.Name = "btnSendOffer";
            this.btnSendOffer.Size = new System.Drawing.Size(275, 48);
            this.btnSendOffer.TabIndex = 12;
            this.btnSendOffer.Text = "④ Send offer →";
            this.btnSendOffer.UseVisualStyleBackColor = true;
            this.btnSendOffer.Click += new System.EventHandler(this.btnSendOffer_Click);
            //
            // lblOffersHeader
            //
            this.lblOffersHeader.AutoSize = true;
            this.lblOffersHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblOffersHeader.Location = new System.Drawing.Point(775, 62);
            this.lblOffersHeader.Name = "lblOffersHeader";
            this.lblOffersHeader.Size = new System.Drawing.Size(150, 21);
            this.lblOffersHeader.TabIndex = 13;
            this.lblOffersHeader.Text = "⑤ Offers";
            //
            // dgvOffers
            //
            this.dgvOffers.AllowUserToAddRows = false;
            this.dgvOffers.AllowUserToDeleteRows = false;
            this.dgvOffers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOffers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOffers.Location = new System.Drawing.Point(775, 90);
            this.dgvOffers.MultiSelect = false;
            this.dgvOffers.Name = "dgvOffers";
            this.dgvOffers.ReadOnly = true;
            this.dgvOffers.RowHeadersVisible = false;
            this.dgvOffers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOffers.Size = new System.Drawing.Size(355, 150);
            this.dgvOffers.TabIndex = 14;
            //
            // lblResp
            //
            this.lblResp.AutoSize = true;
            this.lblResp.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblResp.Location = new System.Drawing.Point(775, 250);
            this.lblResp.Name = "lblResp";
            this.lblResp.Size = new System.Drawing.Size(230, 20);
            this.lblResp.TabIndex = 15;
            this.lblResp.Text = "⑥ Driver response (simulate):";
            //
            // btnWhatsappYes
            //
            this.btnWhatsappYes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWhatsappYes.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnWhatsappYes.Location = new System.Drawing.Point(775, 278);
            this.btnWhatsappYes.Name = "btnWhatsappYes";
            this.btnWhatsappYes.Size = new System.Drawing.Size(172, 36);
            this.btnWhatsappYes.TabIndex = 16;
            this.btnWhatsappYes.Text = "WhatsApp: Yes (intent)";
            this.btnWhatsappYes.UseVisualStyleBackColor = true;
            this.btnWhatsappYes.Click += new System.EventHandler(this.btnWhatsappYes_Click);
            //
            // btnApprove
            //
            this.btnApprove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApprove.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnApprove.ForeColor = System.Drawing.Color.DarkGreen;
            this.btnApprove.Location = new System.Drawing.Point(955, 278);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(175, 36);
            this.btnApprove.TabIndex = 17;
            this.btnApprove.Text = "Approve link ✓";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            //
            // btnDecline
            //
            this.btnDecline.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDecline.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnDecline.ForeColor = System.Drawing.Color.Firebrick;
            this.btnDecline.Location = new System.Drawing.Point(775, 320);
            this.btnDecline.Name = "btnDecline";
            this.btnDecline.Size = new System.Drawing.Size(172, 36);
            this.btnDecline.TabIndex = 18;
            this.btnDecline.Text = "Decline ✗";
            this.btnDecline.UseVisualStyleBackColor = true;
            this.btnDecline.Click += new System.EventHandler(this.btnDecline_Click);
            //
            // btnTimeout
            //
            this.btnTimeout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTimeout.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnTimeout.ForeColor = System.Drawing.Color.DarkOrange;
            this.btnTimeout.Location = new System.Drawing.Point(955, 320);
            this.btnTimeout.Name = "btnTimeout";
            this.btnTimeout.Size = new System.Drawing.Size(175, 36);
            this.btnTimeout.TabIndex = 19;
            this.btnTimeout.Text = "Timeout ⌛";
            this.btnTimeout.UseVisualStyleBackColor = true;
            this.btnTimeout.Click += new System.EventHandler(this.btnTimeout_Click);
            //
            // lblReply
            //
            this.lblReply.AutoSize = true;
            this.lblReply.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblReply.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblReply.Location = new System.Drawing.Point(775, 364);
            this.lblReply.Name = "lblReply";
            this.lblReply.Size = new System.Drawing.Size(230, 17);
            this.lblReply.TabIndex = 22;
            this.lblReply.Text = "Free-text reply (WhatsApp ← AI interpret):";
            //
            // txtReply
            //
            this.txtReply.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtReply.Location = new System.Drawing.Point(775, 384);
            this.txtReply.Name = "txtReply";
            this.txtReply.Size = new System.Drawing.Size(250, 25);
            this.txtReply.TabIndex = 23;
            //
            // btnReceiveReply
            //
            this.btnReceiveReply.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReceiveReply.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnReceiveReply.Location = new System.Drawing.Point(1030, 382);
            this.btnReceiveReply.Name = "btnReceiveReply";
            this.btnReceiveReply.Size = new System.Drawing.Size(100, 29);
            this.btnReceiveReply.TabIndex = 24;
            this.btnReceiveReply.Text = "Receive ←";
            this.btnReceiveReply.UseVisualStyleBackColor = true;
            this.btnReceiveReply.Click += new System.EventHandler(this.btnReceiveReply_Click);
            //
            // lblLog
            //
            this.lblLog.AutoSize = true;
            this.lblLog.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblLog.Location = new System.Drawing.Point(775, 420);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(90, 20);
            this.lblLog.TabIndex = 20;
            this.lblLog.Text = "Activity log";
            //
            // txtLog
            //
            this.txtLog.BackColor = System.Drawing.Color.FromArgb(248, 248, 248);
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.txtLog.Location = new System.Drawing.Point(775, 444);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(355, 206);
            this.txtLog.TabIndex = 21;
            //
            // DispatchBoardPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.btnReceiveReply);
            this.Controls.Add(this.txtReply);
            this.Controls.Add(this.lblReply);
            this.Controls.Add(this.btnTimeout);
            this.Controls.Add(this.btnDecline);
            this.Controls.Add(this.btnApprove);
            this.Controls.Add(this.btnWhatsappYes);
            this.Controls.Add(this.lblResp);
            this.Controls.Add(this.dgvOffers);
            this.Controls.Add(this.lblOffersHeader);
            this.Controls.Add(this.btnSendOffer);
            this.Controls.Add(this.dgvDrivers);
            this.Controls.Add(this.lblDriversHeader);
            this.Controls.Add(this.btnAssignRegion);
            this.Controls.Add(this.comboRegion);
            this.Controls.Add(this.lblRegion);
            this.Controls.Add(this.lblTrip);
            this.Controls.Add(this.dgvTrips);
            this.Controls.Add(this.lblTripsHeader);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnManage);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnTunnel);
            this.Controls.Add(this.lblTitle);
            this.Name = "DispatchBoardPanel";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Size = new System.Drawing.Size(1150, 680);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrips)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDrivers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOffers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnTunnel;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnManage;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label lblTripsHeader;
        private System.Windows.Forms.DataGridView dgvTrips;
        private System.Windows.Forms.Label lblTrip;
        private System.Windows.Forms.Label lblRegion;
        private System.Windows.Forms.ComboBox comboRegion;
        private System.Windows.Forms.Button btnAssignRegion;
        private System.Windows.Forms.Label lblDriversHeader;
        private System.Windows.Forms.DataGridView dgvDrivers;
        private System.Windows.Forms.Button btnSendOffer;
        private System.Windows.Forms.Label lblOffersHeader;
        private System.Windows.Forms.DataGridView dgvOffers;
        private System.Windows.Forms.Label lblResp;
        private System.Windows.Forms.Button btnWhatsappYes;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.Button btnDecline;
        private System.Windows.Forms.Button btnTimeout;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblReply;
        private System.Windows.Forms.TextBox txtReply;
        private System.Windows.Forms.Button btnReceiveReply;
    }
}
