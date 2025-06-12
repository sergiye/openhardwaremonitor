using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HidSharp
{
    public class FilteredDeviceList : DeviceList
    {
        int _dirty;
        List<Func<bool>> _areDriversBeingInstalled;
        Dictionary<Device, int> _refCounts;

        public FilteredDeviceList()
        {
            _areDriversBeingInstalled = new List<Func<bool>>();
            _refCounts = new Dictionary<Device, int>();
        }

        /*
        public override BleDiscovery BeginBleDiscovery()
        {
            throw new NotImplementedException();
        }
        */

        public void Add(Device device)
        {
            Throw.If.Null(device, "device");

            lock (_refCounts)
            {
                IncrementRefCount(device);
            }

            RaiseChangedIfDirty();
        }

        public void Add(DeviceList deviceList)
        {
            Add(deviceList, device => true);
        }

        public void Add(DeviceList deviceList, DeviceFilter filter)
        {
            Throw.If.Null(deviceList, "deviceList").Null(filter, "filter");

            var oldDevices = new Device[0];
            Action updateDeviceList = () =>
            {
                var newDevices = deviceList.GetAllDevices(filter).ToArray();

                lock (_refCounts)
                {
                    foreach (var newDevice in newDevices)
                    {
                        IncrementRefCount(newDevice);
                    }

                    foreach (var oldDevice in oldDevices)
                    {
                        DecrementRefCount(oldDevice);
                    }
                }

                oldDevices = newDevices;
                RaiseChangedIfDirty();
            };

            _areDriversBeingInstalled.Add(() => deviceList.AreDriversBeingInstalled);
            deviceList.Changed += (sender, e) => updateDeviceList();
            updateDeviceList();
        }

        /// <inheritdoc/>
        public override IEnumerable<Device> GetAllDevices()
        {
            lock (_refCounts)
            {
                return _refCounts.Keys.ToList();
            }
        }

        void IncrementRefCount(Device device)
        {
            if (_refCounts.ContainsKey(device))
            {
                _refCounts[device]++;
            }
            else
            {
                _refCounts[device] = 1; _dirty = 1;
            }
        }

        void DecrementRefCount(Device device)
        {
            if (--_refCounts[device] == 0)
            {
                _refCounts.Remove(device); _dirty = 1;
            }
        }

        void RaiseChangedIfDirty()
        {
            if (1 == Interlocked.CompareExchange(ref _dirty, 0, 1))
            {
                RaiseChanged();
            }
        }

        /// <inheritdoc/>
        public override bool AreDriversBeingInstalled
        {
            get { return _areDriversBeingInstalled.Any(callback => callback()); }
        }
    }
}
