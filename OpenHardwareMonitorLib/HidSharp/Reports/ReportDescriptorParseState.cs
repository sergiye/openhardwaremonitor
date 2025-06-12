using System.Collections.Generic;
using HidSharp.Reports.Encodings;

namespace HidSharp.Reports
{
    // TODO: Make this public if anyone finds value in doing so. For now let's not lock ourselves in.
    sealed class ReportDescriptorParseState
    {
        public ReportDescriptorParseState()
        {
            RootItem = new DescriptorCollectionItem();
            GlobalItemStateStack = new Stack<IDictionary<GlobalItemTag, EncodedItem>>();
            LocalItemState = new List<KeyValuePair<LocalItemTag, uint>>();
            Reset();
        }

        public void Reset()
        {
            CurrentCollectionItem = RootItem;
            RootItem.ChildItems.Clear();
            RootItem.CollectionType = 0;

            GlobalItemStateStack.Clear();
            GlobalItemStateStack.Push(new Dictionary<GlobalItemTag, EncodedItem>());
            LocalItemState.Clear();
        }

        public EncodedItem GetGlobalItem(GlobalItemTag tag)
        {
            EncodedItem value;
            GlobalItemState.TryGetValue(tag, out value);
            return value;
        }

        public uint GetGlobalItemValue(GlobalItemTag tag)
        {
            EncodedItem item = GetGlobalItem(tag);
            return item != null ? item.DataValue : 0;
        }

        public bool IsGlobalItemSet(GlobalItemTag tag)
        {
            return GlobalItemState.ContainsKey(tag);
        }

        public DescriptorCollectionItem CurrentCollectionItem
        {
            get;
            set;
        }

        public DescriptorCollectionItem RootItem
        {
            get;
            private set;
        }

        public IDictionary<GlobalItemTag, EncodedItem> GlobalItemState
        {
            get { return GlobalItemStateStack.Peek(); }
        }

        public Stack<IDictionary<GlobalItemTag, EncodedItem>> GlobalItemStateStack
        {
            get;
            private set;
        }

        public IList<KeyValuePair<LocalItemTag, uint>> LocalItemState
        {
            get;
            private set;
        }
    }
}
