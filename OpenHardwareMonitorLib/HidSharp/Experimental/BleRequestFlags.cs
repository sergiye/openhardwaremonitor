using System;

namespace HidSharp.Experimental
{
    [Flags]
    public enum BleRequestFlags
    {
        None = 0,
        Authenticated = 1,
        Encrypted = 2,
        Cacheable = 4
    }
}
