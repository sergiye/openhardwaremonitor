namespace OpenHardwareMonitor.Hardware;

/// <summary>
/// Abstract parent with logic for the abstract class that stores data.
/// </summary>
public interface IElement
{
    /// <summary>
    /// Accepts the observer for this instance.
    /// </summary>
    /// <param name="visitor">Computer observer making the calls.</param>
    void Accept(IVisitor visitor);

    /// <summary>
    /// Call the <see cref="Accept"/> method for all child instances <c>(called only from visitors).</c>
    /// </summary>
    /// <param name="visitor">Computer observer making the calls.</param>
    void Traverse(IVisitor visitor);
}
