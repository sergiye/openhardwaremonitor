using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using OpenHardwareMonitor.Hardware;
using Microsoft.Win32;
using sergiye.Common;

namespace OpenHardwareMonitor.Utilities;

public class PersistentSettings : ISettings
{
    private readonly string configFilePath;
    private IDictionary<string, string> _settings = new Dictionary<string, string>();

    public PersistentSettings()
    {
        configFilePath = Path.ChangeExtension(Application.ExecutablePath, ".config");
    }

    public void Load()
    {
        //old versions configs compatibility
        if (File.Exists(configFilePath))
        {
            try
            {
                var json = File.ReadAllText(configFilePath);
                var data = json.FromJson<IDictionary<string, string>>();
                if (data != null)
                {
                    _settings = data;
                    _isPortable = true;
                    return;
                }
            }
            catch (Exception)
            {
            }
        }

        //read from registry
        using (var reg = Registry.CurrentUser.OpenSubKey("Software\\sergiye\\openHardwareMonitor"))
        {
            if (reg == null) return;
            foreach (var key in reg.GetValueNames())
            {
                var value = reg.GetValue(key, null) as string;
                _settings.Add(key, value);
            }
        }
    }

    public void Save()
    {
        //remove prev registry settings
        Registry.CurrentUser.DeleteSubKeyTree("Software\\sergiye\\openHardwareMonitor", false);

        if (IsPortable)
        {
            SaveToFile(configFilePath);
            return;
        }

        try
        {
            //save to registry
            using (var reg = Registry.CurrentUser.CreateSubKey("Software\\sergiye\\openHardwareMonitor"))
            {
                if (reg == null) return;
                foreach (var p in _settings)
                {
                    reg.SetValue(p.Key, p.Value);
                }
            }

            if (File.Exists(configFilePath))
            {
                try
                {
                    File.Delete(configFilePath);
                }
                catch (Exception)
                {
                    //ignore
                }
            }
        }
        catch (Exception)
        {
            SaveToFile(configFilePath);
        }
    }

    public void SaveToFile(string configFilePath)
    {
        try
        {
            _settings.ToJsonFile(configFilePath);
        }
        catch (UnauthorizedAccessException)
        {
            MessageBox.Show("Access to the path '" + configFilePath + "' is denied. " +
              "The current settings could not be saved.",
              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (IOException)
        {
            MessageBox.Show("The path '" + configFilePath + "' is not writeable. " +
              "The current settings could not be saved.",
              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private bool _isPortable;
    public bool IsPortable
    {
        get
        {
            return _isPortable;
        }
        set
        {
            if (_isPortable == value) return;
            _isPortable = value;
            Save();
        }
    }

    public bool Contains(string name)
    {
        return _settings.ContainsKey(name);
    }

    public void SetValue(string name, string value)
    {
        if (_settings.TryGetValue(name, out var prevValue) && prevValue == value) return;
        _settings[name] = value;
        Save();
    }

    public string GetValue(string name, string value)
    {
        if (_settings.TryGetValue(name, out string result))
            return result;


        return value;
    }

    public void Remove(string name)
    {
        _settings.Remove(name);
        Save();
    }

    public void SetValue(string name, int value)
    {
        if (_settings.TryGetValue(name, out var prevValue) && int.TryParse(prevValue, out var oldValue) && oldValue == value) return;
        _settings[name] = value.ToString();
        Save();
    }

    public int GetValue(string name, int value)
    {
        if (_settings.TryGetValue(name, out string str))
        {
            if (int.TryParse(str, out int parsedValue))
                return parsedValue;


            return value;
        }

        return value;
    }

    public void SetValue(string name, float value)
    {
        if (_settings.TryGetValue(name, out var prevValue) && float.TryParse(prevValue, out var oldValue) && oldValue == value) return;
        _settings[name] = value.ToString(CultureInfo.InvariantCulture);
        Save();
    }

    public float GetValue(string name, float value)
    {
        if (_settings.TryGetValue(name, out string str))
        {
            if (float.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out float parsedValue))
                return parsedValue;
        }

        return value;

    }

    public double GetValue(string name, double value)
    {
        if (_settings.TryGetValue(name, out string str))
        {
            if (double.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedValue))
                return parsedValue;
        }

        return value;
    }

    public void SetValue(string name, bool value)
    {
        if (_settings.TryGetValue(name, out var prevValue) && bool.TryParse(prevValue, out var oldValue) && oldValue == value) return;
        _settings[name] = value ? "true" : "false";
        Save();
    }

    public bool GetValue(string name, bool value)
    {
        if (_settings.TryGetValue(name, out string str))
        {
            return str == "true";
        }

        return value;
    }

    public void SetValue(string name, Color color)
    {
        if (_settings.TryGetValue(name, out var prevValue) && int.TryParse(prevValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int parsedValue) && Color.FromArgb(parsedValue) == color) return;
        _settings[name] = color.ToArgb().ToString("X8");
        Save();
    }

    public Color GetValue(string name, Color value)
    {
        if (_settings.TryGetValue(name, out string str))
        {
            if (int.TryParse(str, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int parsedValue))
                return Color.FromArgb(parsedValue);
        }

        return value;
    }
}
