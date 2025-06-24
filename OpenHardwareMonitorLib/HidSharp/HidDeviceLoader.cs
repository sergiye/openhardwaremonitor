using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace HidSharp
{
    /// <exclude />
    [ComVisible(true), Guid("CD7CBD7D-7204-473c-AA2A-2B9622CFC6CC")]
    [Obsolete, EditorBrowsable(EditorBrowsableState.Never)]
    public class HidDeviceLoader
    {
        /// <exclude />
        [Obsolete, EditorBrowsable(EditorBrowsableState.Never)]
        public HidDeviceLoader()
        {

        }

        /// <exclude />
        [Obsolete, EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerable GetDevicesVB()
        {
            return DeviceList.Local.GetHidDevices();
        }

        /// <exclude />
        [Obsolete, EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerable<HidDevice> GetDevices()
        {
            return DeviceList.Local.GetHidDevices();
        }

        /// <exclude />
        [Obsolete, EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerable<HidDevice> GetDevices(int? vendorID = null, int? productID = null, int? productVersion = null, string serialNumber = null)
        {
            return DeviceList.Local.GetHidDevices(vendorID, productID, productVersion, serialNumber);
        }

        /// <exclude />
        [Obsolete, EditorBrowsable(EditorBrowsableState.Never)]
        public HidDevice GetDeviceOrDefault(int? vendorID = null, int? productID = null, int? productVersion = null, string serialNumber = null)
        {
            return DeviceList.Local.GetHidDeviceOrNull(vendorID, productID, productVersion, serialNumber);
        }
    }
}
