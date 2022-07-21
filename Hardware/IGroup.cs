namespace OpenHardwareMonitor.Hardware {

  internal interface IGroup {

    IHardware[] Hardware { get; }

    string GetReport();

    void Close();
  }

}
