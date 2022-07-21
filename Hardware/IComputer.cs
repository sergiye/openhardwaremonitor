namespace OpenHardwareMonitor.Hardware {

  public delegate void HardwareEventHandler(IHardware hardware);

  public interface IComputer : IElement {

    IHardware[] Hardware { get; }

    bool MainboardEnabled { get; }
    bool CPUEnabled { get; }
    bool RAMEnabled { get; }
    bool GPUEnabled { get; }
    bool FanControllerEnabled { get; }
    bool HDDEnabled { get; }


    string GetReport();

    event HardwareEventHandler HardwareAdded;
    event HardwareEventHandler HardwareRemoved;
  }
}
