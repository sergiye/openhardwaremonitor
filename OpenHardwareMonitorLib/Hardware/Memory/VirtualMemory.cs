namespace OpenHardwareMonitor.Hardware.Memory;

internal sealed class VirtualMemory : Hardware
{
    public VirtualMemory(ISettings settings)
        : base("Virtual Memory", new Identifier("ram"), settings)
    {
        VirtualMemoryUsed = new Sensor("Memory Used", 2, SensorType.Data, this, settings);
        ActivateSensor(VirtualMemoryUsed);

        VirtualMemoryAvailable = new Sensor("Memory Available", 3, SensorType.Data, this, settings);
        ActivateSensor(VirtualMemoryAvailable);

        VirtualMemoryLoad = new Sensor("Memory", 1, SensorType.Load, this, settings);
        ActivateSensor(VirtualMemoryLoad);
    }

    public override HardwareType HardwareType => HardwareType.Memory;

    internal Sensor VirtualMemoryAvailable { get; }

    internal Sensor VirtualMemoryLoad { get; }

    internal Sensor VirtualMemoryUsed { get; }

    public override void Update()
    {
        if (OperatingSystemHelper.IsUnix)
        {
            MemoryLinux.Update(this);
        }
        else
        {
            MemoryWindows.Update(this);
        }
    }
}
