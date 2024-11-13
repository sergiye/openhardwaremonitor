using System;
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
        if (!mutex.WaitOne(TimeSpan.Zero, true))
        {
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
