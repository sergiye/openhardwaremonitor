using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace OpenHardwareMonitor.GUI {
  public partial class AboutBox : Form {
    public AboutBox() {
      InitializeComponent();
      this.Font = SystemFonts.MessageBoxFont;
      this.label3.Text = "Version " + Application.ProductVersion;

      projectLinkLabel.Links.Remove(projectLinkLabel.Links[0]);
      projectLinkLabel.Links.Add(0, projectLinkLabel.Text.Length,
        "http://openhardwaremonitor.org");
    }

    private void linkLabel_LinkClicked(object sender,
      LinkLabelLinkClickedEventArgs e) {
      try {
        Process.Start(new ProcessStartInfo(e.Link.LinkData.ToString()));
      } catch { }
    }

  }
}
