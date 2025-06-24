namespace HidSharp
{
    /// <summary>
    /// The priority at which to open a device stream.
    /// </summary>
    public enum OpenPriority
    {
        /// <summary>
        /// The lowest priority.
        /// </summary>
        Idle = -2,

        /// <summary>
        /// Very low priority.
        /// </summary>
        VeryLow = -1,

        /// <summary>
        /// Low priority.
        /// </summary>
        Low = 0,

        /// <summary>
        /// The default priority.
        /// </summary>
        Normal = 1,

        /// <summary>
        /// High priority.
        /// </summary>
        High = 2,

        /// <summary>
        /// The highest priority.
        /// </summary>
        VeryHigh = 3
    }
}
