namespace ExternalDriverDispatch
{
    partial class TripPanel
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
            this.label_bookingId = new System.Windows.Forms.Label();
            this.textBox_bookingId = new System.Windows.Forms.TextBox();
            this.label_region = new System.Windows.Forms.Label();
            this.comboBox_region = new System.Windows.Forms.ComboBox();
            this.label_status = new System.Windows.Forms.Label();
            this.comboBox_status = new System.Windows.Forms.ComboBox();
            this.label_pickupCity = new System.Windows.Forms.Label();
            this.textBox_pickupCity = new System.Windows.Forms.TextBox();
            this.label_pickupAddress = new System.Windows.Forms.Label();
            this.textBox_pickupAddress = new System.Windows.Forms.TextBox();
            this.label_dropoffCity = new System.Windows.Forms.Label();
            this.textBox_dropoffCity = new System.Windows.Forms.TextBox();
            this.label_dropoffAddress = new System.Windows.Forms.Label();
            this.textBox_dropoffAddress = new System.Windows.Forms.TextBox();
            this.label_pickup = new System.Windows.Forms.Label();
            this.dateTimePicker_pickup = new System.Windows.Forms.DateTimePicker();
            this.label_passengers = new System.Windows.Forms.Label();
            this.textBox_passengers = new System.Windows.Forms.TextBox();
            this.label_vehicleType = new System.Windows.Forms.Label();
            this.comboBox_vehicleType = new System.Windows.Forms.ComboBox();
            this.label_price = new System.Windows.Forms.Label();
            this.textBox_price = new System.Windows.Forms.TextBox();
            this.button_add = new System.Windows.Forms.Button();
            this.button_update = new System.Windows.Forms.Button();
            this.button_delete = new System.Windows.Forms.Button();
            this.button_clear = new System.Windows.Forms.Button();
            this.button_back = new System.Windows.Forms.Button();
            this.button_offer = new System.Windows.Forms.Button();
            this.button_confirm = new System.Windows.Forms.Button();
            this.button_requeue = new System.Windows.Forms.Button();
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
            this.label_title.Size = new System.Drawing.Size(70, 40);
            this.label_title.TabIndex = 0;
            this.label_title.Text = "Trips";
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
            this.dataGridView1.Size = new System.Drawing.Size(1100, 230);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            //
            // label_id
            //
            this.label_id.AutoSize = true;
            this.label_id.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_id.Location = new System.Drawing.Point(20, 312);
            this.label_id.Name = "label_id";
            this.label_id.Size = new System.Drawing.Size(23, 20);
            this.label_id.TabIndex = 2;
            this.label_id.Text = "ID";
            //
            // textBox_id
            //
            this.textBox_id.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.textBox_id.Location = new System.Drawing.Point(150, 310);
            this.textBox_id.Name = "textBox_id";
            this.textBox_id.ReadOnly = true;
            this.textBox_id.Size = new System.Drawing.Size(160, 27);
            this.textBox_id.TabIndex = 3;
            //
            // label_bookingId
            //
            this.label_bookingId.AutoSize = true;
            this.label_bookingId.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_bookingId.Location = new System.Drawing.Point(20, 352);
            this.label_bookingId.Name = "label_bookingId";
            this.label_bookingId.Size = new System.Drawing.Size(64, 20);
            this.label_bookingId.TabIndex = 4;
            this.label_bookingId.Text = "Booking";
            //
            // textBox_bookingId
            //
            this.textBox_bookingId.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.textBox_bookingId.Location = new System.Drawing.Point(150, 350);
            this.textBox_bookingId.Name = "textBox_bookingId";
            this.textBox_bookingId.Size = new System.Drawing.Size(160, 27);
            this.textBox_bookingId.TabIndex = 5;
            //
            // label_region
            //
            this.label_region.AutoSize = true;
            this.label_region.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_region.Location = new System.Drawing.Point(20, 392);
            this.label_region.Name = "label_region";
            this.label_region.Size = new System.Drawing.Size(56, 20);
            this.label_region.TabIndex = 6;
            this.label_region.Text = "Region";
            //
            // comboBox_region
            //
            this.comboBox_region.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_region.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.comboBox_region.Location = new System.Drawing.Point(150, 390);
            this.comboBox_region.Name = "comboBox_region";
            this.comboBox_region.Size = new System.Drawing.Size(160, 28);
            this.comboBox_region.TabIndex = 7;
            //
            // label_status
            //
            this.label_status.AutoSize = true;
            this.label_status.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_status.Location = new System.Drawing.Point(20, 432);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(50, 20);
            this.label_status.TabIndex = 8;
            this.label_status.Text = "Status";
            //
            // comboBox_status
            //
            this.comboBox_status.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_status.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.comboBox_status.Location = new System.Drawing.Point(150, 430);
            this.comboBox_status.Name = "comboBox_status";
            this.comboBox_status.Size = new System.Drawing.Size(160, 28);
            this.comboBox_status.TabIndex = 9;
            //
            // label_pickupCity
            //
            this.label_pickupCity.AutoSize = true;
            this.label_pickupCity.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_pickupCity.Location = new System.Drawing.Point(340, 312);
            this.label_pickupCity.Name = "label_pickupCity";
            this.label_pickupCity.Size = new System.Drawing.Size(78, 20);
            this.label_pickupCity.TabIndex = 10;
            this.label_pickupCity.Text = "Pickup city";
            //
            // textBox_pickupCity
            //
            this.textBox_pickupCity.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.textBox_pickupCity.Location = new System.Drawing.Point(480, 310);
            this.textBox_pickupCity.Name = "textBox_pickupCity";
            this.textBox_pickupCity.Size = new System.Drawing.Size(180, 27);
            this.textBox_pickupCity.TabIndex = 11;
            //
            // label_pickupAddress
            //
            this.label_pickupAddress.AutoSize = true;
            this.label_pickupAddress.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_pickupAddress.Location = new System.Drawing.Point(340, 352);
            this.label_pickupAddress.Name = "label_pickupAddress";
            this.label_pickupAddress.Size = new System.Drawing.Size(103, 20);
            this.label_pickupAddress.TabIndex = 12;
            this.label_pickupAddress.Text = "Pickup address";
            //
            // textBox_pickupAddress
            //
            this.textBox_pickupAddress.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.textBox_pickupAddress.Location = new System.Drawing.Point(480, 350);
            this.textBox_pickupAddress.Name = "textBox_pickupAddress";
            this.textBox_pickupAddress.Size = new System.Drawing.Size(180, 27);
            this.textBox_pickupAddress.TabIndex = 13;
            //
            // label_dropoffCity
            //
            this.label_dropoffCity.AutoSize = true;
            this.label_dropoffCity.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_dropoffCity.Location = new System.Drawing.Point(340, 392);
            this.label_dropoffCity.Name = "label_dropoffCity";
            this.label_dropoffCity.Size = new System.Drawing.Size(92, 20);
            this.label_dropoffCity.TabIndex = 14;
            this.label_dropoffCity.Text = "Dropoff city";
            //
            // textBox_dropoffCity
            //
            this.textBox_dropoffCity.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.textBox_dropoffCity.Location = new System.Drawing.Point(480, 390);
            this.textBox_dropoffCity.Name = "textBox_dropoffCity";
            this.textBox_dropoffCity.Size = new System.Drawing.Size(180, 27);
            this.textBox_dropoffCity.TabIndex = 15;
            //
            // label_dropoffAddress
            //
            this.label_dropoffAddress.AutoSize = true;
            this.label_dropoffAddress.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_dropoffAddress.Location = new System.Drawing.Point(340, 432);
            this.label_dropoffAddress.Name = "label_dropoffAddress";
            this.label_dropoffAddress.Size = new System.Drawing.Size(117, 20);
            this.label_dropoffAddress.TabIndex = 16;
            this.label_dropoffAddress.Text = "Dropoff address";
            //
            // textBox_dropoffAddress
            //
            this.textBox_dropoffAddress.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.textBox_dropoffAddress.Location = new System.Drawing.Point(480, 430);
            this.textBox_dropoffAddress.Name = "textBox_dropoffAddress";
            this.textBox_dropoffAddress.Size = new System.Drawing.Size(180, 27);
            this.textBox_dropoffAddress.TabIndex = 17;
            //
            // label_pickup
            //
            this.label_pickup.AutoSize = true;
            this.label_pickup.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_pickup.Location = new System.Drawing.Point(690, 312);
            this.label_pickup.Name = "label_pickup";
            this.label_pickup.Size = new System.Drawing.Size(43, 20);
            this.label_pickup.TabIndex = 18;
            this.label_pickup.Text = "Time";
            //
            // dateTimePicker_pickup
            //
            this.dateTimePicker_pickup.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dateTimePicker_pickup.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.dateTimePicker_pickup.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_pickup.Location = new System.Drawing.Point(820, 310);
            this.dateTimePicker_pickup.Name = "dateTimePicker_pickup";
            this.dateTimePicker_pickup.Size = new System.Drawing.Size(200, 27);
            this.dateTimePicker_pickup.TabIndex = 19;
            //
            // label_passengers
            //
            this.label_passengers.AutoSize = true;
            this.label_passengers.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_passengers.Location = new System.Drawing.Point(690, 352);
            this.label_passengers.Name = "label_passengers";
            this.label_passengers.Size = new System.Drawing.Size(82, 20);
            this.label_passengers.TabIndex = 20;
            this.label_passengers.Text = "Passengers";
            //
            // textBox_passengers
            //
            this.textBox_passengers.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.textBox_passengers.Location = new System.Drawing.Point(820, 350);
            this.textBox_passengers.Name = "textBox_passengers";
            this.textBox_passengers.Size = new System.Drawing.Size(200, 27);
            this.textBox_passengers.TabIndex = 21;
            //
            // label_vehicleType
            //
            this.label_vehicleType.AutoSize = true;
            this.label_vehicleType.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_vehicleType.Location = new System.Drawing.Point(690, 392);
            this.label_vehicleType.Name = "label_vehicleType";
            this.label_vehicleType.Size = new System.Drawing.Size(56, 20);
            this.label_vehicleType.TabIndex = 22;
            this.label_vehicleType.Text = "Vehicle";
            //
            // comboBox_vehicleType
            //
            this.comboBox_vehicleType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_vehicleType.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.comboBox_vehicleType.Location = new System.Drawing.Point(820, 390);
            this.comboBox_vehicleType.Name = "comboBox_vehicleType";
            this.comboBox_vehicleType.Size = new System.Drawing.Size(200, 28);
            this.comboBox_vehicleType.TabIndex = 23;
            //
            // label_price
            //
            this.label_price.AutoSize = true;
            this.label_price.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_price.Location = new System.Drawing.Point(690, 432);
            this.label_price.Name = "label_price";
            this.label_price.Size = new System.Drawing.Size(40, 20);
            this.label_price.TabIndex = 24;
            this.label_price.Text = "Price";
            //
            // textBox_price
            //
            this.textBox_price.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.textBox_price.Location = new System.Drawing.Point(820, 430);
            this.textBox_price.Name = "textBox_price";
            this.textBox_price.Size = new System.Drawing.Size(200, 27);
            this.textBox_price.TabIndex = 25;
            //
            // button_add
            //
            this.button_add.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_add.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.button_add.Location = new System.Drawing.Point(20, 490);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(130, 42);
            this.button_add.TabIndex = 26;
            this.button_add.Text = "Add";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            //
            // button_update
            //
            this.button_update.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_update.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.button_update.Location = new System.Drawing.Point(160, 490);
            this.button_update.Name = "button_update";
            this.button_update.Size = new System.Drawing.Size(130, 42);
            this.button_update.TabIndex = 27;
            this.button_update.Text = "Update";
            this.button_update.UseVisualStyleBackColor = true;
            this.button_update.Click += new System.EventHandler(this.button_update_Click);
            //
            // button_delete
            //
            this.button_delete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_delete.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.button_delete.ForeColor = System.Drawing.Color.Firebrick;
            this.button_delete.Location = new System.Drawing.Point(300, 490);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(130, 42);
            this.button_delete.TabIndex = 28;
            this.button_delete.Text = "Delete";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            //
            // button_clear
            //
            this.button_clear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_clear.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.button_clear.Location = new System.Drawing.Point(440, 490);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(130, 42);
            this.button_clear.TabIndex = 29;
            this.button_clear.Text = "Clear";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            //
            // button_back
            //
            this.button_back.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.button_back.Location = new System.Drawing.Point(580, 490);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(130, 42);
            this.button_back.TabIndex = 30;
            this.button_back.Text = "← Back";
            this.button_back.UseVisualStyleBackColor = true;
            this.button_back.Click += new System.EventHandler(this.button_back_Click);
            //
            // button_offer
            //
            this.button_offer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_offer.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.button_offer.ForeColor = System.Drawing.Color.DarkGreen;
            this.button_offer.Location = new System.Drawing.Point(20, 545);
            this.button_offer.Name = "button_offer";
            this.button_offer.Size = new System.Drawing.Size(200, 38);
            this.button_offer.TabIndex = 31;
            this.button_offer.Text = "Offer (→ Offered)";
            this.button_offer.UseVisualStyleBackColor = true;
            this.button_offer.Click += new System.EventHandler(this.button_offer_Click);
            //
            // button_confirm
            //
            this.button_confirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_confirm.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.button_confirm.ForeColor = System.Drawing.Color.DarkGreen;
            this.button_confirm.Location = new System.Drawing.Point(230, 545);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(200, 38);
            this.button_confirm.TabIndex = 32;
            this.button_confirm.Text = "Confirm (→ Confirmed)";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            //
            // button_requeue
            //
            this.button_requeue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_requeue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.button_requeue.ForeColor = System.Drawing.Color.DarkOrange;
            this.button_requeue.Location = new System.Drawing.Point(440, 545);
            this.button_requeue.Name = "button_requeue";
            this.button_requeue.Size = new System.Drawing.Size(200, 38);
            this.button_requeue.TabIndex = 33;
            this.button_requeue.Text = "Requeue (→ Open)";
            this.button_requeue.UseVisualStyleBackColor = true;
            this.button_requeue.Click += new System.EventHandler(this.button_requeue_Click);
            //
            // TripPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button_offer);
            this.Controls.Add(this.button_confirm);
            this.Controls.Add(this.button_requeue);
            this.Controls.Add(this.button_back);
            this.Controls.Add(this.button_clear);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.button_update);
            this.Controls.Add(this.button_add);
            this.Controls.Add(this.textBox_price);
            this.Controls.Add(this.label_price);
            this.Controls.Add(this.comboBox_vehicleType);
            this.Controls.Add(this.label_vehicleType);
            this.Controls.Add(this.textBox_passengers);
            this.Controls.Add(this.label_passengers);
            this.Controls.Add(this.dateTimePicker_pickup);
            this.Controls.Add(this.label_pickup);
            this.Controls.Add(this.textBox_dropoffAddress);
            this.Controls.Add(this.label_dropoffAddress);
            this.Controls.Add(this.textBox_dropoffCity);
            this.Controls.Add(this.label_dropoffCity);
            this.Controls.Add(this.textBox_pickupAddress);
            this.Controls.Add(this.label_pickupAddress);
            this.Controls.Add(this.textBox_pickupCity);
            this.Controls.Add(this.label_pickupCity);
            this.Controls.Add(this.comboBox_status);
            this.Controls.Add(this.label_status);
            this.Controls.Add(this.comboBox_region);
            this.Controls.Add(this.label_region);
            this.Controls.Add(this.textBox_bookingId);
            this.Controls.Add(this.label_bookingId);
            this.Controls.Add(this.textBox_id);
            this.Controls.Add(this.label_id);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label_title);
            this.Name = "TripPanel";
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
        private System.Windows.Forms.Label label_bookingId;
        private System.Windows.Forms.TextBox textBox_bookingId;
        private System.Windows.Forms.Label label_region;
        private System.Windows.Forms.ComboBox comboBox_region;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.ComboBox comboBox_status;
        private System.Windows.Forms.Label label_pickupCity;
        private System.Windows.Forms.TextBox textBox_pickupCity;
        private System.Windows.Forms.Label label_pickupAddress;
        private System.Windows.Forms.TextBox textBox_pickupAddress;
        private System.Windows.Forms.Label label_dropoffCity;
        private System.Windows.Forms.TextBox textBox_dropoffCity;
        private System.Windows.Forms.Label label_dropoffAddress;
        private System.Windows.Forms.TextBox textBox_dropoffAddress;
        private System.Windows.Forms.Label label_pickup;
        private System.Windows.Forms.DateTimePicker dateTimePicker_pickup;
        private System.Windows.Forms.Label label_passengers;
        private System.Windows.Forms.TextBox textBox_passengers;
        private System.Windows.Forms.Label label_vehicleType;
        private System.Windows.Forms.ComboBox comboBox_vehicleType;
        private System.Windows.Forms.Label label_price;
        private System.Windows.Forms.TextBox textBox_price;
        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.Button button_update;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Button button_back;
        private System.Windows.Forms.Button button_offer;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.Button button_requeue;
    }
}
