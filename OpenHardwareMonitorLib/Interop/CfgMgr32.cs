using System;
using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Interop;

internal static class CfgMgr32
{
    internal const uint CM_GET_DEVICE_INTERFACE_LIST_PRESENT = 0;
    internal const int CR_SUCCESS = 0;

    internal const string DllName = "CfgMgr32.dll";
    internal static Guid GUID_DISPLAY_DEVICE_ARRIVAL = new("1CA05180-A699-450A-9A0C-DE4FBE3DDD89");

    [DllImport(DllName, CharSet = CharSet.Unicode)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern uint CM_Get_Device_Interface_List_Size(out uint size, ref Guid interfaceClassGuid, string deviceID, uint flags);

    [DllImport(DllName, CharSet = CharSet.Unicode)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern uint CM_Get_Device_Interface_List(ref Guid interfaceClassGuid, string deviceID, char[] buffer, uint bufferLength, uint flags);
}
