using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace HidSharp.Experimental
{
    /// <summary>
    /// Represents a Bluetooth Low Energy device.
    /// </summary>
    [ComVisible(true), Guid("A7AEE7B8-893D-41B6-84F7-6BDA4EE3AA3F")]
    public abstract class BleDevice : Device
    {
        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new BleStream Open()
        {
            return (BleStream)base.Open();
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new BleStream Open(OpenConfiguration openConfig)
        {
            return (BleStream)base.Open(openConfig);
        }

        public BleStream Open(BleService service)
        {
            return Open(service, new OpenConfiguration());
        }

        public BleStream Open(BleService service, OpenConfiguration openConfig)
        {
            Throw.If.Null(service).Null(openConfig);

            openConfig = openConfig.Clone();
            openConfig.SetOption(OpenOption.BleService, service);
            return Open(openConfig);
        }

        /*
        public abstract bool GetConnectionState();
        */

        public abstract BleService[] GetServices();

        public BleService GetServiceOrNull(BleUuid uuid)
        {
            BleService service;
            return TryGetService(uuid, out service) ? service : null;
        }

        public virtual bool HasService(BleUuid uuid)
        {
            BleService service;
            return TryGetService(uuid, out service);
        }

        public virtual bool TryGetService(BleUuid uuid, out BleService service)
        {
            foreach (var s in GetServices())
            {
                if (s.Uuid == uuid) { service = s; return true; }
            }

            service = null; return false;
        }

        public override bool HasImplementationDetail(Guid detail)
        {
            return base.HasImplementationDetail(detail) || detail == ImplementationDetail.BleDevice;
        }

        public override string ToString()
        {
            string friendlyName = "(unknown friendly name)";
            try { friendlyName = GetFriendlyName(); }
            catch { }

            return friendlyName;
        }
    }
}
