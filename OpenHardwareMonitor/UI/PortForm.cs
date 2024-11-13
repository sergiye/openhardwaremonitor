using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace OpenHardwareMonitor.UI;

public partial class PortForm : Form
{
    private readonly MainForm _parent;
    private readonly string _localIP;
    public PortForm(MainForm m)
    {
        InitializeComponent();
        Icon = System.Drawing.Icon.ExtractAssociatedIcon(Utilities.Updater.CurrentFileLocation);
        _parent = m;
        _localIP = GetLocalIP();
    }

    private void PortTextBox_TextChanged(object sender, EventArgs e)
    {

    }

    private string GetLocalIP()
    {
        IPHostEntry host;
        string localIP = "?";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
                localIP = ip.ToString();
        }
        return localIP;
    }

    private void PortNumericUpDn_ValueChanged(object sender, EventArgs e)
    {
        string url = "http://" + _localIP + ":" + portNumericUpDn.Value + "/";
        webServerLinkLabel.Text = url;
        webServerLinkLabel.Links.Remove(webServerLinkLabel.Links[0]);
        webServerLinkLabel.Links.Add(0, webServerLinkLabel.Text.Length, url);
    }

    private void PortOKButton_Click(object sender, EventArgs e)
    {
        _parent.Server.ListenerPort = (int)portNumericUpDn.Value;
        Close();
    }

    private void PortCancelButton_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void PortForm_Load(object sender, EventArgs e)
    {
        portNumericUpDn.Value = _parent.Server.ListenerPort;
        PortNumericUpDn_ValueChanged(null, null);
    }

    private void WebServerLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            Process.Start(new ProcessStartInfo(e.Link.LinkData.ToString()));
        }
        catch { }
    }

    private void PortNumericUpDn_KeyUp(object sender, KeyEventArgs e)
    {
        PortNumericUpDn_ValueChanged(null, null);
    }
}
