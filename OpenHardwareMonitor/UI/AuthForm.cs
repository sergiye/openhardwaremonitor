// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System;
using System.Windows.Forms;

namespace OpenHardwareMonitor.UI;

public partial class AuthForm : Form
{
    private readonly MainForm _parent;

    public AuthForm(MainForm m)
    {
        InitializeComponent();
        Icon = System.Drawing.Icon.ExtractAssociatedIcon(Utilities.Updater.CurrentFileLocation);
        _parent = m;
    }

    private void AuthForm_Load(object sender, EventArgs e)
    {
        httpAuthUsernameTextBox.Enabled = httpAuthPasswordTextBox.Enabled = enableHTTPAuthCheckBox.Checked = _parent.Server.AuthEnabled;
        httpAuthUsernameTextBox.Text = _parent.Server.UserName;
    }

    private void HttpAuthCancelButton_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void HttpAuthOkButton_Click(object sender, EventArgs e)
    {
        _parent.Server.UserName = httpAuthUsernameTextBox.Text;
        _parent.Server.Password = httpAuthPasswordTextBox.Text;
        _parent.Server.AuthEnabled = enableHTTPAuthCheckBox.Checked;
        _parent.AuthWebServerMenuItemChecked = _parent.Server.AuthEnabled;
        Close();
    }

    private void EnableHTTPAuthCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        httpAuthUsernameTextBox.Enabled = httpAuthPasswordTextBox.Enabled = enableHTTPAuthCheckBox.Checked;
    }
}
