using System;
using OpenHardwareMonitor.Interop;

namespace OpenHardwareMonitor.Hardware.Storage;

public interface ISmart : IDisposable
{
    bool IsValid { get; }

    void Close();

    bool EnableSmart();

    Kernel32.SMART_ATTRIBUTE[] ReadSmartData();

    Kernel32.SMART_THRESHOLD[] ReadSmartThresholds();

    bool ReadNameAndFirmwareRevision(out string name, out string firmwareRevision);
}
