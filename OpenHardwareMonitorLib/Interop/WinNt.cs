using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Interop;

/// <summary>
/// Contains Win32 definitions for Windows NT.
/// </summary>
internal static class WinNt
{
    internal const int STATUS_SUCCESS = 0;

    /// <summary>
    /// Describes a local identifier for an adapter.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct LUID
    {
        public readonly uint LowPart;
        public readonly int HighPart;
    }

    /// <summary>
    /// Represents a 64-bit signed integer value.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    internal struct LARGE_INTEGER
    {
        [FieldOffset(0)]
        public long QuadPart;

        [FieldOffset(0)]
        public uint LowPart;

        [FieldOffset(4)]
        public int HighPart;
    }
}
