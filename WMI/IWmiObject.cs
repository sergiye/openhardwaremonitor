namespace OpenHardwareMonitor.WMI {
  interface IWmiObject {
    // Both of these get exposed to WMI
    string Name { get; }
    string Identifier { get; }

    // Not exposed.
    void Update();
  }
}
