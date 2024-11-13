using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace OpenHardwareMonitor.Interop;

internal class LibC
{
    private const string DllName = "libc";

    [DllImport(DllName)]
    internal static extern int sched_getaffinity(int pid, IntPtr maskSize, ref ulong mask);

    [DllImport(DllName)]
    internal static extern int sched_setaffinity(int pid, IntPtr maskSize, ref ulong mask);
}
