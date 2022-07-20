using System;
using System.Security;
using System.Windows.Forms;
using Microsoft.Win32;

namespace OpenHardwareMonitor.GUI {
  
  public class StartupManager {

    private bool startup;

    private const string REGISTRY_RUN = @"Software\Microsoft\Windows\CurrentVersion\Run";

    // private bool IsAdministrator() {
    //   try {
    //     WindowsIdentity identity = WindowsIdentity.GetCurrent();
    //     WindowsPrincipal principal = new WindowsPrincipal(identity);
    //     return principal.IsInRole(WindowsBuiltInRole.Administrator);
    //   } catch {
    //     return false;
    //   }
    // }

    public StartupManager() {
      if (Hardware.OperatingSystem.IsUnix) {
        IsAvailable = false;
        return;
      }

      try {
        using (var key = Registry.CurrentUser.OpenSubKey(REGISTRY_RUN)) {
          startup = false;
          var value = (string) key?.GetValue("OpenHardwareMonitor");
          if (value != null)
            startup = value == Application.ExecutablePath;
        }
        IsAvailable = true;
      } catch (SecurityException) {
        IsAvailable = false;
      }
    }

    private static void CreateRegistryRun() {
      var key = Registry.CurrentUser.CreateSubKey(REGISTRY_RUN);
      key?.SetValue("OpenHardwareMonitor", Application.ExecutablePath);
    }

    private static void DeleteRegistryRun() {
      var key = Registry.CurrentUser.CreateSubKey(REGISTRY_RUN);
      key?.DeleteValue("OpenHardwareMonitor");
    }

    public bool IsAvailable { get; }

    public bool Startup {
      get => startup;
      set {
        if (startup == value) return;
        if (!IsAvailable)
          throw new InvalidOperationException();

        try {
          if (value)
            CreateRegistryRun();
          else
            DeleteRegistryRun();
          startup = value;
        }
        catch (UnauthorizedAccessException) {
          throw new InvalidOperationException();
        }
      }
    }
  }

}
