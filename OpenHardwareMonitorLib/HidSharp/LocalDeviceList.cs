using System.Collections.Generic;

namespace HidSharp
{
    sealed class LocalDeviceList : DeviceList
    {
        /*
        public override BleDiscovery BeginBleDiscovery()
        {
            return Platform.HidSelector.Instance.BeginBleDiscovery();
        }
        */

        public override IEnumerable<Device> GetDevices(DeviceTypes types)
        {
            return Platform.HidSelector.Instance.GetDevices(types);
        }

        public override IEnumerable<Device> GetAllDevices()
        {
            return GetDevices(DeviceTypes.Hid | DeviceTypes.Serial | DeviceTypes.Ble);
        }

        public override string ToString()
        {
            return Platform.HidSelector.Instance.FriendlyName; // This value is useful for debugging.
        }

        public override bool AreDriversBeingInstalled
        {
            get { return Platform.HidSelector.Instance.AreDriversBeingInstalled; }
        }
    }
}
