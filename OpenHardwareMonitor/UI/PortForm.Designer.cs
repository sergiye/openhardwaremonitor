// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) OpenHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

namespace OpenHardwareMonitor.UI
{
    partial class PortForm
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
      this.portOKButton = new System.Windows.Forms.Button();
      this.portCancelButton = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.webServerLinkLabel = new System.Windows.Forms.LinkLabel();
      this.portNumericUpDn = new System.Windows.Forms.NumericUpDown();
      this.label5 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.portNumericUpDn)).BeginInit();
      this.SuspendLayout();
      // 
      // portOKButton
      // 
      this.portOKButton.Location = new System.Drawing.Point(366, 211);
      this.portOKButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.portOKButton.Name = "portOKButton";
      this.portOKButton.Size = new System.Drawing.Size(112, 35);
      this.portOKButton.TabIndex = 0;
      this.portOKButton.Text = "OK";
      this.portOKButton.UseVisualStyleBackColor = true;
      this.portOKButton.Click += new System.EventHandler(this.PortOKButton_Click);
      // 
      // portCancelButton
      // 
      this.portCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.portCancelButton.Location = new System.Drawing.Point(243, 211);
      this.portCancelButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.portCancelButton.Name = "portCancelButton";
      this.portCancelButton.Size = new System.Drawing.Size(112, 35);
      this.portCancelButton.TabIndex = 1;
      this.portCancelButton.Text = "Cancel";
      this.portCancelButton.UseVisualStyleBackColor = true;
      this.portCancelButton.Click += new System.EventHandler(this.PortCancelButton_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(20, 163);
      this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(563, 20);
      this.label1.TabIndex = 3;
      this.label1.Text = "Note: You will need to open the port in firewall settings of the operating system" +
    ".";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(20, 14);
      this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(288, 20);
      this.label2.TabIndex = 4;
      this.label2.Text = "Port number for  the remote web server:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(20, 60);
      this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(659, 20);
      this.label3.TabIndex = 5;
      this.label3.Text = "If the web server is running then it will need to be restarted for the port chang" +
    "e to take effect.";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(20, 95);
      this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(385, 20);
      this.label4.TabIndex = 6;
      this.label4.Text = "The web server will be accessible from the browser at ";
      // 
      // webServerLinkLabel
      // 
      this.webServerLinkLabel.AutoSize = true;
      this.webServerLinkLabel.Location = new System.Drawing.Point(404, 95);
      this.webServerLinkLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.webServerLinkLabel.Name = "webServerLinkLabel";
      this.webServerLinkLabel.Size = new System.Drawing.Size(80, 20);
      this.webServerLinkLabel.TabIndex = 7;
      this.webServerLinkLabel.TabStop = true;
      this.webServerLinkLabel.Text = "linkLabel1";
      this.webServerLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WebServerLinkLabel_LinkClicked);
      // 
      // portNumericUpDn
      // 
      this.portNumericUpDn.Location = new System.Drawing.Point(312, 11);
      this.portNumericUpDn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.portNumericUpDn.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
      this.portNumericUpDn.Minimum = new decimal(new int[] {
            8080,
            0,
            0,
            0});
      this.portNumericUpDn.Name = "portNumericUpDn";
      this.portNumericUpDn.Size = new System.Drawing.Size(112, 26);
      this.portNumericUpDn.TabIndex = 8;
      this.portNumericUpDn.Value = new decimal(new int[] {
            8080,
            0,
            0,
            0});
      this.portNumericUpDn.ValueChanged += new System.EventHandler(this.PortNumericUpDn_ValueChanged);
      this.portNumericUpDn.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PortNumericUpDn_KeyUp);
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(20, 129);
      this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(448, 20);
      this.label5.TabIndex = 9;
      this.label5.Text = "You will have to start the server by clicking Run from the menu.";
      // 
      // PortForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.portCancelButton;
      this.ClientSize = new System.Drawing.Size(699, 262);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.portNumericUpDn);
      this.Controls.Add(this.webServerLinkLabel);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.portCancelButton);
      this.Controls.Add(this.portOKButton);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "PortForm";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Set Port";
      this.Load += new System.EventHandler(this.PortForm_Load);
      ((System.ComponentModel.ISupportInitialize)(this.portNumericUpDn)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button portOKButton;
        private System.Windows.Forms.Button portCancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel webServerLinkLabel;
        private System.Windows.Forms.NumericUpDown portNumericUpDn;
        private System.Windows.Forms.Label label5;
    }
}
