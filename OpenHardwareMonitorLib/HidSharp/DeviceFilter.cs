namespace HidSharp
{
    public delegate bool DeviceFilter(Device device);

    public static class DeviceFilterHelper
    {
        public static bool MatchHidDevices(Device device, int? vendorID = null, int? productID = null, int? releaseNumberBcd = null, string serialNumber = null)
        {
            var hidDevice = device as HidDevice;
            if (hidDevice != null)
            {
                int vid = vendorID ?? -1, pid = productID ?? -1, ver = releaseNumberBcd ?? -1;

                if ((vid < 0 || hidDevice.VendorID == vendorID) &&
                    (pid < 0 || hidDevice.ProductID == productID) &&
                    (ver < 0 || hidDevice.ReleaseNumberBcd == releaseNumberBcd))
                {
                    try
                    {
                        if (string.IsNullOrEmpty(serialNumber) || hidDevice.GetSerialNumber() == serialNumber) { return true; }
                    }
                    catch
                    {

                    }
                }
            }

            return false;
        }

        public static bool MatchSerialDevices(Device device, string portName = null)
        {
            var serialDevice = device as SerialDevice;
            if (serialDevice != null)
            {
                if (string.IsNullOrEmpty(portName) || serialDevice.DevicePath == portName)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
