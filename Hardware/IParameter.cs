namespace OpenHardwareMonitor.Hardware {

  public interface IParameter : IElement {

    ISensor Sensor { get; }
    Identifier Identifier { get; }
    
    string Name { get; }
    string Description { get; }
    float Value { get; set; }
    float DefaultValue { get; }
    bool IsDefault { get; set; }
  }
}
