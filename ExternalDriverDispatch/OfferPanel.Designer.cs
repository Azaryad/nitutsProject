namespace ExternalDriverDispatch
{
    partial class OfferPanel
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label_id = new System.Windows.Forms.Label();
            this.textBox_id = new System.Windows.Forms.TextBox();
            this.label_trip = new System.Windows.Forms.Label();
            this.comboBox_trip = new System.Windows.Forms.ComboBox();
            this.label_driver = new System.Windows.Forms.Label();
            this.comboBox_driver = new System.Windows.Forms.ComboBox();
            this.label_status = new System.Windows.Forms.Label();
            this.comboBox_status = new System.Windows.Forms.ComboBox();
            this.label_rank = new System.Windows.Forms.Label();
            this.textBox_rank = new System.Windows.Forms.TextBox();
            this.label_sent = new System.Windows.Forms.Label();
            this.dateTimePicker_sent = new System.Windows.Forms.DateTimePicker();
            this.label_expires = new System.Windows.Forms.Label();
            this.dateTimePicker_expires = new System.Windows.Forms.DateTimePicker();
            this.label_reply = new System.Windows.Forms.Label();
            this.textBox_reply = new System.Windows.Forms.TextBox();
            this.label_ai = new System.Windows.Forms.Label();
            this.textBox_ai = new System.Windows.Forms.TextBox();
            this.button_add = new System.Windows.Forms.Button();
            this.button_update = new System.Windows.Forms.Button();
            this.button_delete = new System.Windows.Forms.Button();
            this.button_clear = new System.Windows.Forms.Button();
            this.button_back = new System.Windows.Forms.Button();
            this.button_accept = new System.Windows.Forms.Button();
            this.button_reject = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            //
            // label_title
            //
            this.label_title.AutoSize = true;
            this.label_title.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.label_title.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label_title.Location = new System.Drawing.Point(20, 12);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(90, 40);
            this.label_title.TabIndex = 0;
            this.label_title.Text = "Offers";
            //
            // dataGridView1
            //
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(20, 58);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1100, 180);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            //
            // label_id
            //
            this.label_id.AutoSize = true;
            this.label_id.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_id.Location = new System.Drawing.Point(20, 262);
            this.label_id.Name = "label_id";
            this.label_id.Size = new System.Drawing.Size(23, 20);
            this.label_id.TabIndex = 2;
            this.label_id.Text = "ID";
            //
            // textBox_id
            //
            this.textBox_id.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.textBox_id.Location = new System.Drawing.Point(150, 260);
            this.textBox_id.Name = "textBox_id";
            this.textBox_id.ReadOnly = true;
            this.textBox_id.Size = new System.Drawing.Size(200, 27);
            this.textBox_id.TabIndex = 3;
            //
            // label_trip
            //
            this.label_trip.AutoSize = true;
            this.label_trip.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_trip.Location = new System.Drawing.Point(20, 302);
            this.label_trip.Name = "label_trip";
            this.label_trip.Size = new System.Drawing.Size(33, 20);
            this.label_trip.TabIndex = 4;
            this.label_trip.Text = "Trip";
            //
            // comboBox_trip
            //
            this.comboBox_trip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_trip.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.comboBox_trip.Location = new System.Drawing.Point(150, 300);
            this.comboBox_trip.Name = "comboBox_trip";
            this.comboBox_trip.Size = new System.Drawing.Size(200, 28);
            this.comboBox_trip.TabIndex = 5;
            //
            // label_driver
            //
            this.label_driver.AutoSize = true;
            this.label_driver.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_driver.Location = new System.Drawing.Point(20, 342);
            this.label_driver.Name = "label_driver";
            this.label_driver.Size = new System.Drawing.Size(48, 20);
            this.label_driver.TabIndex = 6;
            this.label_driver.Text = "Driver";
            //
            // comboBox_driver
            //
            this.comboBox_driver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_driver.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.comboBox_driver.Location = new System.Drawing.Point(150, 340);
            this.comboBox_driver.Name = "comboBox_driver";
            this.comboBox_driver.Size = new System.Drawing.Size(200, 28);
            this.comboBox_driver.TabIndex = 7;
            //
            // label_status
            //
            this.label_status.AutoSize = true;
            this.label_status.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_status.Location = new System.Drawing.Point(20, 382);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(50, 20);
            this.label_status.TabIndex = 8;
            this.label_status.Text = "Status";
            //
            // comboBox_status
            //
            this.comboBox_status.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_status.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.comboBox_status.Location = new System.Drawing.Point(150, 380);
            this.comboBox_status.Name = "comboBox_status";
            this.comboBox_status.Size = new System.Drawing.Size(200, 28);
            this.comboBox_status.TabIndex = 9;
            //
            // label_rank
            //
            this.label_rank.AutoSize = true;
            this.label_rank.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_rank.Location = new System.Drawing.Point(20, 422);
            this.label_rank.Name = "label_rank";
            this.label_rank.Size = new System.Drawing.Size(40, 20);
            this.label_rank.TabIndex = 10;
            this.label_rank.Text = "Rank";
            //
            // textBox_rank
            //
            this.textBox_rank.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.textBox_rank.Location = new System.Drawing.Point(150, 420);
            this.textBox_rank.Name = "textBox_rank";
            this.textBox_rank.Size = new System.Drawing.Size(200, 27);
            this.textBox_rank.TabIndex = 11;
            //
            // label_sent
            //
            this.label_sent.AutoSize = true;
            this.label_sent.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_sent.Location = new System.Drawing.Point(400, 262);
            this.label_sent.Name = "label_sent";
            this.label_sent.Size = new System.Drawing.Size(38, 20);
            this.label_sent.TabIndex = 12;
            this.label_sent.Text = "Sent";
            //
            // dateTimePicker_sent
            //
            this.dateTimePicker_sent.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dateTimePicker_sent.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.dateTimePicker_sent.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_sent.Location = new System.Drawing.Point(510, 260);
            this.dateTimePicker_sent.Name = "dateTimePicker_sent";
            this.dateTimePicker_sent.Size = new System.Drawing.Size(190, 27);
            this.dateTimePicker_sent.TabIndex = 13;
            //
            // label_expires
            //
            this.label_expires.AutoSize = true;
            this.label_expires.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_expires.Location = new System.Drawing.Point(400, 302);
            this.label_expires.Name = "label_expires";
            this.label_expires.Size = new System.Drawing.Size(58, 20);
            this.label_expires.TabIndex = 14;
            this.label_expires.Text = "Expires";
            //
            // dateTimePicker_expires
            //
            this.dateTimePicker_expires.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dateTimePicker_expires.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.dateTimePicker_expires.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_expires.Location = new System.Drawing.Point(510, 300);
            this.dateTimePicker_expires.Name = "dateTimePicker_expires";
            this.dateTimePicker_expires.Size = new System.Drawing.Size(190, 27);
            this.dateTimePicker_expires.TabIndex = 15;
            //
            // label_reply
            //
            this.label_reply.AutoSize = true;
            this.label_reply.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_reply.Location = new System.Drawing.Point(760, 258);
            this.label_reply.Name = "label_reply";
            this.label_reply.Size = new System.Drawing.Size(86, 20);
            this.label_reply.TabIndex = 16;
            this.label_reply.Text = "Driver reply";
            //
            // textBox_reply
            //
            this.textBox_reply.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.textBox_reply.Location = new System.Drawing.Point(760, 282);
            this.textBox_reply.Multiline = true;
            this.textBox_reply.Name = "textBox_reply";
            this.textBox_reply.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_reply.Size = new System.Drawing.Size(360, 60);
            this.textBox_reply.TabIndex = 17;
            //
            // label_ai
            //
            this.label_ai.AutoSize = true;
            this.label_ai.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_ai.Location = new System.Drawing.Point(760, 352);
            this.label_ai.Name = "label_ai";
            this.label_ai.Size = new System.Drawing.Size(115, 20);
            this.label_ai.TabIndex = 18;
            this.label_ai.Text = "AI interpretation";
            //
            // textBox_ai
            //
            this.textBox_ai.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.textBox_ai.Location = new System.Drawing.Point(760, 376);
            this.textBox_ai.Multiline = true;
            this.textBox_ai.Name = "textBox_ai";
            this.textBox_ai.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_ai.Size = new System.Drawing.Size(360, 60);
            this.textBox_ai.TabIndex = 19;
            //
            // button_add
            //
            this.button_add.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_add.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.button_add.Location = new System.Drawing.Point(20, 475);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(130, 42);
            this.button_add.TabIndex = 20;
            this.button_add.Text = "Add";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            //
            // button_update
            //
            this.button_update.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_update.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.button_update.Location = new System.Drawing.Point(160, 475);
            this.button_update.Name = "button_update";
            this.button_update.Size = new System.Drawing.Size(130, 42);
            this.button_update.TabIndex = 21;
            this.button_update.Text = "Update";
            this.button_update.UseVisualStyleBackColor = true;
            this.button_update.Click += new System.EventHandler(this.button_update_Click);
            //
            // button_delete
            //
            this.button_delete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_delete.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.button_delete.ForeColor = System.Drawing.Color.Firebrick;
            this.button_delete.Location = new System.Drawing.Point(300, 475);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(130, 42);
            this.button_delete.TabIndex = 22;
            this.button_delete.Text = "Delete";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            //
            // button_clear
            //
            this.button_clear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_clear.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.button_clear.Location = new System.Drawing.Point(440, 475);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(130, 42);
            this.button_clear.TabIndex = 23;
            this.button_clear.Text = "Clear";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            //
            // button_back
            //
            this.button_back.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.button_back.Location = new System.Drawing.Point(580, 475);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(130, 42);
            this.button_back.TabIndex = 24;
            this.button_back.Text = "← Back";
            this.button_back.UseVisualStyleBackColor = true;
            this.button_back.Click += new System.EventHandler(this.button_back_Click);
            //
            // button_accept
            //
            this.button_accept.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_accept.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.button_accept.ForeColor = System.Drawing.Color.DarkGreen;
            this.button_accept.Location = new System.Drawing.Point(20, 530);
            this.button_accept.Name = "button_accept";
            this.button_accept.Size = new System.Drawing.Size(260, 38);
            this.button_accept.TabIndex = 25;
            this.button_accept.Text = "Accept (→ trip Confirmed)";
            this.button_accept.UseVisualStyleBackColor = true;
            this.button_accept.Click += new System.EventHandler(this.button_accept_Click);
            //
            // button_reject
            //
            this.button_reject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_reject.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.button_reject.ForeColor = System.Drawing.Color.DarkOrange;
            this.button_reject.Location = new System.Drawing.Point(290, 530);
            this.button_reject.Name = "button_reject";
            this.button_reject.Size = new System.Drawing.Size(260, 38);
            this.button_reject.TabIndex = 26;
            this.button_reject.Text = "Reject (→ trip Open)";
            this.button_reject.UseVisualStyleBackColor = true;
            this.button_reject.Click += new System.EventHandler(this.button_reject_Click);
            //
            // OfferPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button_accept);
            this.Controls.Add(this.button_reject);
            this.Controls.Add(this.button_back);
            this.Controls.Add(this.button_clear);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.button_update);
            this.Controls.Add(this.button_add);
            this.Controls.Add(this.textBox_ai);
            this.Controls.Add(this.label_ai);
            this.Controls.Add(this.textBox_reply);
            this.Controls.Add(this.label_reply);
            this.Controls.Add(this.dateTimePicker_expires);
            this.Controls.Add(this.label_expires);
            this.Controls.Add(this.dateTimePicker_sent);
            this.Controls.Add(this.label_sent);
            this.Controls.Add(this.textBox_rank);
            this.Controls.Add(this.label_rank);
            this.Controls.Add(this.comboBox_status);
            this.Controls.Add(this.label_status);
            this.Controls.Add(this.comboBox_driver);
            this.Controls.Add(this.label_driver);
            this.Controls.Add(this.comboBox_trip);
            this.Controls.Add(this.label_trip);
            this.Controls.Add(this.textBox_id);
            this.Controls.Add(this.label_id);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label_title);
            this.Name = "OfferPanel";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Size = new System.Drawing.Size(1150, 680);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label_id;
        private System.Windows.Forms.TextBox textBox_id;
        private System.Windows.Forms.Label label_trip;
        private System.Windows.Forms.ComboBox comboBox_trip;
        private System.Windows.Forms.Label label_driver;
        private System.Windows.Forms.ComboBox comboBox_driver;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.ComboBox comboBox_status;
        private System.Windows.Forms.Label label_rank;
        private System.Windows.Forms.TextBox textBox_rank;
        private System.Windows.Forms.Label label_sent;
        private System.Windows.Forms.DateTimePicker dateTimePicker_sent;
        private System.Windows.Forms.Label label_expires;
        private System.Windows.Forms.DateTimePicker dateTimePicker_expires;
        private System.Windows.Forms.Label label_reply;
        private System.Windows.Forms.TextBox textBox_reply;
        private System.Windows.Forms.Label label_ai;
        private System.Windows.Forms.TextBox textBox_ai;
        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.Button button_update;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Button button_back;
        private System.Windows.Forms.Button button_accept;
        private System.Windows.Forms.Button button_reject;
    }
}
