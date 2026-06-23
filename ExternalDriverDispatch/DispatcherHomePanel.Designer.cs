namespace ExternalDriverDispatch
{
    partial class DispatcherHomePanel
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
            this.button_regions = new System.Windows.Forms.Button();
            this.button_drivers = new System.Windows.Forms.Button();
            this.button_trips = new System.Windows.Forms.Button();
            this.button_offers = new System.Windows.Forms.Button();
            this.button_messages = new System.Windows.Forms.Button();
            this.button_settings = new System.Windows.Forms.Button();
            this.button_report = new System.Windows.Forms.Button();
            this.button_logout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // label_title
            //
            this.label_title.AutoSize = true;
            this.label_title.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.label_title.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label_title.Location = new System.Drawing.Point(380, 40);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(360, 51);
            this.label_title.TabIndex = 0;
            this.label_title.Text = "Data Management";
            //
            // button_regions
            //
            this.button_regions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_regions.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.button_regions.Location = new System.Drawing.Point(330, 150);
            this.button_regions.Name = "button_regions";
            this.button_regions.Size = new System.Drawing.Size(220, 80);
            this.button_regions.TabIndex = 1;
            this.button_regions.Text = "Regions";
            this.button_regions.UseVisualStyleBackColor = true;
            this.button_regions.Click += new System.EventHandler(this.button_regions_Click);
            //
            // button_drivers
            //
            this.button_drivers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_drivers.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.button_drivers.Location = new System.Drawing.Point(580, 150);
            this.button_drivers.Name = "button_drivers";
            this.button_drivers.Size = new System.Drawing.Size(220, 80);
            this.button_drivers.TabIndex = 2;
            this.button_drivers.Text = "Drivers";
            this.button_drivers.UseVisualStyleBackColor = true;
            this.button_drivers.Click += new System.EventHandler(this.button_drivers_Click);
            //
            // button_trips
            //
            this.button_trips.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_trips.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.button_trips.Location = new System.Drawing.Point(330, 260);
            this.button_trips.Name = "button_trips";
            this.button_trips.Size = new System.Drawing.Size(220, 80);
            this.button_trips.TabIndex = 3;
            this.button_trips.Text = "Trips";
            this.button_trips.UseVisualStyleBackColor = true;
            this.button_trips.Click += new System.EventHandler(this.button_trips_Click);
            //
            // button_offers
            //
            this.button_offers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_offers.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.button_offers.Location = new System.Drawing.Point(580, 260);
            this.button_offers.Name = "button_offers";
            this.button_offers.Size = new System.Drawing.Size(220, 80);
            this.button_offers.TabIndex = 4;
            this.button_offers.Text = "Offers";
            this.button_offers.UseVisualStyleBackColor = true;
            this.button_offers.Click += new System.EventHandler(this.button_offers_Click);
            //
            // button_messages
            //
            this.button_messages.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_messages.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.button_messages.Location = new System.Drawing.Point(330, 370);
            this.button_messages.Name = "button_messages";
            this.button_messages.Size = new System.Drawing.Size(220, 80);
            this.button_messages.TabIndex = 5;
            this.button_messages.Text = "Messages";
            this.button_messages.UseVisualStyleBackColor = true;
            this.button_messages.Click += new System.EventHandler(this.button_messages_Click);
            //
            // button_settings
            //
            this.button_settings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_settings.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.button_settings.Location = new System.Drawing.Point(580, 370);
            this.button_settings.Name = "button_settings";
            this.button_settings.Size = new System.Drawing.Size(220, 80);
            this.button_settings.TabIndex = 6;
            this.button_settings.Text = "⚙ Settings";
            this.button_settings.UseVisualStyleBackColor = true;
            this.button_settings.Click += new System.EventHandler(this.button_settings_Click);
            //
            // button_report
            //
            this.button_report.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_report.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.button_report.ForeColor = System.Drawing.Color.DodgerBlue;
            this.button_report.Location = new System.Drawing.Point(330, 470);
            this.button_report.Name = "button_report";
            this.button_report.Size = new System.Drawing.Size(470, 70);
            this.button_report.TabIndex = 7;
            this.button_report.Text = "📊 Driver Performance Report";
            this.button_report.UseVisualStyleBackColor = true;
            this.button_report.Click += new System.EventHandler(this.button_report_Click);
            //
            // button_logout
            //
            this.button_logout.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.button_logout.Location = new System.Drawing.Point(420, 565);
            this.button_logout.Name = "button_logout";
            this.button_logout.Size = new System.Drawing.Size(290, 45);
            this.button_logout.TabIndex = 8;
            this.button_logout.Text = "← Back to Dispatch Board";
            this.button_logout.UseVisualStyleBackColor = true;
            this.button_logout.Click += new System.EventHandler(this.button_logout_Click);
            //
            // DispatcherHomePanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button_logout);
            this.Controls.Add(this.button_report);
            this.Controls.Add(this.button_settings);
            this.Controls.Add(this.button_messages);
            this.Controls.Add(this.button_offers);
            this.Controls.Add(this.button_trips);
            this.Controls.Add(this.button_drivers);
            this.Controls.Add(this.button_regions);
            this.Controls.Add(this.label_title);
            this.Name = "DispatcherHomePanel";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Size = new System.Drawing.Size(1150, 680);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.Button button_regions;
        private System.Windows.Forms.Button button_drivers;
        private System.Windows.Forms.Button button_trips;
        private System.Windows.Forms.Button button_offers;
        private System.Windows.Forms.Button button_messages;
        private System.Windows.Forms.Button button_settings;
        private System.Windows.Forms.Button button_report;
        private System.Windows.Forms.Button button_logout;
    }
}
