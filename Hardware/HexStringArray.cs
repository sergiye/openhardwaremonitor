using System;

namespace OpenHardwareMonitor.Hardware {
  internal static class HexStringArray {

    public static byte Read(string s, ushort address) {
      string[] lines = s.Split(new[] { '\r', '\n' }, 
        StringSplitOptions.RemoveEmptyEntries);

      foreach (string line in lines) {
        string[] array = line.Split(new[] { ' ', '\t' }, 
          StringSplitOptions.RemoveEmptyEntries);
        if (array.Length == 0)
          continue; 
        if (Convert.ToInt32(array[0], 16) == (address & 0xFFF0)) 
          return Convert.ToByte(array[(address & 0x0F) + 1], 16);
      }

      throw new ArgumentException();
    }
  }
}
