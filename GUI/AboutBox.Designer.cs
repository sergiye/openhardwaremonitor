namespace OpenHardwareMonitor.GUI {
  partial class AboutBox {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
      this.okButton = new System.Windows.Forms.Button();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.lblVersion = new System.Windows.Forms.Label();
      this.projectLinkLabel = new System.Windows.Forms.LinkLabel();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.SuspendLayout();
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.okButton.Location = new System.Drawing.Point(269, 79);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 23);
      this.okButton.TabIndex = 0;
      this.okButton.Text = "Close";
      this.okButton.UseVisualStyleBackColor = true;
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
      this.pictureBox1.Location = new System.Drawing.Point(10, 11);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(48, 48);
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.pictureBox1.TabIndex = 1;
      this.pictureBox1.TabStop = false;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(74, 12);
      this.label1.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(120, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Open Hardware Monitor";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(74, 46);
      this.label2.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(166, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Copyright © 2022 Sergiy Egoshyn";
      // 
      // lblVersion
      // 
      this.lblVersion.AutoSize = true;
      this.lblVersion.Location = new System.Drawing.Point(74, 29);
      this.lblVersion.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
      this.lblVersion.Name = "lblVersion";
      this.lblVersion.Size = new System.Drawing.Size(69, 13);
      this.lblVersion.TabIndex = 4;
      this.lblVersion.Text = "Version 9.0.7";
      // 
      // projectLinkLabel
      // 
      this.projectLinkLabel.AutoSize = true;
      this.projectLinkLabel.Location = new System.Drawing.Point(74, 68);
      this.projectLinkLabel.Margin = new System.Windows.Forms.Padding(0);
      this.projectLinkLabel.Name = "projectLinkLabel";
      this.projectLinkLabel.Size = new System.Drawing.Size(82, 13);
      this.projectLinkLabel.TabIndex = 6;
      this.projectLinkLabel.TabStop = true;
      this.projectLinkLabel.Text = "Project Website";
      this.projectLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_LinkClicked);
      // 
      // AboutBox
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.AutoSize = true;
      this.CancelButton = this.okButton;
      this.ClientSize = new System.Drawing.Size(359, 115);
      this.Controls.Add(this.projectLinkLabel);
      this.Controls.Add(this.lblVersion);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.pictureBox1);
      this.Controls.Add(this.okButton);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AboutBox";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "About";
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label lblVersion;
    private System.Windows.Forms.LinkLabel projectLinkLabel;
  }
}
