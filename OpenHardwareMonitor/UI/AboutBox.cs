using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using OpenHardwareMonitor.Utilities;
using sergiye.Common;

namespace OpenHardwareMonitor.UI;

public sealed partial class AboutBox : Form
{
    public AboutBox()
    {
        InitializeComponent();
        lblAppTitle.Text = Updater.ApplicationTitle;
        lblCopyright.Text = $"Copyright © {Updater.ApplicationName}";
        Font = SystemFonts.MessageBoxFont;
        lblVersion.Text = $"Version {Application.ProductVersion} {(Environment.Is64BitProcess ? "x64" : "x32")}";
        //pictureBox1.Image = Icon.ExtractAssociatedIcon(Updater.CurrentFileLocation).ToBitmap();
        picLogo.Image = EmbeddedResources.GetIcon("icon.ico").ToBitmap();
        picLogo.SizeMode = PictureBoxSizeMode.StretchImage;
        projectLinkLabel.Links.Remove(projectLinkLabel.Links[0]);
        projectLinkLabel.Links.Add(0, projectLinkLabel.Text.Length, $"https://github.com/{Updater.ApplicationCompany}/{Updater.ApplicationName}");
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
