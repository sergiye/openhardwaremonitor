namespace OpenHardwareMonitor.Hardware.LPC {
  internal interface ISuperIO {

    Chip Chip { get; }

    // get voltage, temperature, fan and control channel values
    float?[] Voltages { get; }
    float?[] Temperatures { get; }
    float?[] Fans { get; }
    float?[] Controls { get; }

    // set control value, null = auto    
    void SetControl(int index, byte? value);         

    // read and write GPIO
    byte? ReadGPIO(int index);
    void WriteGPIO(int index, byte value);

    string GetReport();

    void Update();
  }
}
