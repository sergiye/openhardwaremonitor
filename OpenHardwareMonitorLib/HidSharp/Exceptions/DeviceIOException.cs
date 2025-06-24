using System.IO;

namespace HidSharp.Exceptions
{
    sealed class DeviceIOException : IOException, IDeviceException
    {
        public DeviceIOException(Device device, string message)
            : base(message)
        {
            Device = device;
        }

        public DeviceIOException(Device device, string message, int hresult)
            : base(message, hresult)
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
