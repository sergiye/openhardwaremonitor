using System;
using System.Text;
using System.Windows.Forms;

namespace OpenHardwareMonitor.GUI {

  public partial class CrashForm : Form {

    private Exception exception;

    public CrashForm() {
      InitializeComponent();
    }

    public Exception Exception {
      get { return exception; }
      set {
        exception = value;
        var s = new StringBuilder();
        var version = typeof(CrashForm).Assembly.GetName().Version;
        s.Append("Version: "); s.AppendLine(version.ToString());        
        s.AppendLine();
        s.AppendLine(exception.ToString());
        s.AppendLine();
        var innerException = exception.InnerException;
        while (innerException != null) {
          s.AppendLine(innerException.ToString());
          s.AppendLine();
          innerException = innerException.InnerException;
        }
        s.Append("Common Language Runtime: "); 
        s.AppendLine(Environment.Version.ToString());
        s.Append("Operating System: ");
        s.AppendLine(Environment.OSVersion.ToString());
        s.Append("Process Type: ");
        s.AppendLine(IntPtr.Size == 4 ? "32-Bit" : "64-Bit");
        reportTextBox.Text = s.ToString();        
      }
    }
  }  
}
