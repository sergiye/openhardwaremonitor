﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Windows.Forms;
using Microsoft.Win32;

namespace OpenHardwareMonitor.UI
{
    public class StartupManager
    {

        private readonly TaskSchedulerClass _scheduler;
        private bool _startup;
        private const string REGISTRY_RUN = @"Software\Microsoft\Windows\CurrentVersion\Run";

        private bool IsAdministrator()
        {
            try
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
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
                _scheduler = null;
                IsAvailable = false;
                return;
            }

            if (IsAdministrator())
            {
                try
                {
                    _scheduler = new TaskSchedulerClass();
                    _scheduler.Connect(null, null, null, null);
                }
                catch
                {
                    _scheduler = null;
                }

                if (_scheduler != null)
                {
                    try
                    {
                        try
                        {
                            // check if the taskscheduler is running
                            IRunningTaskCollection collection = _scheduler.GetRunningTasks(0);
                        }
                        catch (ArgumentException) { }

                        ITaskFolder folder = _scheduler.GetFolder("\\Open Hardware Monitor");
                        IRegisteredTask task = folder.GetTask("Startup");
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
                            DeleteSchedulerTask();
                            CreateRegistryRun();
                        }
                    }
                    catch (IOException)
                    {
                        _startup = false;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        _scheduler = null;
                    }
                    catch (COMException)
                    {
                        _scheduler = null;
                    }
                    catch (NotImplementedException)
                    {
                        _scheduler = null;
                    }
                }
            }
            else
            {
                _scheduler = null;
            }

            if (_scheduler == null)
            {
                try
                {
                    using (RegistryKey key =
                      Registry.CurrentUser.OpenSubKey(REGISTRY_RUN))
                    {
                        _startup = false;
                        if (key != null)
                        {
                            string value = (string)key.GetValue("OpenHardwareMonitor");
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
            else
            {
                IsAvailable = true;
            }
        }

        private void DeleteSchedulerTask()
        {
            ITaskFolder root = _scheduler.GetFolder("\\");
            try
            {
                ITaskFolder folder = root.GetFolder("Open Hardware Monitor");
                folder.DeleteTask("Startup", 0);
            }
            catch (IOException) { }
            try
            {
                root.DeleteFolder("Open Hardware Monitor", 0);
            }
            catch (IOException) { }
        }

        private void CreateRegistryRun()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(REGISTRY_RUN);
            key.SetValue("OpenHardwareMonitor", Application.ExecutablePath);
        }

        private void DeleteRegistryRun()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(REGISTRY_RUN);
            key.DeleteValue("OpenHardwareMonitor");
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
