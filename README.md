# Open hardware monitor
[![Release](https://img.shields.io/github/v/release/sergiye/openhardwaremonitor?style=for-the-badge)](https://github.com/sergiye/openhardwaremonitor/releases/latest)
![Downloads](https://img.shields.io/github/downloads/sergiye/openhardwaremonitor/total?style=for-the-badge&color=ff4f42)
![Last commit](https://img.shields.io/github/last-commit/sergiye/openhardwaremonitor?style=for-the-badge&color=00AD00)

[![Nuget](https://img.shields.io/nuget/v/OpenHardwareMonitorLib?style=for-the-badge)](https://www.nuget.org/packages/OpenHardwareMonitorLib/)
[![Nuget](https://img.shields.io/nuget/dt/OpenHardwareMonitorLib?label=nuget-downloads&style=for-the-badge)](https://www.nuget.org/packages/OpenHardwareMonitorLib/)

Open hardware monitor - is free software that can monitor the temperature sensors, fan speeds, voltages, load and clock speeds of your computer.

This application is based on the "original" [openhardwaremonitor](https://github.com/openhardwaremonitor/openhardwaremonitor) project.

----

## Features

### What can it do?

You can see information about devices such as:
 - Motherboards
 - Intel and AMD processors
 - RAM
 - NVIDIA and AMD graphics cards
 - HDD, SSD and NVMe hard drives
 - Network cards
 - Power suppliers
 - Laptop batteries

### Additional features

 - `Remote web-server` mode for browsing data from remote machine with custom port and authentication.
 - `Hide/Unhide` sensors to remove some data from UI and web server.
 - Multiple `Tray icons` and `Gadget` for selected sensor values.
 - `Light`/`Dark` themes with auto switching mode.
 - Custom `color-themes` from external files - You can find examples [here](https://github.com/sergiye/openhardwaremonitor/tree/dev/OpenHardwareMonitor/Resources/themes)
 - `Portable` mode for storing temporary driver file and settings configuration next to the executable file.
 - `Updated versions check` - manually from main menu.

 Note: Some sensors are only available when running the application as administrator.

### UI example with `Light`/`Dark` themes

[<img src="https://github.com/sergiye/openhardwaremonitor/raw/master/themes.png" alt="Themes" width="300"/>](https://github.com/sergiye/openhardwaremonitor/raw/master/themes.png)

## Download

The published version can be obtained from [releases](https://github.com/sergiye/openhardwaremonitor/releases).


## Developer information
**Integrate the library in own application**
1. Add the [OpenHardwareMonitorLib](https://www.nuget.org/packages/OpenHardwareMonitorLib/) NuGet package to your application.
2. Use the sample code below or the test console application from [here](https://github.com/sergiye/openhardwaremonitor/tree/dev/LibTest)


**Sample code**
```c#
class HardwareInfoProvider : IVisitor, IDisposable {

  private readonly Computer computer;

  public HardwareInfoProvider() {
    computer = new Computer {
      IsCpuEnabled = true,
      IsMemoryEnabled = true,
    };
    computer.Open(false);
  }

  internal float Cpu { get; private set; }
  internal float Memory { get; private set; }

  public void VisitComputer(IComputer computer) => computer.Traverse(this);
  public void VisitHardware(IHardware hardware) => hardware.Update();
  public void VisitSensor(ISensor sensor) { }
  public void VisitParameter(IParameter parameter) { }
  public void Dispose() => computer.Close();

  internal void Refresh() {
    computer.Accept(this);
    var cpuTotal = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu)?.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Load && s.Name == "CPU Total");
    Cpu = cpuTotal?.Value ?? -1;

    var memorySensorName = "Physical Memory Available"; //"Virtual Memory Available";
    var memUsed = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Memory)?.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Data && s.Name == memorySensorName);
    Memory = (memUsed == null || !memUsed.Value.HasValue) ? -1 : memUsed.Value.Value * 1024; //GB -> MB
  }
}
```

**Administrator rights**

Some sensors require administrator privileges to access the data. Restart your IDE with admin privileges, or add an [app.manifest](https://learn.microsoft.com/en-us/windows/win32/sbscs/application-manifests) file to your project with requestedExecutionLevel on requireAdministrator.


## How can I help improve it?
The OpenHardwareMonitor team welcomes feedback and contributions!<br/>
You can check if it works properly on your motherboard. For many manufacturers, the way of reading data differs a bit, so if you notice any inaccuracies, please send us a pull request. If you have any suggestions or improvements, don't hesitate to create an issue.

## License

OpenHardwareMonitor is free and open source software licensed under [MPL 2.0](https://www.mozilla.org/en-US/MPL/2.0/). You can use it for personal and commercial purposes.


