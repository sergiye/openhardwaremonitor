namespace OpenHardwareMonitor.Hardware;

/// <summary>
/// Abstract object that stores information about the limits of <see cref="ISensor"/>.
/// </summary>
public interface ISensorLimits
{
    /// <summary>
    /// Upper limit of <see cref="ISensor"/> value.
    /// </summary>
    float? HighLimit { get; }

    /// <summary>
    /// Lower limit of <see cref="ISensor"/> value.
    /// </summary>
    float? LowLimit { get; }
}

/// <summary>
/// Abstract object that stores information about the critical limits of <see cref="ISensor"/>.
/// </summary>
public interface ICriticalSensorLimits
{
    /// <summary>
    /// Critical upper limit of <see cref="ISensor"/> value.
    /// </summary>
    float? CriticalHighLimit { get; }

    /// <summary>
    /// Critical lower limit of <see cref="ISensor"/> value.
    /// </summary>
    float? CriticalLowLimit { get; }
}
