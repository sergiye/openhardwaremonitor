using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.HDD {

  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  internal struct DriveAttributeValue {
    public byte Identifier;
    public short StatusFlags;
    public byte AttrValue;
    public byte WorstValue;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public byte[] RawValue;
    public byte Reserved;
  }  

}
