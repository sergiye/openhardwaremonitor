using System;

namespace HidSharp.Platform.MacOS
{
    sealed class MacSerialDevice : SerialDevice
    {
        NativeMethods.io_string_t _path;
        string _fileSystemName;

        protected override DeviceStream OpenDeviceDirectly(OpenConfiguration openConfig)
        {
            return new MacSerialStream(this);
        }

        internal static MacSerialDevice TryCreate(NativeMethods.io_string_t path)
        {
            var d = new MacSerialDevice() { _path = path };

            var handle = NativeMethods.IORegistryEntryFromPath(0, ref path).ToIOObject();
            if (!handle.IsSet) { return null; }

            using (handle)
            {
                d._fileSystemName = NativeMethods.IORegistryEntryGetCFProperty_String(handle, NativeMethods.kIOCalloutDeviceKey);
                if (d._fileSystemName == null) { return null; }
            }

            return d;
        }

        public override string GetFileSystemName()
        {
            return _fileSystemName;
        }

        public override bool HasImplementationDetail(Guid detail)
        {
            return base.HasImplementationDetail(detail) || detail == ImplementationDetail.MacOS;
        }

        public override string DevicePath
        {
            get { return _path.ToString(); }
        }
    }
}
