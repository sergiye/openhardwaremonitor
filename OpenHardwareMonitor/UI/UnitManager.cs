using OpenHardwareMonitor.Utilities;

namespace OpenHardwareMonitor.UI;

public enum TemperatureUnit
{
    Celsius = 0,
    Fahrenheit = 1
}

public class UnitManager
{

    private readonly PersistentSettings _settings;
    private TemperatureUnit _temperatureUnit;

    public UnitManager(PersistentSettings settings)
    {
        _settings = settings;
        _temperatureUnit = (TemperatureUnit)settings.GetValue("TemperatureUnit", (int)TemperatureUnit.Celsius);
    }

    public TemperatureUnit TemperatureUnit
    {
        get { return _temperatureUnit; }
        set
        {
            _temperatureUnit = value;
            _settings.SetValue("TemperatureUnit", (int)_temperatureUnit);
        }
    }

    public static float? CelsiusToFahrenheit(float? valueInCelsius)
    {
        return valueInCelsius * 1.8f + 32;
    }
}
