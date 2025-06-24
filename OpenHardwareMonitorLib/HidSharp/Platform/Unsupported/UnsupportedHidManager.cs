using System;

namespace HidSharp.Platform.Unsupported
{
    sealed class UnsupportedHidManager : HidManager
    {
        protected override object[] GetBleDeviceKeys()
        {
            return new object[0];
        }

        protected override object[] GetHidDeviceKeys()
        {
            return new object[0];
        }

        protected override object[] GetSerialDeviceKeys()
        {
            return new object[0];
        }

        protected override bool TryCreateBleDevice(object key, out Device device)
        {
            throw new NotImplementedException();
        }

        protected override bool TryCreateHidDevice(object key, out Device device)
        {
            throw new NotImplementedException();
        }

        protected override bool TryCreateSerialDevice(object key, out Device device)
        {
            throw new NotImplementedException();
        }

        public override string FriendlyName
        {
            get { return "Platform Not Supported"; }
        }

        public override bool IsSupported
        {
            get { return true; }
        }
    }
}
