using System.Collections.Generic;

namespace OpenHardwareMonitor.Hardware.HDD {
  internal class HarddriveGroup : IGroup {

    private const int MAX_DRIVES = 32;

    private readonly List<AbstractHarddrive> hardware = 
      new List<AbstractHarddrive>();

    public HarddriveGroup(ISettings settings) {
      if (OperatingSystem.IsUnix) 
        return;

      ISmart smart = new WindowsSmart();

      for (int drive = 0; drive < MAX_DRIVES; drive++) {
        AbstractHarddrive instance =
          AbstractHarddrive.CreateInstance(smart, drive, settings);
        if (instance != null) {
          this.hardware.Add(instance);
        }
      }
    }

    public IHardware[] Hardware {
      get {
        return hardware.ToArray();
      }
    }

    public string GetReport() {
      return null;
    }

    public void Close() {
      foreach (AbstractHarddrive hdd in hardware) 
        hdd.Close();
    }
  }
}
