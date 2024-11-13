using System.Collections.Generic;

namespace OpenHardwareMonitor.Hardware;

/// <summary>
/// A group of devices from one category in one list.
/// </summary>
internal interface IGroup
{
    /// <summary>
    /// Gets a list that stores information about <see cref="IHardware"/> in a given group.
    /// </summary>
    IReadOnlyList<IHardware> Hardware { get; }

    /// <summary>
    /// Report containing most of the known information about all <see cref="IHardware"/> in this <see cref="IGroup"/>.
    /// </summary>
    /// <returns>A formatted text string with hardware information.</returns>
    string GetReport();

    /// <summary>
    /// Stop updating this group in the future.
    /// </summary>
    void Close();
}
