namespace OpenHardwareMonitor.Hardware.Motherboard.Lpc;

internal interface IGigabyteController
{
    bool Enable(bool enabled);

    void Restore();
}
