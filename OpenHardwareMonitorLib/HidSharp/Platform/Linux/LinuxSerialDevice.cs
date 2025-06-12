using System;

namespace HidSharp.Platform.Linux
{
    sealed class LinuxSerialDevice : SerialDevice
    {
        string _portName;

        protected override DeviceStream OpenDeviceDirectly(OpenConfiguration openConfig)
        {
            return new LinuxSerialStream(this);
        }

        internal static LinuxSerialDevice TryCreate(string portName)
        {
            return new LinuxSerialDevice() { _portName = portName };
        }

        public override string GetFileSystemName()
        {
            return _portName;
        }

        public override bool HasImplementationDetail(Guid detail)
        {
            return base.HasImplementationDetail(detail) || detail == ImplementationDetail.Linux;
        }

        public override string DevicePath
        {
            get { return _portName; }
        }
    }
}
