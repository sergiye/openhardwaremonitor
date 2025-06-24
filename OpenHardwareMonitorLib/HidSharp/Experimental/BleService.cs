namespace HidSharp.Experimental
{
    public abstract class BleService
    {
        public override string ToString()
        {
            return Uuid.ToString();
        }

        public abstract BleCharacteristic[] GetCharacteristics();

        public BleCharacteristic GetCharacteristicOrNull(BleUuid uuid)
        {
            BleCharacteristic characteristic;
            return TryGetCharacteristic(uuid, out characteristic) ? characteristic : null;
        }

        public virtual bool HasCharacteristic(BleUuid uuid)
        {
            BleCharacteristic characteristic;
            return TryGetCharacteristic(uuid, out characteristic);
        }

        public virtual bool TryGetCharacteristic(BleUuid uuid, out BleCharacteristic characteristic)
        {
            foreach (var c in GetCharacteristics())
            {
                if (c.Uuid == uuid) { characteristic = c; return true; }
            }

            characteristic = null; return false;
        }

        public abstract BleDevice Device
        {
            get;
        }

        public abstract BleUuid Uuid
        {
            get;
        }
    }
}
