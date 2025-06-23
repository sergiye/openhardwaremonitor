using System.Runtime.InteropServices;
using OpenHardwareMonitor.Interop;

namespace OpenHardwareMonitor.Hardware.Memory;

internal static class MemoryWindows
{
    public static void Update(TotalMemory memory)
    {
        Kernel32.MEMORYSTATUSEX status = new() { dwLength = (uint)Marshal.SizeOf<Kernel32.MEMORYSTATUSEX>() };

        if (!Kernel32.GlobalMemoryStatusEx(ref status))
            return;

        memory.PhysicalMemoryUsed.Value      = (float)(status.ullTotalPhys - status.ullAvailPhys) / (1024 * 1024 * 1024);
        memory.PhysicalMemoryAvailable.Value = (float)status.ullAvailPhys / (1024 * 1024 * 1024);
        memory.PhysicalMemoryLoad.Value      = 100.0f - ((100.0f * status.ullAvailPhys) / status.ullTotalPhys);
    }

    public static void Update(VirtualMemory memory)
    {
        Kernel32.MEMORYSTATUSEX status = new() { dwLength = (uint)Marshal.SizeOf<Kernel32.MEMORYSTATUSEX>() };

        if (!Kernel32.GlobalMemoryStatusEx(ref status))
            return;

        memory.VirtualMemoryUsed.Value      = (float)(status.ullTotalPageFile - status.ullAvailPageFile) / (1024 * 1024 * 1024);
        memory.VirtualMemoryAvailable.Value = (float)status.ullAvailPageFile / (1024 * 1024 * 1024);
        memory.VirtualMemoryLoad.Value      = 100.0f - ((100.0f * status.ullAvailPageFile) / status.ullTotalPageFile);
    }
}
