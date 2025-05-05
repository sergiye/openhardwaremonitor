using System.IO;
using System.Linq;

namespace OpenHardwareMonitor.Hardware.Memory;

internal sealed class GenericLinuxMemory : Hardware
{
    private readonly Sensor _physicalMemoryTotal;
    private readonly Sensor _physicalMemoryAvailable;
    private readonly Sensor _physicalMemoryLoad;
    private readonly Sensor _physicalMemoryUsed;
    private readonly Sensor _virtualMemoryTotal;
    private readonly Sensor _virtualMemoryAvailable;
    private readonly Sensor _virtualMemoryLoad;
    private readonly Sensor _virtualMemoryUsed;

    public override HardwareType HardwareType => HardwareType.Memory;

    public GenericLinuxMemory(string name, ISettings settings) : base(name, new Identifier("ram"), settings)
    {
        ActivateSensor(_physicalMemoryLoad = new Sensor("Physical Memory", 0, SensorType.Load, this, settings));
        ActivateSensor(_virtualMemoryLoad = new Sensor("Virtual Memory", 1, SensorType.Load, this, settings));

        ActivateSensor(_physicalMemoryTotal = new Sensor("Physical Memory Total", 0, SensorType.Data, this, settings));
        ActivateSensor(_physicalMemoryUsed = new Sensor("Physical Memory Used", 1, SensorType.Data, this, settings));
        ActivateSensor(_physicalMemoryAvailable = new Sensor("Physical Memory Available", 2, SensorType.Data, this, settings));
        ActivateSensor(_virtualMemoryTotal = new Sensor("Virtual Memory Total", 3, SensorType.Data, this, settings));
        ActivateSensor(_virtualMemoryUsed = new Sensor("Virtual Memory Used", 4, SensorType.Data, this, settings));
        ActivateSensor(_virtualMemoryAvailable = new Sensor("Virtual Memory Available", 5, SensorType.Data, this, settings));
    }

    public override void Update()
    {
        try
        {
            string[] memoryInfo = File.ReadAllLines("/proc/meminfo");

            {
                float totalMemory_GB = GetMemInfoValue(memoryInfo.First(entry => entry.StartsWith("MemTotal:"))) / 1024.0f / 1024.0f;
                float freeMemory_GB = GetMemInfoValue(memoryInfo.First(entry => entry.StartsWith("MemFree:"))) / 1024.0f / 1024.0f;
                float cachedMemory_GB = GetMemInfoValue(memoryInfo.First(entry => entry.StartsWith("Cached:"))) / 1024.0f / 1024.0f;

                float usedMemory_GB = totalMemory_GB - freeMemory_GB - cachedMemory_GB;

                _physicalMemoryUsed.Value = usedMemory_GB;
                _physicalMemoryTotal.Value = totalMemory_GB;
                _physicalMemoryAvailable.Value = freeMemory_GB;
                _physicalMemoryLoad.Value = 100.0f * (usedMemory_GB / totalMemory_GB);
            }
            {
                float totalSwapMemory_GB = GetMemInfoValue(memoryInfo.First(entry => entry.StartsWith("SwapTotal"))) / 1024.0f / 1024.0f;
                float freeSwapMemory_GB = GetMemInfoValue(memoryInfo.First(entry => entry.StartsWith("SwapFree"))) / 1024.0f / 1024.0f;
                float usedSwapMemory_GB = totalSwapMemory_GB - freeSwapMemory_GB;

                _virtualMemoryUsed.Value = usedSwapMemory_GB;
                _virtualMemoryTotal.Value = totalSwapMemory_GB;
                _virtualMemoryAvailable.Value = freeSwapMemory_GB;
                _virtualMemoryLoad.Value = 100.0f * (usedSwapMemory_GB / totalSwapMemory_GB);
            }
        }
        catch
        {
            _physicalMemoryUsed.Value = null;
            _physicalMemoryAvailable.Value = null;
            _physicalMemoryLoad.Value = null;

            _virtualMemoryUsed.Value = null;
            _virtualMemoryAvailable.Value = null;
            _virtualMemoryLoad.Value = null;
        }
    }

    private static long GetMemInfoValue(string line)
    {
        // Example: "MemTotal:       32849676 kB"

        string valueWithUnit = line.Split(':').Skip(1).First().Trim();
        string valueAsString = valueWithUnit.Split(' ').First();

        return long.Parse(valueAsString);
    }
}
