namespace OpenHardwareMonitor.Hardware.RAM {
  internal class RAMGroup : IGroup {

    private Hardware[] hardware;

    public RAMGroup(SMBIOS smbios, ISettings settings) {

      // No implementation for RAM on Unix systems
      if (OperatingSystem.IsUnix) {
        hardware = new Hardware[0];
        return;
      }

      hardware = new Hardware[] { new GenericRAM("Generic Memory", settings) };
    }

    public string GetReport() {
      return null;
    }

    public IHardware[] Hardware {
      get {
        return hardware;
      }
    }

    public void Close() {
      foreach (Hardware ram in hardware)
        ram.Close();
    }
  }
}
