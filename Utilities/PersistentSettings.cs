using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using Microsoft.Win32;
using OpenHardwareMonitor.Hardware;

namespace OpenHardwareMonitor {

  public class PersistentSettings : ISettings {

    private IDictionary<string, string> settings = new Dictionary<string, string>();

    public void Load(string fileName) {

      //old versions configs compatibility
      if (File.Exists(fileName)) {
        try {
          var json = File.ReadAllText(fileName);
          settings = json.FromJson<IDictionary<string, string>>();
          return;
        }
        catch (Exception) {
        }
      }

      //read from registry
      using (var reg = Registry.CurrentUser.OpenSubKey("Software\\sergiye\\openHardwareMonitor")) {
        if (reg == null) return;
        foreach (var key in reg.GetValueNames()) {
          var value = reg.GetValue(key, null) as string;
          settings.Add(key, value);
        }
      }
    }

    public void Save(string fileName) {

      try {
        //remove prev settings
        Registry.CurrentUser.DeleteSubKeyTree("Software\\sergiye\\openHardwareMonitor", false);
        //save to registry
        using (var reg = Registry.CurrentUser.CreateSubKey("Software\\sergiye\\openHardwareMonitor")) {
          if (reg == null) return;
          foreach (var p in settings) {
            reg.SetValue(p.Key, p.Value);
          }
        }

        if (File.Exists(fileName)) {
          try {
            File.Delete(fileName);
          } catch (Exception) {
            //ignore
          }
        }
      } catch (Exception) {
        //save old-style config
        settings.ToJsonFile(fileName);
      }
    }

    public bool Contains(string name) {
      return settings.ContainsKey(name);
    }

    public void SetValue(string name, string value) {
      settings[name] = value;
    }

    public string GetValue(string name, string defaultValue) {
      return settings.TryGetValue(name, out var result) ? result : defaultValue;
    }

    public void Remove(string name) {
      settings.Remove(name);
    }

    public void SetValue(string name, int value) {
      settings[name] = value.ToString();
    }

    public int GetValue(string name, int defaultValue) {
      if (settings.TryGetValue(name, out var str) && int.TryParse(str, out var parsedValue))
        return parsedValue;
      return defaultValue;
    }

    public void SetValue(string name, float value) {
      settings[name] = value.ToString(CultureInfo.InvariantCulture);
    }

    public float GetValue(string name, float defaultValue) {
      if (settings.TryGetValue(name, out var str) && float.TryParse(str, NumberStyles.Float,
        CultureInfo.InvariantCulture, out var parsedValue))
        return parsedValue;
      return defaultValue;
    }

    public void SetValue(string name, bool value) {
      settings[name] = value ? "true" : "false";
    }

    public bool GetValue(string name, bool defaultValue) {
      if (settings.TryGetValue(name, out var str))
        return str == "true";
      return defaultValue;
    }

    public void SetValue(string name, Color color) {
      settings[name] = color.ToArgb().ToString("X8");
    }

    public Color GetValue(string name, Color defaultValue) {
      if (settings.TryGetValue(name, out var str) && int.TryParse(str, NumberStyles.HexNumber,
        CultureInfo.InvariantCulture, out var parsedValue))
        return Color.FromArgb(parsedValue);
      return defaultValue;
    }
  }
}
