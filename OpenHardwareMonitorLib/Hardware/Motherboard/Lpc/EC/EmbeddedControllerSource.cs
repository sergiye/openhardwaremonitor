namespace OpenHardwareMonitor.Hardware.Motherboard.Lpc.EC;

public class EmbeddedControllerSource
{
    public EmbeddedControllerSource(string name, SensorType type, ushort register, byte size = 1, float factor = 1.0f, int blank = int.MaxValue)
    {
        Name = name;

        Register = register;
        Size = size;
        Type = type;
        Factor = factor;
        Blank = blank;
    }

    public string Name { get; }
    public ushort Register { get; }
    public byte Size { get; }
    public float Factor { get; }

    public int Blank { get; }

    public EmbeddedControllerReader Reader { get; }

    public SensorType Type { get; }
}
