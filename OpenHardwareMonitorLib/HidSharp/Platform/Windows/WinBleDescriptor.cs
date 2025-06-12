using HidSharp.Experimental;

namespace HidSharp.Platform.Windows
{
    sealed class WinBleDescriptor : BleDescriptor
    {
        internal NativeMethods.BTH_LE_GATT_DESCRIPTOR NativeData;

        public WinBleDescriptor(NativeMethods.BTH_LE_GATT_DESCRIPTOR nativeData)
        {
            NativeData = nativeData;
        }

        public override BleUuid Uuid
        {
            get { return NativeData.DescriptorUuid.ToGuid(); }
        }
    }
}
