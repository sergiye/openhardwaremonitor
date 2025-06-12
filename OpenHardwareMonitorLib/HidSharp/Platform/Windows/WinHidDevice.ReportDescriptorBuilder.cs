using System.Collections.Generic;
using HidSharp.Reports.Encodings;

namespace HidSharp.Platform.Windows
{
    partial class WinHidDevice
    {
        sealed class ReportDescriptorBuilder
        {
            Dictionary<GlobalItemTag, uint> _globals;
            List<EncodedItem> _items;

            public ReportDescriptorBuilder()
            {
                _globals = new Dictionary<GlobalItemTag, uint>();
                _items = new List<EncodedItem>();
            }

            public void AddGlobalItem(GlobalItemTag globalItemTag, uint dataValue)
            {
                uint oldDataValue;
                if (_globals.TryGetValue(globalItemTag, out oldDataValue) && oldDataValue == dataValue) { return; }
                _globals[globalItemTag] = dataValue;

                var item = new EncodedItem() { ItemType = ItemType.Global, TagForGlobal = globalItemTag, DataValue = dataValue };
                _items.Add(item);
            }

            public void AddGlobalItemSigned(GlobalItemTag globalItemTag, int dataValue)
            {
                uint oldDataValue;
                if (_globals.TryGetValue(globalItemTag, out oldDataValue) && oldDataValue == (uint)dataValue) { return; }
                _globals[globalItemTag] = (uint)dataValue;

                var item = new EncodedItem() { ItemType = ItemType.Global, TagForGlobal = globalItemTag, DataValueSigned = dataValue };
                _items.Add(item);
            }

            public void AddLocalItem(LocalItemTag localItemTag, uint dataValue)
            {
                _items.Add(new EncodedItem() { ItemType = ItemType.Local, TagForLocal = localItemTag, DataValue = dataValue });
            }

            public void AddMainItem(MainItemTag mainItemTag, uint dataValue)
            {
                _items.Add(new EncodedItem() { ItemType = ItemType.Main, TagForMain = mainItemTag, DataValue = dataValue });
            }

            public byte[] GetReportDescriptor()
            {
                var bytes = new List<byte>();
                EncodedItem.EncodeItems(_items, bytes);
                return bytes.ToArray();
            }
        }
    }
}
