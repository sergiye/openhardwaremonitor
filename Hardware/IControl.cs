namespace OpenHardwareMonitor.Hardware {

  public enum ControlMode {
    Undefined,
    Software,
    Default
  }

  public interface IControl {

    Identifier Identifier { get; }

    ControlMode ControlMode { get; }

    float SoftwareValue { get; }

    void SetDefault();

    float MinSoftwareValue { get; }
    float MaxSoftwareValue { get; }

    void SetSoftware(float value);

  }
}
