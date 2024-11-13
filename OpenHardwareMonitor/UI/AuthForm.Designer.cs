namespace OpenHardwareMonitor.UI
{
    partial class AuthForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
      this.enableHTTPAuthCheckBox = new System.Windows.Forms.CheckBox();
      this.httpAuthUsernameTextBox = new System.Windows.Forms.TextBox();
      this.httpAuthPasswordTextBox = new System.Windows.Forms.TextBox();
      this.httpUsernameLabel = new System.Windows.Forms.Label();
      this.httpPasswordLabel = new System.Windows.Forms.Label();
      this.credentialsGroupBox = new System.Windows.Forms.GroupBox();
      this.httpAuthCancelButton = new System.Windows.Forms.Button();
      this.httpAuthOkButton = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.credentialsGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // enableHTTPAuthCheckBox
      // 
      this.enableHTTPAuthCheckBox.AutoSize = true;
      this.enableHTTPAuthCheckBox.Location = new System.Drawing.Point(12, 12);
      this.enableHTTPAuthCheckBox.Name = "enableHTTPAuthCheckBox";
      this.enableHTTPAuthCheckBox.Size = new System.Drawing.Size(279, 24);
      this.enableHTTPAuthCheckBox.TabIndex = 0;
      this.enableHTTPAuthCheckBox.Text = "Enable HTTP Basic Authentication";
      this.enableHTTPAuthCheckBox.UseVisualStyleBackColor = true;
      this.enableHTTPAuthCheckBox.CheckedChanged += new System.EventHandler(this.EnableHTTPAuthCheckBox_CheckedChanged);
      // 
      // httpAuthUsernameTextBox
      // 
      this.httpAuthUsernameTextBox.Location = new System.Drawing.Point(153, 35);
      this.httpAuthUsernameTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.httpAuthUsernameTextBox.MaxLength = 255;
      this.httpAuthUsernameTextBox.Name = "httpAuthUsernameTextBox";
      this.httpAuthUsernameTextBox.Size = new System.Drawing.Size(254, 26);
      this.httpAuthUsernameTextBox.TabIndex = 1;
      // 
      // httpAuthPasswordTextBox
      // 
      this.httpAuthPasswordTextBox.Location = new System.Drawing.Point(153, 92);
      this.httpAuthPasswordTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.httpAuthPasswordTextBox.MaxLength = 255;
      this.httpAuthPasswordTextBox.Name = "httpAuthPasswordTextBox";
      this.httpAuthPasswordTextBox.PasswordChar = '*';
      this.httpAuthPasswordTextBox.Size = new System.Drawing.Size(254, 26);
      this.httpAuthPasswordTextBox.TabIndex = 2;
      this.httpAuthPasswordTextBox.UseSystemPasswordChar = true;
      // 
      // httpUsernameLabel
      // 
      this.httpUsernameLabel.AutoSize = true;
      this.httpUsernameLabel.Location = new System.Drawing.Point(9, 40);
      this.httpUsernameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.httpUsernameLabel.Name = "httpUsernameLabel";
      this.httpUsernameLabel.Size = new System.Drawing.Size(133, 20);
      this.httpUsernameLabel.TabIndex = 6;
      this.httpUsernameLabel.Text = "HTTP UserName:";
      // 
      // httpPasswordLabel
      // 
      this.httpPasswordLabel.AutoSize = true;
      this.httpPasswordLabel.Location = new System.Drawing.Point(9, 97);
      this.httpPasswordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.httpPasswordLabel.Name = "httpPasswordLabel";
      this.httpPasswordLabel.Size = new System.Drawing.Size(126, 20);
      this.httpPasswordLabel.TabIndex = 7;
      this.httpPasswordLabel.Text = "HTTP Password:";
      // 
      // credentialsGroupBox
      // 
      this.credentialsGroupBox.Controls.Add(this.httpAuthUsernameTextBox);
      this.credentialsGroupBox.Controls.Add(this.httpPasswordLabel);
      this.credentialsGroupBox.Controls.Add(this.httpAuthPasswordTextBox);
      this.credentialsGroupBox.Controls.Add(this.httpUsernameLabel);
      this.credentialsGroupBox.Location = new System.Drawing.Point(18, 54);
      this.credentialsGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.credentialsGroupBox.Name = "credentialsGroupBox";
      this.credentialsGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.credentialsGroupBox.Size = new System.Drawing.Size(418, 154);
      this.credentialsGroupBox.TabIndex = 5;
      this.credentialsGroupBox.TabStop = false;
      this.credentialsGroupBox.Text = "Credentials";
      // 
      // httpAuthCancelButton
      // 
      this.httpAuthCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.httpAuthCancelButton.Location = new System.Drawing.Point(114, 317);
      this.httpAuthCancelButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.httpAuthCancelButton.Name = "httpAuthCancelButton";
      this.httpAuthCancelButton.Size = new System.Drawing.Size(112, 35);
      this.httpAuthCancelButton.TabIndex = 3;
      this.httpAuthCancelButton.Text = "Cancel";
      this.httpAuthCancelButton.UseVisualStyleBackColor = true;
      this.httpAuthCancelButton.Click += new System.EventHandler(this.HttpAuthCancelButton_Click);
      // 
      // httpAuthOkButton
      // 
      this.httpAuthOkButton.Location = new System.Drawing.Point(236, 317);
      this.httpAuthOkButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.httpAuthOkButton.Name = "httpAuthOkButton";
      this.httpAuthOkButton.Size = new System.Drawing.Size(112, 35);
      this.httpAuthOkButton.TabIndex = 4;
      this.httpAuthOkButton.Text = "OK";
      this.httpAuthOkButton.UseVisualStyleBackColor = true;
      this.httpAuthOkButton.Click += new System.EventHandler(this.HttpAuthOkButton_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(18, 212);
      this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(399, 40);
      this.label1.TabIndex = 8;
      this.label1.Text = "Password is stored hashed, if you forget it you will need \r\nto set a new one.";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(18, 268);
      this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(422, 40);
      this.label2.TabIndex = 9;
      this.label2.Text = "If the web server is running then it will need to be restarted \r\nfor the change t" +
    "o take effect.";
      // 
      // AuthForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.httpAuthCancelButton;
      this.ClientSize = new System.Drawing.Size(458, 371);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.httpAuthOkButton);
      this.Controls.Add(this.httpAuthCancelButton);
      this.Controls.Add(this.credentialsGroupBox);
      this.Controls.Add(this.enableHTTPAuthCheckBox);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AuthForm";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Set HTTP Credentials";
      this.Load += new System.EventHandler(this.AuthForm_Load);
      this.credentialsGroupBox.ResumeLayout(false);
      this.credentialsGroupBox.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox enableHTTPAuthCheckBox;
        private System.Windows.Forms.TextBox httpAuthUsernameTextBox;
        private System.Windows.Forms.TextBox httpAuthPasswordTextBox;
        private System.Windows.Forms.Label httpUsernameLabel;
        private System.Windows.Forms.Label httpPasswordLabel;
        private System.Windows.Forms.GroupBox credentialsGroupBox;
        private System.Windows.Forms.Button httpAuthCancelButton;
        private System.Windows.Forms.Button httpAuthOkButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
