using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace HidSharp.Utility
{
    [Obsolete("This class is experimental and its functionality may be moved elsewhere in the future. Please do not rely on it.")]
    public static class HResult
    {
        public const int FileNotFound = unchecked((int)0x80070002);     // ERROR_FILE_NOT_FOUND
        public const int SharingViolation = unchecked((int)0x80070020); // ERROR_SHARING_VIOLATION
        public const int SemTimeout = unchecked((int)0x80070079);       // ERROR_SEM_TIMEOUT

        public static int FromException(Exception exception)
        {
            Throw.If.Null(exception);

            try
            {
                // This works with .NET 4.0 as well as later versions. Also, it does not change any state.
                return (int)exception.GetType().InvokeMember("HResult",
                    BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null, exception, new object[0]);
            }
            catch
            {
                return Marshal.GetHRForException(exception);
            }
        }
    }
}
