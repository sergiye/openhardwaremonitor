using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenHardwareMonitor.Hardware;
using OpenHardwareMonitor.Utilities;

namespace OpenHardwareMonitor.GUI {
  public class SystemTray : IDisposable {
    // private IComputer computer;
    private readonly PersistentSettings settings;
    private readonly UnitManager unitManager;
    private readonly List<SensorNotifyIcon> list = new List<SensorNotifyIcon>();
    private bool mainIconEnabled;
    private readonly NotifyIconAdv mainIcon;

    public SystemTray(IComputer computer, PersistentSettings settings,
      UnitManager unitManager)
    {
      // this.computer = computer;
      this.settings = settings;
      this.unitManager = unitManager;
      computer.HardwareAdded += HardwareAdded;
      computer.HardwareRemoved += HardwareRemoved;

      mainIcon = new NotifyIconAdv();

      var contextMenu = new ContextMenu();
      var hideShowItem = new MenuItem("Hide/Show");
      hideShowItem.DefaultItem = true;
      hideShowItem.Click += (obj, args) => SendHideShowCommand();
      contextMenu.MenuItems.Add(hideShowItem);
      contextMenu.MenuItems.Add("-");
      var exitItem = new MenuItem("Exit");
      exitItem.Click += (obj, args) => SendExitCommand();
      contextMenu.MenuItems.Add(exitItem);
      mainIcon.ContextMenu = contextMenu;
      mainIcon.DoubleClick += (obj, args) => SendHideShowCommand();
      mainIcon.Icon = EmbeddedResources.GetIcon("smallicon.ico");
      mainIcon.Text = "Open Hardware Monitor";
    }

    private void HardwareRemoved(IHardware hardware) {
      hardware.SensorAdded -= SensorAdded;
      hardware.SensorRemoved -= SensorRemoved;
      foreach (var sensor in hardware.Sensors)
        SensorRemoved(sensor);
      foreach (var subHardware in hardware.SubHardware)
        HardwareRemoved(subHardware);
    }

    private void HardwareAdded(IHardware hardware) {
      foreach (var sensor in hardware.Sensors)
        SensorAdded(sensor);
      hardware.SensorAdded += SensorAdded;
      hardware.SensorRemoved += SensorRemoved;
      foreach (var subHardware in hardware.SubHardware)
        HardwareAdded(subHardware);
    }

    private void SensorAdded(ISensor sensor) {
      if (settings.GetValue(new Identifier(sensor.Identifier,
        "tray").ToString(), false))
        Add(sensor, false);
    }

    private void SensorRemoved(ISensor sensor) {
      if (Contains(sensor))
        Remove(sensor, false);
    }

    public void Dispose() {
      foreach (var icon in list)
        icon.Dispose();
      mainIcon.Dispose();
    }

    public void Redraw() {
      foreach (var icon in list)
        icon.Update();
    }

    public bool Contains(ISensor sensor) {
      foreach (var icon in list)
        if (icon.Sensor == sensor)
          return true;
      return false;
    }

    public void Add(ISensor sensor, bool balloonTip) {
      if (Contains(sensor))
        return;

      list.Add(new SensorNotifyIcon(this, sensor, settings, unitManager));
      UpdateMainIconVisibilty();
      settings.SetValue(new Identifier(sensor.Identifier, "tray").ToString(), true);
    }

    public void Remove(ISensor sensor) {
      Remove(sensor, true);
    }

    private void Remove(ISensor sensor, bool deleteConfig) {
      if (deleteConfig) {
        settings.Remove(
          new Identifier(sensor.Identifier, "tray").ToString());
        settings.Remove(
          new Identifier(sensor.Identifier, "traycolor").ToString());
      }
      SensorNotifyIcon instance = null;
      foreach (var icon in list)
        if (icon.Sensor == sensor)
          instance = icon;
      if (instance != null) {
        list.Remove(instance);
        UpdateMainIconVisibilty();
        instance.Dispose();
      }
    }

    public event EventHandler HideShowCommand;

    public void SendHideShowCommand() {
      if (HideShowCommand != null)
        HideShowCommand(this, null);
    }

    public event EventHandler ExitCommand;

    public void SendExitCommand() {
      if (ExitCommand != null)
        ExitCommand(this, null);
    }

    private void UpdateMainIconVisibilty() {
      if (mainIconEnabled) {
        mainIcon.Visible = list.Count == 0;
      } else {
        mainIcon.Visible = false;
      }
    }

    public bool IsMainIconEnabled {
      get => mainIconEnabled;
      set {
        if (mainIconEnabled == value) return;
        mainIconEnabled = value;
        UpdateMainIconVisibilty();
      }
    }
  }
}
