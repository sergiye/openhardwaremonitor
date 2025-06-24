using System;

namespace HidSharp.Utility
{
    public static class HidSharpDiagnostics
    {
        static HidSharpDiagnostics()
        {
            PerformStrictChecks = false;
        }

        internal static void PerformStrictCheck(bool condition, string message)
        {
            if (!condition)
            {
                message += "\n\nTo disable this exception, set HidSharpDiagnostics.PerformStrictChecks to false.";
                throw new InvalidOperationException(message);
            }
        }

        internal static void Trace(string message)
        {
            if (!EnableTracing) { return; }
            System.Diagnostics.Trace.WriteLine(message, "HIDSharp");
        }

        internal static void Trace(string formattedMessage, object arg)
        {
            if (!EnableTracing) { return; }
            Trace(string.Format(formattedMessage, arg));
        }

        internal static void Trace(string formattedMessage, params object[] args)
        {
            if (!EnableTracing) { return; }
            Trace(string.Format(formattedMessage, args));
        }

        public static bool EnableTracing
        {
            get;
            set;
        }

        public static bool PerformStrictChecks
        {
            get;
            set;
        }
    }
}
