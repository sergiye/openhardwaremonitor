using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using OpenHardwareMonitor.GUI;

namespace OpenHardwareMonitor {
  public static class Program {

    static Mutex mutex = new Mutex(true, "{3661aef2-95dc-433e-932c-2e06112d24ec}");

    [STAThread]
    public static void Main() {

      if (!IsNetFrameworkInstalled())
        Environment.Exit(0);

      if (!mutex.WaitOne(TimeSpan.Zero, true)) {
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
      using (var form = new MainForm()) {
        form.FormClosed += (sender, e) => Application.Exit();
        Application.Run();
        mutex.ReleaseMutex();
      }
    }

    private static bool IsNetFrameworkInstalled() {
      Type type;
      try {
        type = TryGetDefaultDllImportSearchPathsAttributeType();
      } catch (TypeLoadException) {
        MessageBox.Show(
          "This application requires the .NET Framework 4.8 or a later version.\n" +
          "Please install the latest .NET Framework. For more information, see\n\n" +
          "https://dotnet.microsoft.com/download/dotnet-framework",
          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      return type != null;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Type TryGetDefaultDllImportSearchPathsAttributeType() {
      return typeof(DefaultDllImportSearchPathsAttribute);
    }

    private static void ReportException(Exception e) {
      var form = new CrashForm {
        Exception = e
      };
      form.ShowDialog();
    }

    public static void Application_ThreadException(object sender,
      ThreadExceptionEventArgs e) {
      try {
        ReportException(e.Exception);
      } catch {
      } finally {
        Application.Exit();
      }
    }

    public static void CurrentDomain_UnhandledException(object sender,
      UnhandledExceptionEventArgs args) {
      try {
        if (args.ExceptionObject is Exception e)
          ReportException(e);
      } catch {
      } finally {
        Environment.Exit(0);
      }
    }
  }
}
