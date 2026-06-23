namespace ExternalDriverDispatch
{
    partial class RegionPanel
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
            this.label_name = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label_country = new System.Windows.Forms.Label();
            this.textBox_country = new System.Windows.Forms.TextBox();
            this.label_city = new System.Windows.Forms.Label();
            this.textBox_city = new System.Windows.Forms.TextBox();
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
            this.label_title.Size = new System.Drawing.Size(130, 45);
            this.label_title.TabIndex = 0;
            this.label_title.Text = "Regions";
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
            this.label_id.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label_id.Location = new System.Drawing.Point(640, 92);
            this.label_id.Name = "label_id";
            this.label_id.Size = new System.Drawing.Size(25, 21);
            this.label_id.TabIndex = 2;
            this.label_id.Text = "ID";
            //
            // textBox_id
            //
            this.textBox_id.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.textBox_id.Location = new System.Drawing.Point(760, 90);
            this.textBox_id.Name = "textBox_id";
            this.textBox_id.ReadOnly = true;
            this.textBox_id.Size = new System.Drawing.Size(240, 29);
            this.textBox_id.TabIndex = 3;
            //
            // label_name
            //
            this.label_name.AutoSize = true;
            this.label_name.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label_name.Location = new System.Drawing.Point(640, 142);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(49, 21);
            this.label_name.TabIndex = 4;
            this.label_name.Text = "Name";
            //
            // textBox_name
            //
            this.textBox_name.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.textBox_name.Location = new System.Drawing.Point(760, 140);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(240, 29);
            this.textBox_name.TabIndex = 5;
            //
            // label_country
            //
            this.label_country.AutoSize = true;
            this.label_country.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label_country.Location = new System.Drawing.Point(640, 192);
            this.label_country.Name = "label_country";
            this.label_country.Size = new System.Drawing.Size(63, 21);
            this.label_country.TabIndex = 6;
            this.label_country.Text = "Country";
            //
            // textBox_country
            //
            this.textBox_country.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.textBox_country.Location = new System.Drawing.Point(760, 190);
            this.textBox_country.Name = "textBox_country";
            this.textBox_country.Size = new System.Drawing.Size(240, 29);
            this.textBox_country.TabIndex = 7;
            //
            // label_city
            //
            this.label_city.AutoSize = true;
            this.label_city.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label_city.Location = new System.Drawing.Point(640, 242);
            this.label_city.Name = "label_city";
            this.label_city.Size = new System.Drawing.Size(33, 21);
            this.label_city.TabIndex = 8;
            this.label_city.Text = "City";
            //
            // textBox_city
            //
            this.textBox_city.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.textBox_city.Location = new System.Drawing.Point(760, 240);
            this.textBox_city.Name = "textBox_city";
            this.textBox_city.Size = new System.Drawing.Size(240, 29);
            this.textBox_city.TabIndex = 9;
            //
            // button_add
            //
            this.button_add.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_add.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.button_add.Location = new System.Drawing.Point(640, 320);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(170, 45);
            this.button_add.TabIndex = 10;
            this.button_add.Text = "Add";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            //
            // button_update
            //
            this.button_update.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_update.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.button_update.Location = new System.Drawing.Point(830, 320);
            this.button_update.Name = "button_update";
            this.button_update.Size = new System.Drawing.Size(170, 45);
            this.button_update.TabIndex = 11;
            this.button_update.Text = "Update";
            this.button_update.UseVisualStyleBackColor = true;
            this.button_update.Click += new System.EventHandler(this.button_update_Click);
            //
            // button_delete
            //
            this.button_delete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_delete.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.button_delete.ForeColor = System.Drawing.Color.Firebrick;
            this.button_delete.Location = new System.Drawing.Point(640, 375);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(170, 45);
            this.button_delete.TabIndex = 12;
            this.button_delete.Text = "Delete";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            //
            // button_clear
            //
            this.button_clear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_clear.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.button_clear.Location = new System.Drawing.Point(830, 375);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(170, 45);
            this.button_clear.TabIndex = 13;
            this.button_clear.Text = "Clear";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            //
            // button_back
            //
            this.button_back.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.button_back.Location = new System.Drawing.Point(640, 435);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(360, 40);
            this.button_back.TabIndex = 14;
            this.button_back.Text = "← Back";
            this.button_back.UseVisualStyleBackColor = true;
            this.button_back.Click += new System.EventHandler(this.button_back_Click);
            //
            // RegionPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button_back);
            this.Controls.Add(this.button_clear);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.button_update);
            this.Controls.Add(this.button_add);
            this.Controls.Add(this.textBox_city);
            this.Controls.Add(this.label_city);
            this.Controls.Add(this.textBox_country);
            this.Controls.Add(this.label_country);
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.label_name);
            this.Controls.Add(this.textBox_id);
            this.Controls.Add(this.label_id);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label_title);
            this.Name = "RegionPanel";
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
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Label label_country;
        private System.Windows.Forms.TextBox textBox_country;
        private System.Windows.Forms.Label label_city;
        private System.Windows.Forms.TextBox textBox_city;
        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.Button button_update;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Button button_back;
    }
}
