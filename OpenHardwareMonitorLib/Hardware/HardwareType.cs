namespace OpenHardwareMonitor.Hardware;

/// <summary>
/// Collection of identifiers representing the purpose of the hardware.
/// </summary>
public enum HardwareType
{
    Motherboard,
    SuperIO,
    Cpu,
    Memory,
    GpuNvidia,
    GpuAmd,
    GpuIntel,
    Storage,
    Network,
    Cooler,
    EmbeddedController,
    Psu,
    Battery
}
