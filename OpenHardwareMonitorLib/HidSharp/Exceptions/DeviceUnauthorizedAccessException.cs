using System;

namespace HidSharp.Exceptions
{
    sealed class DeviceUnauthorizedAccessException : UnauthorizedAccessException, IDeviceException
    {
        public DeviceUnauthorizedAccessException(Device device, string message)
            : base(message)
        {
            Device = device;
        }

        public Device Device
        {
            get;
            private set;
        }
    }
}
