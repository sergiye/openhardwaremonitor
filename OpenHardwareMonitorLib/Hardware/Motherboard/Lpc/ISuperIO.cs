namespace OpenHardwareMonitor.Hardware.Motherboard.Lpc;

internal interface ISuperIO
{
    Chip Chip { get; }

    float?[] Controls { get; }

    float?[] Fans { get; }

    float?[] Temperatures { get; }

    // get voltage, temperature, fan and control channel values
    float?[] Voltages { get; }

    // set control value, null = auto
    void SetControl(int index, byte? value);

    // read and write GPIO
    byte? ReadGpio(int index);

    void WriteGpio(int index, byte value);

    string GetReport();

    void Update();
}
