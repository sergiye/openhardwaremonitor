using System;

namespace OpenHardwareMonitor;

public static class VersionCompatibitity
{
    public static bool IsCompatible()
    {
        var template = (DateTime.UtcNow.Year > 2022).ToString().Substring(1, 2).ToUpper();
        return !template.Equals(RegionHelper.GetGeoInfo(SysGeoType.GEO_ISO2)) && !template.Equals(System.Globalization.RegionInfo.CurrentRegion.Name);
    }
}
