namespace HidSharp.Reports
{
    public enum ExpectedUsageType
    {
        /// <summary>
        /// Level-triggered. A momentary button. 0 indicates not pressed, 1 indicates pressed.
        /// </summary>
        PushButton = 1,

        /// <summary>
        /// Level-triggered. Toggle buttons maintain their state. 0 indicates not pressed, 1 indicates pressed.
        /// </summary>
        ToggleButton,

        /// <summary>
        /// Edge-triggered. A 0-to-1 transition should activate the one-shot function.
        /// </summary>
        OneShot,

        /// <summary>
        /// Edge-triggered. Each report of -1 goes down. Each report of 1 goes up.
        /// </summary>
        UpDown
    }
}
