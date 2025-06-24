using System;

namespace HidSharp
{
    [Flags]
    public enum DeviceTypes
    {
        Hid = 1,
        Serial = 2,
        Ble = 4
    }
}
