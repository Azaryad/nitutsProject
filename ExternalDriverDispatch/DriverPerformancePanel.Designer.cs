namespace ExternalDriverDispatch
{
    partial class DriverPerformancePanel
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
            this.label_region = new System.Windows.Forms.Label();
            this.comboRegion = new System.Windows.Forms.ComboBox();
            this.label_from = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label_to = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.button_generate = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label_summary = new System.Windows.Forms.Label();
            this.button_back = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            //
            // label_title
            //
            this.label_title.AutoSize = true;
            this.label_title.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.label_title.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label_title.Location = new System.Drawing.Point(24, 20);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(360, 37);
            this.label_title.TabIndex = 0;
            this.label_title.Text = "Driver Performance Report";
            //
            // label_region
            //
            this.label_region.AutoSize = true;
            this.label_region.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label_region.Location = new System.Drawing.Point(24, 80);
            this.label_region.Name = "label_region";
            this.label_region.Size = new System.Drawing.Size(54, 19);
            this.label_region.TabIndex = 1;
            this.label_region.Text = "Region:";
            //
            // comboRegion
            //
            this.comboRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRegion.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.comboRegion.Location = new System.Drawing.Point(90, 76);
            this.comboRegion.Name = "comboRegion";
            this.comboRegion.Size = new System.Drawing.Size(220, 25);
            this.comboRegion.TabIndex = 2;
            //
            // label_from
            //
            this.label_from.AutoSize = true;
            this.label_from.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label_from.Location = new System.Drawing.Point(340, 80);
            this.label_from.Name = "label_from";
            this.label_from.Size = new System.Drawing.Size(43, 19);
            this.label_from.TabIndex = 3;
            this.label_from.Text = "From:";
            //
            // dtpFrom
            //
            this.dtpFrom.Checked = false;
            this.dtpFrom.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(389, 76);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.ShowCheckBox = true;
            this.dtpFrom.Size = new System.Drawing.Size(140, 25);
            this.dtpFrom.TabIndex = 4;
            //
            // label_to
            //
            this.label_to.AutoSize = true;
            this.label_to.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label_to.Location = new System.Drawing.Point(545, 80);
            this.label_to.Name = "label_to";
            this.label_to.Size = new System.Drawing.Size(27, 19);
            this.label_to.TabIndex = 5;
            this.label_to.Text = "To:";
            //
            // dtpTo
            //
            this.dtpTo.Checked = false;
            this.dtpTo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(578, 76);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.ShowCheckBox = true;
            this.dtpTo.Size = new System.Drawing.Size(140, 25);
            this.dtpTo.TabIndex = 6;
            //
            // button_generate
            //
            this.button_generate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_generate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.button_generate.Location = new System.Drawing.Point(740, 74);
            this.button_generate.Name = "button_generate";
            this.button_generate.Size = new System.Drawing.Size(130, 30);
            this.button_generate.TabIndex = 7;
            this.button_generate.Text = "Generate";
            this.button_generate.UseVisualStyleBackColor = true;
            this.button_generate.Click += new System.EventHandler(this.button_generate_Click);
            //
            // dataGridView1
            //
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.DefaultCellStyle.NullValue = "—";
            this.dataGridView1.Location = new System.Drawing.Point(24, 120);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1100, 470);
            this.dataGridView1.TabIndex = 8;
            //
            // label_summary
            //
            this.label_summary.AutoSize = true;
            this.label_summary.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label_summary.ForeColor = System.Drawing.Color.DimGray;
            this.label_summary.Location = new System.Drawing.Point(24, 600);
            this.label_summary.Name = "label_summary";
            this.label_summary.Size = new System.Drawing.Size(0, 19);
            this.label_summary.TabIndex = 9;
            //
            // button_back
            //
            this.button_back.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.button_back.Location = new System.Drawing.Point(1004, 600);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(120, 38);
            this.button_back.TabIndex = 10;
            this.button_back.Text = "← Back";
            this.button_back.UseVisualStyleBackColor = true;
            this.button_back.Click += new System.EventHandler(this.button_back_Click);
            //
            // DriverPerformancePanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button_back);
            this.Controls.Add(this.label_summary);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button_generate);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.label_to);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.label_from);
            this.Controls.Add(this.comboRegion);
            this.Controls.Add(this.label_region);
            this.Controls.Add(this.label_title);
            this.Name = "DriverPerformancePanel";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Size = new System.Drawing.Size(1150, 680);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.Label label_region;
        private System.Windows.Forms.ComboBox comboRegion;
        private System.Windows.Forms.Label label_from;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label_to;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button button_generate;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label_summary;
        private System.Windows.Forms.Button button_back;
    }
}
