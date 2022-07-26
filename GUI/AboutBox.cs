using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace OpenHardwareMonitor.GUI {
  public partial class AboutBox : Form {
    public AboutBox() {
      InitializeComponent();
      this.Font = SystemFonts.MessageBoxFont;
      this.lblVersion.Text = "Version " + Application.ProductVersion;

      projectLinkLabel.Links.Remove(projectLinkLabel.Links[0]);
      projectLinkLabel.Links.Add(0, projectLinkLabel.Text.Length,
        "https://github.com/sergiye/openhardwaremonitor");
    }

    private void linkLabel_LinkClicked(object sender,
      LinkLabelLinkClickedEventArgs e) {
      try {
        Process.Start(new ProcessStartInfo(e.Link.LinkData.ToString()));
      } catch { }
    }

  }
}
