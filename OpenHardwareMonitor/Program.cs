using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using OpenHardwareMonitor.UI;

namespace OpenHardwareMonitor;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        Crasher.Listen();

        if (!OperatingSystemHelper.IsCompatible(false, out var errorMessage, out var fixAction))
        {
            if (fixAction != null)
            {
                if (MessageBox.Show(errorMessage, Updater.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    fixAction?.Invoke();
                }
            }
            else
            {
                MessageBox.Show(errorMessage, Updater.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Environment.Exit(0);
        }

        if (WinApiHelper.CheckRunningInstances(true, true))
        {
            // fallback
            MessageBox.Show($"{Updater.ApplicationName} is already running.", Updater.ApplicationName,
              MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
        }

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        using (MainForm form = new MainForm())
        {
            form.FormClosed += delegate
            {
                Application.Exit();
            };
            Application.Run();
        }
    }
}
