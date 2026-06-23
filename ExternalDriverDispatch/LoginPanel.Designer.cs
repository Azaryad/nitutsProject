namespace ExternalDriverDispatch
{
    partial class LoginPanel
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
            this.label_subtitle = new System.Windows.Forms.Label();
            this.label_user = new System.Windows.Forms.Label();
            this.label_password = new System.Windows.Forms.Label();
            this.textBox_user = new System.Windows.Forms.TextBox();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.enter = new System.Windows.Forms.Button();
            this.devEnter = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // label_title
            //
            this.label_title.AutoSize = true;
            this.label_title.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.label_title.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label_title.Location = new System.Drawing.Point(300, 80);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(540, 47);
            this.label_title.TabIndex = 0;
            this.label_title.Text = "External Driver Dispatch";
            //
            // label_subtitle
            //
            this.label_subtitle.AutoSize = true;
            this.label_subtitle.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.label_subtitle.ForeColor = System.Drawing.Color.Gray;
            this.label_subtitle.Location = new System.Drawing.Point(380, 135);
            this.label_subtitle.Name = "label_subtitle";
            this.label_subtitle.Size = new System.Drawing.Size(240, 25);
            this.label_subtitle.TabIndex = 1;
            this.label_subtitle.Text = "Transfers TLV — Dispatcher login";
            //
            // label_user
            //
            this.label_user.AutoSize = true;
            this.label_user.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label_user.Location = new System.Drawing.Point(360, 220);
            this.label_user.Name = "label_user";
            this.label_user.Size = new System.Drawing.Size(80, 21);
            this.label_user.TabIndex = 2;
            this.label_user.Text = "Username";
            //
            // label_password
            //
            this.label_password.AutoSize = true;
            this.label_password.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label_password.Location = new System.Drawing.Point(360, 270);
            this.label_password.Name = "label_password";
            this.label_password.Size = new System.Drawing.Size(75, 21);
            this.label_password.TabIndex = 3;
            this.label_password.Text = "Password";
            //
            // textBox_user
            //
            this.textBox_user.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.textBox_user.Location = new System.Drawing.Point(470, 218);
            this.textBox_user.Name = "textBox_user";
            this.textBox_user.Size = new System.Drawing.Size(220, 29);
            this.textBox_user.TabIndex = 4;
            //
            // textBox_password
            //
            this.textBox_password.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.textBox_password.Location = new System.Drawing.Point(470, 268);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.Size = new System.Drawing.Size(220, 29);
            this.textBox_password.TabIndex = 5;
            this.textBox_password.UseSystemPasswordChar = true;
            //
            // enter
            //
            this.enter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.enter.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.enter.Location = new System.Drawing.Point(470, 330);
            this.enter.Name = "enter";
            this.enter.Size = new System.Drawing.Size(160, 50);
            this.enter.TabIndex = 6;
            this.enter.Text = "Sign in";
            this.enter.UseVisualStyleBackColor = true;
            this.enter.Click += new System.EventHandler(this.enter_Click);
            //
            // devEnter
            //
            this.devEnter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.devEnter.ForeColor = System.Drawing.Color.Gray;
            this.devEnter.Location = new System.Drawing.Point(470, 392);
            this.devEnter.Name = "devEnter";
            this.devEnter.Size = new System.Drawing.Size(160, 30);
            this.devEnter.TabIndex = 7;
            this.devEnter.Text = "Dev bypass";
            this.devEnter.UseVisualStyleBackColor = true;
            this.devEnter.Click += new System.EventHandler(this.devEnter_Click);
            //
            // LoginPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.devEnter);
            this.Controls.Add(this.enter);
            this.Controls.Add(this.textBox_password);
            this.Controls.Add(this.textBox_user);
            this.Controls.Add(this.label_password);
            this.Controls.Add(this.label_user);
            this.Controls.Add(this.label_subtitle);
            this.Controls.Add(this.label_title);
            this.Name = "LoginPanel";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Size = new System.Drawing.Size(1150, 680);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.Label label_subtitle;
        private System.Windows.Forms.Label label_user;
        private System.Windows.Forms.Label label_password;
        private System.Windows.Forms.TextBox textBox_user;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.Button enter;
        private System.Windows.Forms.Button devEnter;
    }
}
