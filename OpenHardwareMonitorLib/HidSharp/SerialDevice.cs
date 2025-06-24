using System;

namespace HidSharp
{
    /// <summary>
    /// Represents a serial device.
    /// </summary>
    public abstract class SerialDevice : Device
    {
        /// <inheritdoc/>
        public new SerialStream Open()
        {
            return Open(null);
        }

        /// <inheritdoc/>
        public new SerialStream Open(OpenConfiguration openConfig)
        {
            return (SerialStream)base.Open(openConfig);
        }

        /// <inheritdoc/>
        public bool TryOpen(out SerialStream stream)
        {
            return TryOpen(null, out stream);
        }

        /// <inheritdoc/>
        public bool TryOpen(OpenConfiguration openConfig, out SerialStream stream)
        {
            DeviceStream baseStream;
            bool result = base.TryOpen(openConfig, out baseStream);
            stream = (SerialStream)baseStream; return result;
        }

        /// <inheritdoc/>
        public override string GetFriendlyName()
        {
            return GetFileSystemName();
        }

        public override bool HasImplementationDetail(Guid detail)
        {
            return base.HasImplementationDetail(detail) || detail == ImplementationDetail.SerialDevice;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            string fileSystemName = "(unknown filesystem name)";
            try { fileSystemName = GetFileSystemName(); } catch { }

            return string.Format("{0} ({1})", fileSystemName, DevicePath);
        }
    }
}
