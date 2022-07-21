using System;
using System.Text;

namespace OpenHardwareMonitor.Utilities {

  public class HttpUtility {
    public static string UrlEncode(string s) {
 
      int maxLength = 32765;
      var sb = new StringBuilder();
      int imax = s.Length / maxLength;

      for (int i = 0; i <= imax; i++) {
        sb.Append(
          Uri.EscapeDataString(i < imax
          ? s.Substring(maxLength * i, maxLength)
          : s.Substring(maxLength * i)));
      }

      return sb.ToString();
    }

  }
}
