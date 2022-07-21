namespace OpenHardwareMonitor.Hardware.Mainboard {
  internal class MainboardGroup : IGroup {

    private readonly Mainboard[] mainboards;

    public MainboardGroup(SMBIOS smbios, ISettings settings) {
      mainboards = new Mainboard[1];
      mainboards[0] = new Mainboard(smbios, settings);
    }

    public void Close() {
      foreach (Mainboard mainboard in mainboards)
        mainboard.Close();
    }

    public string GetReport() {
      return null;
    }

    public IHardware[] Hardware {
      get { return mainboards; }

    }
  }
}
