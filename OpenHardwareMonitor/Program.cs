using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using OpenHardwareMonitor.UI;

namespace OpenHardwareMonitor;

public static class Program
{
    static Mutex mutex = new Mutex(true, "{3661aef2-95dc-433e-932c-2e06112d24ec}");

    [STAThread]
    public static void Main()
    {
        if (!VersionCompatibitity.IsCompatible())
        {
            MessageBox.Show("The application is not compatible with your region.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Environment.Exit(0);
        }
        if (!mutex.WaitOne(TimeSpan.Zero, true))
        {
            foreach (var process in Process.GetProcesses())
            {
                if (process.ProcessName != Updater.ApplicationName) continue;
                process.Refresh();
                IntPtr hwnd = process.MainWindowHandle;
                if (hwnd == (IntPtr)0)
                    hwnd = NativeMethods.FindWindow(null, Updater.ApplicationTitle);
                if (hwnd != (IntPtr)0)
                {
                    NativeMethods.SendMessage(hwnd, NativeMethods.WM_USER + 1, 0, (IntPtr)0);
                    Environment.Exit(0);
                }
            }
            MessageBox.Show("Another instance of the application is already running.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            Environment.Exit(0);
        }

        Utilities.Crasher.Listen();

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        using (MainForm form = new MainForm())
        {
            form.FormClosed += delegate
            {
                Application.Exit();
            };
            Application.Run();
            mutex.ReleaseMutex();
        }
    }
}
