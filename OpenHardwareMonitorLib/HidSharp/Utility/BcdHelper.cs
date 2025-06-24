using System;

namespace HidSharp.Utility
{
    /// <summary>
    /// Converts USB device release numbers to and from binary-coded decimal.
    /// </summary>
    static class BcdHelper
    {
        /// <summary>
        /// Converts a USB device release number to binary-coded decimal.
        /// </summary>
        /// <param name="version">The device release number.</param>
        /// <returns>The device release number, in binary-coded decimal.</returns>
        public static int FromVersion(Version version)
        {
            Throw.If.Null(version);
            return (version.Major / 10) << 12 | (version.Major % 10) << 8 | (version.Minor / 10) << 4 | (version.Minor % 10);
        }

        /// <summary>
        /// Converts a USB device release number from binary-coded decimal.
        /// </summary>
        /// <param name="bcd">The device release number, in binary-coded decimal.</param>
        /// <returns>The device release number.</returns>
        public static Version ToVersion(int bcd)
        {
            Throw.If.False(bcd >= ushort.MinValue && bcd <= ushort.MaxValue);
            return new Version(((bcd >> 12) & 0xf) * 10 + ((bcd >> 8) & 0xf), ((bcd >> 4) & 0xf) * 10 + (bcd & 0xf));
        }
    }
}
