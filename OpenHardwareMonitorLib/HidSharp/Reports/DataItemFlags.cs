using System;

namespace HidSharp.Reports
{
    [Flags]
    public enum DataItemFlags : uint
    {
        /// <summary>
        /// No flags are set.
        /// </summary>
        None = 0,

        /// <summary>
        /// Constant values cannot be changed.
        /// </summary>
        Constant = 1 << 0,

        /// <summary>
        /// Each variable field corresponds to a particular value.
        /// The alternative is an array, where each field specifies an index.
        /// For example, with eight buttons, a variable field would have eight bits.
        /// An array would have an index of which button is pressed.
        /// </summary>
        Variable = 1 << 1,

        /// <summary>
        /// Mouse motion is in relative coordinates.
        /// Most sensors -- joysticks, accelerometers, etc. -- output absolute coordinates.
        /// </summary>
        Relative = 1 << 2,

        /// <summary>
        /// The value wraps around in a continuous manner.
        /// </summary>
        Wrap = 1 << 3,

        Nonlinear = 1 << 4,

        NoPreferred = 1 << 5,

        NullState = 1 << 6,

        Volatile = 1 << 7,

        BufferedBytes = 1 << 8
    }
}
