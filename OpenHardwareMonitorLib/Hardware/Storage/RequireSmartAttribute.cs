using System;

namespace OpenHardwareMonitor.Hardware.Storage;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class RequireSmartAttribute : Attribute
{
    public RequireSmartAttribute(byte attributeId)
    {
        AttributeId = attributeId;
    }

    public byte AttributeId { get; }
}
