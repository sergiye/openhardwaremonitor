using RAMSPDToolkit.SPD.Interfaces;

namespace OpenHardwareMonitor.Hardware.Memory.Sensors;

internal class SpdThermalSensor(string name, int index, SensorType sensorType, Hardware hardware, ISettings settings, IThermalSensor thermalSensor)
    : Sensor(name, index, sensorType, hardware, settings)
{
    public bool UpdateSensor()
    {
        if (!thermalSensor.UpdateTemperature())
            return false;

        Value = thermalSensor.Temperature;

        return true;
    }
}
