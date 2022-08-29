using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using OpenHardwareMonitor.Hardware;

namespace OpenHardwareMonitor {

  public class PersistentSettings : ISettings {

    private IDictionary<string, string> settings = new Dictionary<string, string>();

    public void Load(string fileName) {
      if (!File.Exists(fileName)) return;
      try {
        var json = File.ReadAllText(fileName);
        settings = JsonConvert.DeserializeObject<IDictionary<string, string>>(json);
      }
      catch (Exception) {
        LoadOld(fileName);
      }
    }

    public void LoadOld(string fileName) {
      var doc = new XmlDocument();
      try {
        doc.Load(fileName);
      } catch {
        try {
          File.Delete(fileName);
        } catch { }

        var backupFileName = fileName + ".backup";
        try {
          doc.Load(backupFileName);
        } catch {
          try {
            File.Delete(backupFileName);
          } catch { }
          return;
        }
      }

      var list = doc.GetElementsByTagName("appSettings");
      foreach (XmlNode node in list) {
        var parent = node.ParentNode;
        if (parent != null && parent.Name == "configuration" &&
          parent.ParentNode is XmlDocument) {
          foreach (XmlNode child in node.ChildNodes) {
            if (child.Name == "add") {
              var attributes = child.Attributes;
              var keyAttribute = attributes["key"];
              var valueAttribute = attributes["value"];
              if (keyAttribute != null && valueAttribute != null &&
                keyAttribute.Value != null) {
                settings.Add(keyAttribute.Value, valueAttribute.Value);
              }
            }
          }
        }
      }
    }

    public void Save(string fileName) {
      var json = JsonConvert.SerializeObject(settings, Newtonsoft.Json.Formatting.Indented);
      File.WriteAllText(fileName, json);
    }

    public void SaveOld(string fileName) {

      var doc = new XmlDocument();
      doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));
      var configuration = doc.CreateElement("configuration");
      doc.AppendChild(configuration);
      var appSettings = doc.CreateElement("appSettings");
      configuration.AppendChild(appSettings);
      foreach (var keyValuePair in settings) {
        var add = doc.CreateElement("add");
        add.SetAttribute("key", keyValuePair.Key);
        add.SetAttribute("value", keyValuePair.Value);
        appSettings.AppendChild(add);
      }

      byte[] file;
      using (var memory = new MemoryStream()) {
        using (var writer = new StreamWriter(memory, Encoding.UTF8)) {
          doc.Save(writer);
        }
        file = memory.ToArray();
      }

      var backupFileName = fileName + ".backup";
      if (File.Exists(fileName)) {
        try {
          File.Delete(backupFileName);
        } catch { }
        try {
          File.Move(fileName, backupFileName);
        } catch { }
      }

      using (var stream = new FileStream(fileName,
        FileMode.Create, FileAccess.Write))
      {
        stream.Write(file, 0, file.Length);
      }

      try {
        File.Delete(backupFileName);
      } catch { }
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
