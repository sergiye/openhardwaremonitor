using System.Management.Instrumentation;
using OpenHardwareMonitor.Hardware;

namespace OpenHardwareMonitor.WMI;

[InstrumentationClass(InstrumentationType.Instance)]
public class Hardware : IWmiObject
{
    #region WMI Exposed

    public string HardwareType { get; }
    public string Identifier { get; }
    public string Name { get; }
    public string Parent { get; }

    #endregion

    public Hardware(IHardware hardware)
    {
        Name = hardware.Name;
        Identifier = hardware.Identifier.ToString();
        HardwareType = hardware.HardwareType.ToString();
        Parent = hardware.Parent != null
            ? hardware.Parent.Identifier.ToString()
            : "";
    }

    public void Update() { }
}
