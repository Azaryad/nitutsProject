namespace ExternalDriverDispatch
{
    partial class ExternalDriverPanel
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
            this.label_drivercode = new System.Windows.Forms.Label();
            this.textBox_drivercode = new System.Windows.Forms.TextBox();
            this.label_name = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label_phone = new System.Windows.Forms.Label();
            this.textBox_phone = new System.Windows.Forms.TextBox();
            this.label_homeCity = new System.Windows.Forms.Label();
            this.textBox_homeCity = new System.Windows.Forms.TextBox();
            this.label_vehicleType = new System.Windows.Forms.Label();
            this.comboBox_vehicleType = new System.Windows.Forms.ComboBox();
            this.checkBox_shabbat = new System.Windows.Forms.CheckBox();
            this.checkBox_nights = new System.Windows.Forms.CheckBox();
            this.checkBox_long = new System.Windows.Forms.CheckBox();
            this.checkBox_active = new System.Windows.Forms.CheckBox();
            this.button_add = new System.Windows.Forms.Button();
            this.button_update = new System.Windows.Forms.Button();
            this.button_delete = new System.Windows.Forms.Button();
            this.button_clear = new System.Windows.Forms.Button();
            this.button_back = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            //
            // label_title
            //
            this.label_title.AutoSize = true;
            this.label_title.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.label_title.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label_title.Location = new System.Drawing.Point(30, 12);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(110, 45);
            this.label_title.TabIndex = 0;
            this.label_title.Text = "Drivers";
            //
            // dataGridView1
            //
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(30, 80);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(560, 480);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            //
            // label_id
            //
            this.label_id.AutoSize = true;
            this.label_id.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_id.Location = new System.Drawing.Point(620, 92);
            this.label_id.Name = "label_id";
            this.label_id.Size = new System.Drawing.Size(23, 20);
            this.label_id.TabIndex = 2;
            this.label_id.Text = "ID";
            //
            // textBox_id
            //
            this.textBox_id.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.textBox_id.Location = new System.Drawing.Point(760, 90);
            this.textBox_id.Name = "textBox_id";
            this.textBox_id.ReadOnly = true;
            this.textBox_id.Size = new System.Drawing.Size(240, 27);
            this.textBox_id.TabIndex = 3;
            //
            // label_drivercode
            //
            this.label_drivercode.AutoSize = true;
            this.label_drivercode.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_drivercode.Location = new System.Drawing.Point(620, 132);
            this.label_drivercode.Name = "label_drivercode";
            this.label_drivercode.Size = new System.Drawing.Size(85, 20);
            this.label_drivercode.TabIndex = 4;
            this.label_drivercode.Text = "Driver code";
            //
            // textBox_drivercode
            //
            this.textBox_drivercode.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.textBox_drivercode.Location = new System.Drawing.Point(760, 130);
            this.textBox_drivercode.Name = "textBox_drivercode";
            this.textBox_drivercode.Size = new System.Drawing.Size(240, 27);
            this.textBox_drivercode.TabIndex = 5;
            //
            // label_name
            //
            this.label_name.AutoSize = true;
            this.label_name.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_name.Location = new System.Drawing.Point(620, 172);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(48, 20);
            this.label_name.TabIndex = 6;
            this.label_name.Text = "Name";
            //
            // textBox_name
            //
            this.textBox_name.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.textBox_name.Location = new System.Drawing.Point(760, 170);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(240, 27);
            this.textBox_name.TabIndex = 7;
            //
            // label_phone
            //
            this.label_phone.AutoSize = true;
            this.label_phone.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_phone.Location = new System.Drawing.Point(620, 212);
            this.label_phone.Name = "label_phone";
            this.label_phone.Size = new System.Drawing.Size(49, 20);
            this.label_phone.TabIndex = 8;
            this.label_phone.Text = "Phone";
            //
            // textBox_phone
            //
            this.textBox_phone.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.textBox_phone.Location = new System.Drawing.Point(760, 210);
            this.textBox_phone.Name = "textBox_phone";
            this.textBox_phone.Size = new System.Drawing.Size(240, 27);
            this.textBox_phone.TabIndex = 9;
            //
            // label_homeCity
            //
            this.label_homeCity.AutoSize = true;
            this.label_homeCity.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_homeCity.Location = new System.Drawing.Point(620, 252);
            this.label_homeCity.Name = "label_homeCity";
            this.label_homeCity.Size = new System.Drawing.Size(74, 20);
            this.label_homeCity.TabIndex = 10;
            this.label_homeCity.Text = "Home city";
            //
            // textBox_homeCity
            //
            this.textBox_homeCity.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.textBox_homeCity.Location = new System.Drawing.Point(760, 250);
            this.textBox_homeCity.Name = "textBox_homeCity";
            this.textBox_homeCity.Size = new System.Drawing.Size(240, 27);
            this.textBox_homeCity.TabIndex = 11;
            //
            // label_vehicleType
            //
            this.label_vehicleType.AutoSize = true;
            this.label_vehicleType.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label_vehicleType.Location = new System.Drawing.Point(620, 292);
            this.label_vehicleType.Name = "label_vehicleType";
            this.label_vehicleType.Size = new System.Drawing.Size(56, 20);
            this.label_vehicleType.TabIndex = 12;
            this.label_vehicleType.Text = "Vehicle";
            //
            // comboBox_vehicleType
            //
            this.comboBox_vehicleType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_vehicleType.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.comboBox_vehicleType.Location = new System.Drawing.Point(760, 290);
            this.comboBox_vehicleType.Name = "comboBox_vehicleType";
            this.comboBox_vehicleType.Size = new System.Drawing.Size(240, 28);
            this.comboBox_vehicleType.TabIndex = 13;
            //
            // checkBox_shabbat
            //
            this.checkBox_shabbat.AutoSize = true;
            this.checkBox_shabbat.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.checkBox_shabbat.Location = new System.Drawing.Point(620, 332);
            this.checkBox_shabbat.Name = "checkBox_shabbat";
            this.checkBox_shabbat.Size = new System.Drawing.Size(125, 24);
            this.checkBox_shabbat.TabIndex = 14;
            this.checkBox_shabbat.Text = "Works Shabbat";
            this.checkBox_shabbat.UseVisualStyleBackColor = true;
            //
            // checkBox_nights
            //
            this.checkBox_nights.AutoSize = true;
            this.checkBox_nights.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.checkBox_nights.Location = new System.Drawing.Point(820, 332);
            this.checkBox_nights.Name = "checkBox_nights";
            this.checkBox_nights.Size = new System.Drawing.Size(110, 24);
            this.checkBox_nights.TabIndex = 15;
            this.checkBox_nights.Text = "Works nights";
            this.checkBox_nights.UseVisualStyleBackColor = true;
            //
            // checkBox_long
            //
            this.checkBox_long.AutoSize = true;
            this.checkBox_long.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.checkBox_long.Location = new System.Drawing.Point(620, 362);
            this.checkBox_long.Name = "checkBox_long";
            this.checkBox_long.Size = new System.Drawing.Size(115, 24);
            this.checkBox_long.TabIndex = 16;
            this.checkBox_long.Text = "Long distance";
            this.checkBox_long.UseVisualStyleBackColor = true;
            //
            // checkBox_active
            //
            this.checkBox_active.AutoSize = true;
            this.checkBox_active.Checked = true;
            this.checkBox_active.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_active.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.checkBox_active.Location = new System.Drawing.Point(820, 362);
            this.checkBox_active.Name = "checkBox_active";
            this.checkBox_active.Size = new System.Drawing.Size(67, 24);
            this.checkBox_active.TabIndex = 17;
            this.checkBox_active.Text = "Active";
            this.checkBox_active.UseVisualStyleBackColor = true;
            //
            // button_add
            //
            this.button_add.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_add.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.button_add.Location = new System.Drawing.Point(620, 400);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(180, 42);
            this.button_add.TabIndex = 18;
            this.button_add.Text = "Add";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            //
            // button_update
            //
            this.button_update.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_update.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.button_update.Location = new System.Drawing.Point(820, 400);
            this.button_update.Name = "button_update";
            this.button_update.Size = new System.Drawing.Size(180, 42);
            this.button_update.TabIndex = 19;
            this.button_update.Text = "Update";
            this.button_update.UseVisualStyleBackColor = true;
            this.button_update.Click += new System.EventHandler(this.button_update_Click);
            //
            // button_delete
            //
            this.button_delete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_delete.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.button_delete.ForeColor = System.Drawing.Color.Firebrick;
            this.button_delete.Location = new System.Drawing.Point(620, 450);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(180, 42);
            this.button_delete.TabIndex = 20;
            this.button_delete.Text = "Delete";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            //
            // button_clear
            //
            this.button_clear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_clear.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.button_clear.Location = new System.Drawing.Point(820, 450);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(180, 42);
            this.button_clear.TabIndex = 21;
            this.button_clear.Text = "Clear";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            //
            // button_back
            //
            this.button_back.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.button_back.Location = new System.Drawing.Point(620, 502);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(380, 40);
            this.button_back.TabIndex = 22;
            this.button_back.Text = "← Back";
            this.button_back.UseVisualStyleBackColor = true;
            this.button_back.Click += new System.EventHandler(this.button_back_Click);
            //
            // ExternalDriverPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button_back);
            this.Controls.Add(this.button_clear);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.button_update);
            this.Controls.Add(this.button_add);
            this.Controls.Add(this.checkBox_active);
            this.Controls.Add(this.checkBox_long);
            this.Controls.Add(this.checkBox_nights);
            this.Controls.Add(this.checkBox_shabbat);
            this.Controls.Add(this.comboBox_vehicleType);
            this.Controls.Add(this.label_vehicleType);
            this.Controls.Add(this.textBox_homeCity);
            this.Controls.Add(this.label_homeCity);
            this.Controls.Add(this.textBox_phone);
            this.Controls.Add(this.label_phone);
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.label_name);
            this.Controls.Add(this.textBox_drivercode);
            this.Controls.Add(this.label_drivercode);
            this.Controls.Add(this.textBox_id);
            this.Controls.Add(this.label_id);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label_title);
            this.Name = "ExternalDriverPanel";
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
        private System.Windows.Forms.Label label_drivercode;
        private System.Windows.Forms.TextBox textBox_drivercode;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Label label_phone;
        private System.Windows.Forms.TextBox textBox_phone;
        private System.Windows.Forms.Label label_homeCity;
        private System.Windows.Forms.TextBox textBox_homeCity;
        private System.Windows.Forms.Label label_vehicleType;
        private System.Windows.Forms.ComboBox comboBox_vehicleType;
        private System.Windows.Forms.CheckBox checkBox_shabbat;
        private System.Windows.Forms.CheckBox checkBox_nights;
        private System.Windows.Forms.CheckBox checkBox_long;
        private System.Windows.Forms.CheckBox checkBox_active;
        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.Button button_update;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Button button_back;
    }
}
