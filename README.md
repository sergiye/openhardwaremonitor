# Open hardware monitor
[![Release](https://img.shields.io/github/v/release/sergiye/openhardwaremonitor?style=for-the-badge)](https://github.com/sergiye/openhardwaremonitor/releases/latest)
![Downloads](https://img.shields.io/github/downloads/sergiye/openhardwaremonitor/total?style=for-the-badge&color=ff4f42)
![Last commit](https://img.shields.io/github/last-commit/sergiye/openhardwaremonitor?style=for-the-badge&color=00AD00)

[![Nuget](https://img.shields.io/nuget/v/OpenHardwareMonitorLib?style=for-the-badge)](https://www.nuget.org/packages/OpenHardwareMonitorLib/) 
[![Nuget](https://img.shields.io/nuget/dt/OpenHardwareMonitorLib?label=nuget-downloads&style=for-the-badge)](https://www.nuget.org/packages/OpenHardwareMonitorLib/)

Open hardware monitor - is free software that can monitor the temperature sensors, fan speeds, voltages, load and clock speeds of your computer.

This application is based on the "original" [openhardwaremonitor](https://github.com/openhardwaremonitor/openhardwaremonitor) project.

----

[<img src="https://github.com/sergiye/hiberbeeTheme/raw/master/assets/ukraine_flag_bar.png" alt="UA"/>](https://u24.gov.ua)


Support the Armed Forces of Ukraine and People Affected by Russia’s Aggression on UNITED24, the official fundraising platform of Ukraine: https://u24.gov.ua.

**Слава Україні!**

[<img src="https://github.com/sergiye/hiberbeeTheme/raw/master/assets/ukraine_flag_bar.png" alt="UA"/>](https://u24.gov.ua)


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

 - `Remote web-server` mode for browsing data from remote machine with custom port and authentification.
 - `Hide/Unhide` sensors to remove some data from UI and web server.
 - Multiple `Tray icons` and `Gadget` for selected sensor values.
 - `Light`/`Dark` themes with auto switching mode.
 - Custom `color-themes` from external files - You can find examples [here](https://github.com/sergiye/openhardwaremonitor/tree/dev/OpenHardwareMonitor/Resources/themes)
 - `Portable` mode for storing temporary driver file and settings configuration next to the executable file.
 - `Updated versions check` - manually from main menu.
 
 Note: Some sensors are only available when running the application as administrator.

### UI example with `Light`/`Dark` themes 

[<img src="https://github.com/sergiye/openhardwaremonitor/raw/master/themes.png" alt="Themes" width="300"/>](https://github.com/sergiye/openhardwaremonitor/releases)

# Download

**The recommended way to get the program is BUILD from source**
- Install git, Visual Studio
- `git clone https://github.com/sergiye/openhardwaremonitor.git`
- build

**or download build from [releases](https://github.com/sergiye/openhardwaremonitor/releases).**


# Developer information
**Integrate the library in own application**
1. Add the [OpenHardwareMonitorLib](https://www.nuget.org/packages/OpenHardwareMonitorLib/) NuGet package to your application.
2. Use the sample code below or the test console application from [here](https://github.com/sergiye/openhardwaremonitor/tree/dev/OpenHardwareMonitor/LibTest)


**Sample code**
```c#
public class UpdateVisitor : IVisitor {
    public void VisitComputer(IComputer computer) {
        computer.Traverse(this);
    }
    public void VisitHardware(IHardware hardware) {
        hardware.Update();
        foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
    }
    public void VisitSensor(ISensor sensor) { }
    public void VisitParameter(IParameter parameter) { }
}

public void Monitor() {
    Computer computer = new Computer {
        IsCpuEnabled = true,
        IsGpuEnabled = true,
        IsMemoryEnabled = true,
        IsMotherboardEnabled = true,
        IsControllerEnabled = true,
        IsNetworkEnabled = true,
        IsBatteryEnabled = true,
        IsStorageEnabled = true
    };

    computer.Open(false);
    computer.Accept(new UpdateVisitor());

    foreach (IHardware hardware in computer.Hardware) {
        Console.WriteLine("Hardware: {0}", hardware.Name);
        foreach (IHardware subhardware in hardware.SubHardware) {
            Console.WriteLine("\tSubhardware: {0}", subhardware.Name);
            foreach (ISensor sensor in subhardware.Sensors) {
                Console.WriteLine("\t\tSensor: {0}, value: {1}", sensor.Name, sensor.Value);
            }
        }

        foreach (ISensor sensor in hardware.Sensors) {
            Console.WriteLine("\tSensor: {0}, value: {1}", sensor.Name, sensor.Value);
        }
    }
    
    computer.Close();
}
```

**Administrator rights**

Some sensors require administrator privileges to access the data. Restart your IDE with admin privileges, or add an [app.manifest](https://learn.microsoft.com/en-us/windows/win32/sbscs/application-manifests) file to your project with requestedExecutionLevel on requireAdministrator.


## How can I help improve it?
The OpenHardwareMonitor team welcomes feedback and contributions!<br/>
You can check if it works properly on your motherboard. For many manufacturers, the way of reading data differs a bit, so if you notice any inaccuracies, please send us a pull request. If you have any suggestions or improvements, don't hesitate to create an issue.

## License

OpenHardwareMonitor is free and open source software licensed under [MPL 2.0](https://www.mozilla.org/en-US/MPL/2.0/). You can use it for personal and commercial purposes.


