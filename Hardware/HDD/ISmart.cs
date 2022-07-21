using System;

namespace OpenHardwareMonitor.Hardware.HDD {

  internal interface ISmart {

    IntPtr OpenDrive(int driveNumber);

    bool EnableSmart(IntPtr handle, int driveNumber);

    DriveAttributeValue[] ReadSmartData(IntPtr handle, int driveNumber);

    DriveThresholdValue[] ReadSmartThresholds(IntPtr handle, int driveNumber);

    bool ReadNameAndFirmwareRevision(IntPtr handle, int driveNumber,
      out string name, out string firmwareRevision); 

    void CloseHandle(IntPtr handle);

    IntPtr InvalidHandle { get; }

    string[] GetLogicalDrives(int driveIndex);
  }
}
