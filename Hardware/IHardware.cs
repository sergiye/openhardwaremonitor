namespace OpenHardwareMonitor.Hardware {

  public delegate void SensorEventHandler(ISensor sensor);
  
  public enum HardwareType {
    Mainboard,
    SuperIO,
    CPU,
    RAM,
    GpuNvidia,
    GpuAti,    
    TBalancer,
    Heatmaster,
    HDD
  }

  public interface IHardware : IElement {

    string Name { get; set; }
    Identifier Identifier { get; }

    HardwareType HardwareType { get; }

    string GetReport();

    void Update();

    IHardware[] SubHardware { get; }

    IHardware Parent { get; }

    ISensor[] Sensors { get; }

    event SensorEventHandler SensorAdded;
    event SensorEventHandler SensorRemoved;
  }
}
