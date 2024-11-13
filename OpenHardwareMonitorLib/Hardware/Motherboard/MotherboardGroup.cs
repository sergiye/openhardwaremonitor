using System.Collections.Generic;

namespace OpenHardwareMonitor.Hardware.Motherboard;

internal class MotherboardGroup : IGroup
{
    private readonly Motherboard[] _motherboards;

    public MotherboardGroup(SMBios smbios, ISettings settings)
    {
        _motherboards = new Motherboard[1];
        _motherboards[0] = new Motherboard(smbios, settings);
    }

    public IReadOnlyList<IHardware> Hardware => _motherboards;

    public void Close()
    {
        foreach (Motherboard mainboard in _motherboards)
            mainboard.Close();
    }

    public string GetReport()
    {
        return null;
    }
}
