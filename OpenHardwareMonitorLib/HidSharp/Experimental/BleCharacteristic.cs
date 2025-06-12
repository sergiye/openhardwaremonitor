namespace HidSharp.Experimental
{
    public abstract class BleCharacteristic
    {
        public override string ToString()
        {
            return string.Format("{0} (properties: {1})", Uuid, Properties);
        }

        public abstract BleDescriptor[] GetDescriptors();

        public bool HasDescriptor(BleUuid uuid)
        {
            BleDescriptor descriptor;
            return TryGetDescriptor(uuid, out descriptor);
        }

        public BleDescriptor GetDescriptorOrNull(BleUuid uuid)
        {
            BleDescriptor descriptor;
            return TryGetDescriptor(uuid, out descriptor) ? descriptor : null;
        }

        public virtual bool TryGetDescriptor(BleUuid uuid, out BleDescriptor descriptor)
        {
            foreach (var d in GetDescriptors())
            {
                if (d.Uuid == uuid) { descriptor = d; return true; }
            }

            descriptor = null; return false;
        }

        public abstract BleUuid Uuid
        {
            get;
        }

        public abstract BleCharacteristicProperties Properties
        {
            get;
        }

        public bool IsReadable
        {
            get { return (Properties & BleCharacteristicProperties.Read) != 0; }
        }

        public bool IsWritable
        {
            get { return (Properties & BleCharacteristicProperties.Write) != 0; }
        }

        public bool IsWritableWithoutResponse
        {
            get { return (Properties & BleCharacteristicProperties.WriteWithoutResponse) != 0; }
        }

        public bool IsNotifiable
        {
            get { return (Properties & BleCharacteristicProperties.Notify) != 0; }
        }

        public bool IsIndicatable
        {
            get { return (Properties & BleCharacteristicProperties.Indicate) != 0; }
        }
    }
}
