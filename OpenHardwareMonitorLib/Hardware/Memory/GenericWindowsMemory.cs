using System.Runtime.InteropServices;
using OpenHardwareMonitor.Interop;

namespace OpenHardwareMonitor.Hardware.Memory;

internal sealed class GenericWindowsMemory : Hardware
{
    private readonly Sensor _physicalMemoryTotal;
    private readonly Sensor _physicalMemoryAvailable;
    private readonly Sensor _physicalMemoryLoad;
    private readonly Sensor _physicalMemoryUsed;

    private readonly Sensor _virtualMemoryTotal;
    private readonly Sensor _virtualMemoryAvailable;
    private readonly Sensor _virtualMemoryLoad;
    private readonly Sensor _virtualMemoryUsed;

    private readonly Sensor _kernelSize;

    private readonly Sensor _processCount;
    private readonly Sensor _threadCount;
    private readonly Sensor _handleCount;

    public GenericWindowsMemory(string name, ISettings settings) : base(name, new Identifier("ram"), settings)
    {
        ActivateSensor(_physicalMemoryLoad = new Sensor("Physical Memory", 0, SensorType.Load, this, settings));
        ActivateSensor(_virtualMemoryLoad = new Sensor("Virtual Memory", 1, SensorType.Load, this, settings));

        ActivateSensor(_physicalMemoryTotal = new Sensor("Physical Memory Total", 0, SensorType.Data, this, settings));
        ActivateSensor(_physicalMemoryUsed = new Sensor("Physical Memory Used", 1, SensorType.Data, this, settings));
        ActivateSensor(_physicalMemoryAvailable = new Sensor("Physical Memory Available", 2, SensorType.Data, this, settings));
        ActivateSensor(_virtualMemoryTotal = new Sensor("Virtual Memory Total", 3, SensorType.Data, this, settings));
        ActivateSensor(_virtualMemoryUsed = new Sensor("Virtual Memory Used", 4, SensorType.Data, this, settings));
        ActivateSensor(_virtualMemoryAvailable = new Sensor("Virtual Memory Available", 5, SensorType.Data, this, settings));
        ActivateSensor(_kernelSize = new Sensor("Kernel memory usage", 6, SensorType.Data, this, settings));

        ActivateSensor(_processCount = new Sensor("Processes", 0, SensorType.IntFactor, this, settings));
        ActivateSensor(_threadCount = new Sensor("Threads", 1, SensorType.IntFactor, this, settings));
        ActivateSensor(_handleCount = new Sensor("Handles", 2, SensorType.IntFactor, this, settings));
    }

    public override HardwareType HardwareType => HardwareType.Memory;

    public override void Update()
    {
        Kernel32.MEMORYSTATUSEX status = new() { dwLength = (uint)Marshal.SizeOf<Kernel32.MEMORYSTATUSEX>() };
        if (Kernel32.GlobalMemoryStatusEx(ref status))
        {
            _physicalMemoryTotal.Value = (float)status.ullTotalPhys / (1024 * 1024 * 1024);
            _physicalMemoryUsed.Value = (float)(status.ullTotalPhys - status.ullAvailPhys) / (1024 * 1024 * 1024);
            _physicalMemoryAvailable.Value = (float)status.ullAvailPhys / (1024 * 1024 * 1024);
            _physicalMemoryLoad.Value = 100.0f - 100.0f * status.ullAvailPhys / status.ullTotalPhys;

            _virtualMemoryUsed.Value =
                (float)(status.ullTotalPageFile - status.ullAvailPageFile) / (1024 * 1024 * 1024);
            _virtualMemoryAvailable.Value = (float)status.ullAvailPageFile / (1024 * 1024 * 1024);
            _virtualMemoryLoad.Value = 100.0f - 100.0f * status.ullAvailPageFile / status.ullTotalPageFile;
        }

        Kernel32.PERFORMANCE_INFORMATION performanceInfo = new() { cb = (uint)Marshal.SizeOf<Kernel32.PERFORMANCE_INFORMATION>() };
        if (Kernel32.GetPerformanceInfo(ref performanceInfo, performanceInfo.cb))
        {
            _virtualMemoryTotal.Value = (float)performanceInfo.CommitLimit * performanceInfo.PageSize / (1024 * 1024 * 1024);
            //_virtualMemoryUsed.Value = (float)performanceInfo.CommitTotal * performanceInfo.PageSize / (1024 * 1024 * 1024);
            _kernelSize.Value = (float)performanceInfo.KernelNonpaged * performanceInfo.PageSize / (1024 * 1024 * 1024);

            _processCount.Value = performanceInfo.ProcessCount;
            _threadCount.Value = performanceInfo.ThreadCount;
            _handleCount.Value = performanceInfo.HandleCount;
        }
    }
}
