namespace HidSharp.Reports.Units
{
    /// <summary>
    /// Defines the possible unit systems.
    /// </summary>
    public enum UnitSystem
    {
        /// <summary>
        /// No units are used.
        /// </summary>
        None = 0,

        /// <summary>
        /// The SI Linear unit system uses centimeters for length, grams for mass, seconds for time,
        /// Kelvin for temperature, Amperes for current, and candelas for luminous intensity.
        /// </summary>
        SILinear,

        /// <summary>
        /// The SI Rotation unit system uses radians for length, grams for mass, seconds for time,
        /// Kelvin for temperature, Amperes for current, and candelas for luminous intensity.
        /// </summary>
        SIRotation,

        /// <summary>
        /// The English Linear unit system uses inches for length, slugs for mass, seconds for time,
        /// Fahrenheit for temperature, Amperes for current, and candelas for luminous intensity.
        /// </summary>
        EnglishLinear,

        /// <summary>
        /// The English Rotation unit system uses degrees for length, slugs for mass, seconds for time,
        /// Fahrenheit for temperature, Amperes for current, and candelas for luminous intensity.
        /// </summary>
        EnglishRotation
    }
}
