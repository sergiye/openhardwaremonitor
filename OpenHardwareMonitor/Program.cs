// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System;
using System.Threading;
using System.Windows.Forms;
using OpenHardwareMonitor.UI;
using System.Reflection;

[assembly: AssemblyTitle("Open Hardware Monitor")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Sergiy Egoshyn")]
[assembly: AssemblyProduct("Open Hardware Monitor")]
[assembly: AssemblyCopyright("Copyright © 2022 Sergiy Egoshyn")]
[assembly: AssemblyVersion("2024.10.*")]

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

#if !DEBUG
      Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
      Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
      AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
#endif

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

    private static void ReportException(Exception e)
    {
        //var form = new CrashForm
        //{
        //    Exception = e
        //};
        //form.ShowDialog();
    }

    public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
        try
        {
            ReportException(e.Exception);
        }
        catch
        {
        }
        finally
        {
            Application.Exit();
        }
    }

    public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
    {
        try
        {
            if (args.ExceptionObject is Exception e)
                ReportException(e);
        }
        catch
        {
        }
        finally
        {
            Environment.Exit(0);
        }
    }
}
