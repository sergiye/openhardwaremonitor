using System;
using System.Collections.Generic;
using System.Management.Instrumentation;
using OpenHardwareMonitor.Hardware;

[assembly: Instrumented("root/OpenHardwareMonitor")]

[System.ComponentModel.RunInstaller(true)]
public class InstanceInstaller : DefaultManagementProjectInstaller { }

namespace OpenHardwareMonitor.WMI {
  /// <summary>
  /// The WMI Provider.
  /// This class is not exposed to WMI itself.
  /// </summary>
  public class WmiProvider : IDisposable {
    private List<IWmiObject> activeInstances;

    public WmiProvider(IComputer computer) {
      activeInstances = new List<IWmiObject>();

      foreach (IHardware hardware in computer.Hardware)
        ComputerHardwareAdded(hardware);

      computer.HardwareAdded += ComputerHardwareAdded;
      computer.HardwareRemoved += ComputerHardwareRemoved;
    }

    public void Update() {
      foreach (IWmiObject instance in activeInstances)
        instance.Update();
    }

    #region Eventhandlers
    
    private void ComputerHardwareAdded(IHardware hardware) {
      if (!Exists(hardware.Identifier.ToString())) {
        foreach (ISensor sensor in hardware.Sensors)
          HardwareSensorAdded(sensor);

        hardware.SensorAdded += HardwareSensorAdded;
        hardware.SensorRemoved += HardwareSensorRemoved;

        Hardware hw = new Hardware(hardware);
        activeInstances.Add(hw);

        try {
          Instrumentation.Publish(hw);
        } catch (Exception) { }
      }

      foreach (IHardware subHardware in hardware.SubHardware)
        ComputerHardwareAdded(subHardware);
    }

    private void HardwareSensorAdded(ISensor data) {
      Sensor sensor = new Sensor(data);
      activeInstances.Add(sensor);

      try {
        Instrumentation.Publish(sensor);
      } catch (Exception) { }
    }

    private void ComputerHardwareRemoved(IHardware hardware) {
      hardware.SensorAdded -= HardwareSensorAdded;
      hardware.SensorRemoved -= HardwareSensorRemoved;
      
      foreach (ISensor sensor in hardware.Sensors) 
        HardwareSensorRemoved(sensor);
      
      foreach (IHardware subHardware in hardware.SubHardware)
        ComputerHardwareRemoved(subHardware);

      RevokeInstance(hardware.Identifier.ToString());
    }

    private void HardwareSensorRemoved(ISensor sensor) {
      RevokeInstance(sensor.Identifier.ToString());
    }

    #endregion

    #region Helpers
    
    private bool Exists(string identifier) {
      return activeInstances.Exists(h => h.Identifier == identifier);
    }

    private void RevokeInstance(string identifier) {
      int instanceIndex = activeInstances.FindIndex(
        item => item.Identifier == identifier.ToString()
      );

      if (instanceIndex == -1)
        return;

      try {
        Instrumentation.Revoke(activeInstances[instanceIndex]);
      } catch (Exception) { }

      activeInstances.RemoveAt(instanceIndex);
    }

    #endregion

    public void Dispose() {
      foreach (IWmiObject instance in activeInstances) {
        try {
          Instrumentation.Revoke(instance);
        } catch (Exception) { }
      }
      activeInstances = null;
    }
  }
}
