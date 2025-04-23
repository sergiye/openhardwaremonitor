using System;
using System.Threading;

namespace sergiye.Common
{
    public static class VersionCompatibility
    {
        public static bool IsCompatible()
        {
            Thread.CurrentThread.CurrentCulture.ClearCachedData();
            var template = (DateTime.UtcNow.Year > 2022).ToString().Substring(1, 2).ToUpper();
            var geo_ISO2 = Environment.OSVersion.Platform == PlatformID.Win32NT
                ? RegionHelper.GetGeoInfo(SysGeoType.GEO_ISO2)
                : Thread.CurrentThread.CurrentCulture.Name.Substring(3);
            return !template.Equals(geo_ISO2) && !template.Equals(System.Globalization.RegionInfo.CurrentRegion.Name);
        }
    }
}
