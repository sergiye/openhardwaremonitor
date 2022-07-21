namespace OpenHardwareMonitor.Hardware {
  public interface ISettings {

    bool Contains(string name);
    
    void SetValue(string name, string value);
    
    string GetValue(string name, string value);
    
    void Remove(string name);
  }
}
