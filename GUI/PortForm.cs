using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using OpenHardwareMonitor.Utilities;

namespace OpenHardwareMonitor.GUI {
  public partial class PortForm : Form {
    private readonly HttpServer server;
    private readonly string localIP;

    public PortForm(HttpServer server) {
      InitializeComponent();
      this.server = server;
      localIP = getLocalIP();
    }

    private string getLocalIP() {
      IPHostEntry host;
      var localIP = "?";
      host = Dns.GetHostEntry(Dns.GetHostName());
      foreach (var ip in host.AddressList) {
        if (ip.AddressFamily == AddressFamily.InterNetwork) {
          localIP = ip.ToString();
        }
      }
      return localIP;
    }

    private void portNumericUpDn_ValueChanged(object sender, EventArgs e) {
      var url = "http://" + localIP + ":" + portNumericUpDn.Value + "/";
      webServerLinkLabel.Text = url;
      webServerLinkLabel.Links.Remove(webServerLinkLabel.Links[0]);
      webServerLinkLabel.Links.Add(0, webServerLinkLabel.Text.Length, url);
    }

    private void portOKButton_Click(object sender, EventArgs e) {
      server.ListenerPort = (int)portNumericUpDn.Value;
    }

    private void PortForm_Load(object sender, EventArgs e) {
      portNumericUpDn.Value = server.ListenerPort;
      portNumericUpDn_ValueChanged(null, null);
    }

    private void webServerLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      try {
        Process.Start(new ProcessStartInfo(e.Link.LinkData.ToString()));
      } catch { }
    }

  }
}
