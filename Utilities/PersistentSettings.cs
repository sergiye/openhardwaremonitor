using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using OpenHardwareMonitor.Hardware;

namespace OpenHardwareMonitor {

  public class PersistentSettings : ISettings {

    private readonly string configFilePath;

    public PersistentSettings() {
      configFilePath = Path.ChangeExtension(Application.ExecutablePath, ".config");
    }

    private IDictionary<string, string> settings = new Dictionary<string, string>();

    public void Load() {

      //old versions configs compatibility
      if (File.Exists(configFilePath)) {
        try {
          var json = File.ReadAllText(configFilePath);
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

    public void Save() {

      if (GetValue("portable", false)) {
        SaveToFile(configFilePath);
        return;
      }

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

        if (File.Exists(configFilePath)) {
          try {
            File.Delete(configFilePath);
          } catch (Exception) {
            //ignore
          }
        }
      } catch (Exception) {
        //save old-style config
        SaveToFile(configFilePath);
      }
    }

    public void SaveToFile(string configFilePath) {
      try {
        settings.ToJsonFile(configFilePath);
      } catch (UnauthorizedAccessException) {
        MessageBox.Show("Access to the path '" + configFilePath + "' is denied. " +
          "The current settings could not be saved.",
          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      } catch (IOException) {
        MessageBox.Show("The path '" + configFilePath + "' is not writeable. " +
          "The current settings could not be saved.",
          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    public bool Contains(string name) {
      return settings.ContainsKey(name);
    }

    public void SetValue(string name, string value) {
      settings[name] = value;
      Save();
    }

    public string GetValue(string name, string defaultValue) {
      return settings.TryGetValue(name, out var result) ? result : defaultValue;
    }

    public void Remove(string name) {
      settings.Remove(name);
      Save();
    }

    public void SetValue(string name, int value) {
      settings[name] = value.ToString();
      Save();
    }

    public int GetValue(string name, int defaultValue) {
      if (settings.TryGetValue(name, out var str) && int.TryParse(str, out var parsedValue))
        return parsedValue;
      return defaultValue;
    }

    public void SetValue(string name, float value) {
      settings[name] = value.ToString(CultureInfo.InvariantCulture);
      Save();
    }

    public float GetValue(string name, float defaultValue) {
      if (settings.TryGetValue(name, out var str) && float.TryParse(str, NumberStyles.Float,
        CultureInfo.InvariantCulture, out var parsedValue))
        return parsedValue;
      return defaultValue;
    }

    public void SetValue(string name, bool value) {
      settings[name] = value ? "true" : "false";
      Save();
    }

    public bool GetValue(string name, bool defaultValue) {
      if (settings.TryGetValue(name, out var str))
        return str == "true";
      return defaultValue;
    }

    public void SetValue(string name, Color color) {
      settings[name] = color.ToArgb().ToString("X8");
      Save();
    }

    public Color GetValue(string name, Color defaultValue) {
      if (settings.TryGetValue(name, out var str) && int.TryParse(str, NumberStyles.HexNumber,
        CultureInfo.InvariantCulture, out var parsedValue))
        return Color.FromArgb(parsedValue);
      return defaultValue;
    }
  }
}
