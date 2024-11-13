using System.Collections.Generic;

namespace OpenHardwareMonitor.Hardware.Storage;

[NamePrefix("PLEXTOR")]
internal class SsdPlextor : AtaStorage
{
    private static readonly IReadOnlyList<SmartAttribute> _smartAttributes = new List<SmartAttribute>
    {
        new(0x09, SmartNames.PowerOnHours, RawToInt),
        new(0x0C, SmartNames.PowerCycleCount, RawToInt),
        new(0xF1, SmartNames.HostWrites, RawToGb, SensorType.Data, 0, SmartNames.HostWrites),
        new(0xF2, SmartNames.HostReads, RawToGb, SensorType.Data, 1, SmartNames.HostReads)
    };

    public SsdPlextor(StorageInfo storageInfo, ISmart smart, string name, string firmwareRevision, int index, ISettings settings)
        : base(storageInfo, smart, name, firmwareRevision, "ssd", index, _smartAttributes, settings)
    { }

    private static float RawToGb(byte[] rawValue, byte value, IReadOnlyList<IParameter> parameters)
    {
        return RawToInt(rawValue, value, parameters) / 32;
    }
}
