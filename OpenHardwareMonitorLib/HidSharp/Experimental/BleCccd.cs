using System;

namespace HidSharp.Experimental
{
    [Flags]
    public enum BleCccd : ushort
    {
        None = 0,
        Notification = 1,
        Indication = 2
    }
}
