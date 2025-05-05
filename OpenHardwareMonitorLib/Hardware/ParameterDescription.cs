namespace OpenHardwareMonitor.Hardware;

/// <summary>
/// Composite class containing information about the selected <see cref="ISensor"/>.
/// </summary>
public struct ParameterDescription
{
    /// <summary>
    /// Creates a new instance and assigns values.
    /// </summary>
    /// <param name="name">Name of the selected component.</param>
    /// <param name="description">Description of the selected component.</param>
    /// <param name="defaultValue">Default value of the selected component.</param>
    public ParameterDescription(string name, string description, float defaultValue)
    {
        Name = name;
        Description = description;
        DefaultValue = defaultValue;
    }

    /// <summary>
    /// Gets a name of the parent <see cref="ISensor"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets a description of the parent <see cref="ISensor"/>.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets a default value of the parent <see cref="ISensor"/>.
    /// </summary>
    public float DefaultValue { get; }
}
