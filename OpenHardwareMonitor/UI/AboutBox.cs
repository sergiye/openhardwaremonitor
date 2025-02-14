using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using OpenHardwareMonitor.Utilities;

namespace OpenHardwareMonitor.UI;

public sealed partial class AboutBox : Form
{
    public AboutBox()
    {
        InitializeComponent();
        Font = SystemFonts.MessageBoxFont;
        label3.Text = "Version " + Application.ProductVersion;
        //pictureBox1.Image = Icon.ExtractAssociatedIcon(Updater.CurrentFileLocation).ToBitmap();
        pictureBox1.Image = EmbeddedResources.GetIcon("icon.ico").ToBitmap();
        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        projectLinkLabel.Links.Remove(projectLinkLabel.Links[0]);
        projectLinkLabel.Links.Add(0, projectLinkLabel.Text.Length, "https://github.com/sergiye/openhardwaremonitor");
        licenseLinkLabel.Links.Remove(licenseLinkLabel.Links[0]);
        licenseLinkLabel.Links.Add(0, licenseLinkLabel.Text.Length, "https://www.mozilla.org/en-US/MPL/2.0/");
        Theme.Current.Apply(this);
    }

    private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            Process.Start(new ProcessStartInfo(e.Link.LinkData.ToString()));
        }
        catch { }
    }
}
