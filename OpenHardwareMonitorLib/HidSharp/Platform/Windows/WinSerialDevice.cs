namespace HidSharp.Platform.Windows
{
    sealed class WinSerialDevice : SerialDevice
    {
        string _path;
        string _fileSystemName;
        string _friendlyName;

        protected override DeviceStream OpenDeviceDirectly(OpenConfiguration openConfig)
        {
            var stream = new WinSerialStream(this);
            stream.Init(DevicePath);
            return stream;
        }

        internal static WinSerialDevice TryCreate(string portName, string fileSystemName, string friendlyName)
        {
            return new WinSerialDevice() { _path = portName, _fileSystemName = fileSystemName, _friendlyName = friendlyName };
        }

        public override string GetFileSystemName()
        {
            return _fileSystemName;
        }

        public override string GetFriendlyName()
        {
            return _friendlyName;
        }

        public override string DevicePath
        {
            get { return _path; }
        }
    }
}
