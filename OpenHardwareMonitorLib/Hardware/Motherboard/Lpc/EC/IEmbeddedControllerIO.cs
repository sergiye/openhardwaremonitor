using System;

namespace OpenHardwareMonitor.Hardware.Motherboard.Lpc.EC;

public interface IEmbeddedControllerIO : IDisposable
{
    void Read(ushort[] registers, byte[] data);
}
