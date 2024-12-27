using System;
using System.IO;
using System.Security;
using System.Security.Principal;
using System.Windows.Forms;
using Microsoft.Win32;

namespace OpenHardwareMonitor.UI
{
    public class StartupManager
    {

        private bool _startup;
        private const string REGISTRY_RUN = @"Software\Microsoft\Windows\CurrentVersion\Run";

        private bool IsAdministrator()
        {
            try
            {
                var identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }

        public StartupManager()
        {
            if (Software.OperatingSystem.IsUnix)
            {
                IsAvailable = false;
                return;
            }

            if (IsAdministrator())
            {
                try
                {
                    var _scheduler = new TaskSchedulerClass();
                    _scheduler.Connect();

                    var folder = _scheduler.GetFolder("\\Open Hardware Monitor");
                    var task = folder.GetTask("Startup");
                    _startup = task != null &&
                               task.Definition.Triggers.Count > 0 &&
                               task.Definition.Triggers[1].Type == TASK_TRIGGER_TYPE2.TASK_TRIGGER_LOGON &&
                               task.Definition.Actions.Count > 0 &&
                               task.Definition.Actions[1].Type == TASK_ACTION_TYPE.TASK_ACTION_EXEC &&
                               task.Definition.Actions[1] is IExecAction execAction &&
                               execAction.Path == Application.ExecutablePath;

                    if (_startup)
                    {
                        //old versions compatibility - convert task to registry
                        DeleteSchedulerTask(_scheduler);
                        CreateRegistryRun();
                    }
                }
                catch
                {
                    _startup = false;
                }
            }

            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(REGISTRY_RUN))
                {
                    _startup = false;
                    if (key != null)
                    {
                        var value = (string)key.GetValue("OpenHardwareMonitor");
                        if (value != null)
                            _startup = value == Application.ExecutablePath;
                    }
                }
                IsAvailable = true;
            }
            catch (SecurityException)
            {
                IsAvailable = false;
            }
        }

        private static void DeleteSchedulerTask(TaskSchedulerClass scheduler)
        {
            var root = scheduler.GetFolder("\\");
            try
            {
                var folder = root.GetFolder("Open Hardware Monitor");
                folder.DeleteTask("Startup", 0);
            }
            catch (IOException) { }
            try
            {
                root.DeleteFolder("Open Hardware Monitor", 0);
            }
            catch (IOException) { }
        }

        private static void CreateRegistryRun()
        {
            var key = Registry.CurrentUser.CreateSubKey(REGISTRY_RUN);
            key?.SetValue("OpenHardwareMonitor", Application.ExecutablePath);
        }

        private static void DeleteRegistryRun()
        {
            var key = Registry.CurrentUser.CreateSubKey(REGISTRY_RUN);
            key?.DeleteValue("OpenHardwareMonitor");
        }

        public bool IsAvailable { get; }

        public bool Startup
        {
            get
            {
                return _startup;
            }
            set
            {
                if (_startup == value)
                    return;

                if (!IsAvailable)
                    throw new InvalidOperationException();

                try
                {
                    if (value)
                        CreateRegistryRun();
                    else
                        DeleteRegistryRun();
                    _startup = value;
                }
                catch (UnauthorizedAccessException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
