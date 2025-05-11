using System.Collections.Generic;

namespace OpenHardwareMonitor.Hardware.Memory;

internal class MemoryGroup : IGroup
{
    private readonly Hardware[] _hardware;

    public MemoryGroup(ISettings settings)
    {
        _hardware = [OperatingSystemHelper.IsUnix ? new GenericLinuxMemory("Memory", settings) : new GenericWindowsMemory("Memory", settings)];
    }

    public string GetReport()
    {
        return null;
    }

    public IReadOnlyList<IHardware> Hardware => _hardware;

    public void Close()
    {
        foreach (Hardware ram in _hardware)
            ram.Close();
    }
}
