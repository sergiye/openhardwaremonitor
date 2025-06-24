using System;

namespace OpenHardwareMonitor.Hardware.Storage;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class NamePrefixAttribute : Attribute
{
    public NamePrefixAttribute(string namePrefix)
    {
        Prefix = namePrefix;
    }

    public string Prefix { get; }
}
