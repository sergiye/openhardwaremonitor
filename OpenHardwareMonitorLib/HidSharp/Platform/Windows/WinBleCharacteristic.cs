using HidSharp.Experimental;

namespace HidSharp.Platform.Windows
{
    sealed class WinBleCharacteristic : Experimental.BleCharacteristic
    {
        internal NativeMethods.BTH_LE_GATT_CHARACTERISTIC NativeData;

        internal WinBleDescriptor[] _characteristicDescriptors;
        BleCharacteristicProperties _properties;

        public WinBleCharacteristic(NativeMethods.BTH_LE_GATT_CHARACTERISTIC nativeData)
        {
            NativeData = nativeData;

            _properties = (nativeData.IsBroadcastable != 0 ? BleCharacteristicProperties.Broadcast : 0)
                | (nativeData.IsReadable != 0 ? BleCharacteristicProperties.Read : 0)
                | (nativeData.IsWritableWithoutResponse != 0 ? BleCharacteristicProperties.WriteWithoutResponse : 0)
                | (nativeData.IsWritable != 0 ? BleCharacteristicProperties.Write : 0)
                | (nativeData.IsNotifiable != 0 ? BleCharacteristicProperties.Notify : 0)
                | (nativeData.IsIndicatable != 0 ? BleCharacteristicProperties.Indicate : 0)
                | (nativeData.IsSignedWritable != 0 ? BleCharacteristicProperties.SignedWrite : 0)
                | (nativeData.HasExtendedProperties != 0 ? BleCharacteristicProperties.ExtendedProperties : 0)
                ;
        }

        public override BleDescriptor[] GetDescriptors()
        {
            return (BleDescriptor[])_characteristicDescriptors.Clone();
        }

        public override BleUuid Uuid
        {
            get { return NativeData.CharacteristicUuid.ToGuid(); }
        }

        public override BleCharacteristicProperties Properties
        {
            get { return _properties; }
        }
    }
}
