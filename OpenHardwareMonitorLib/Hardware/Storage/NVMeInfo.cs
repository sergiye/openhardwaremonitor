namespace OpenHardwareMonitor.Hardware.Storage;

public abstract class NVMeInfo
{
    public ushort ControllerId { get; protected set; }

    public byte[] IEEE { get; protected set; }

    public int Index { get; protected set; }

    public string Model { get; protected set; }

    public uint NumberNamespaces { get; protected set; }

    public string Revision { get; protected set; }

    public string Serial { get; protected set; }

    public ushort SSVID { get; protected set; }

    public ulong TotalCapacity { get; protected set; }

    public ulong UnallocatedCapacity { get; protected set; }

    public ushort VID { get; protected set; }
}
