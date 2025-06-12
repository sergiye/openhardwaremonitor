using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using HidSharp.Reports.Encodings;

namespace HidSharp.Reports
{
    public class DescriptorCollectionItem : DescriptorItem
    {
        ReportCollectionItemChildren _children;

        public DescriptorCollectionItem()
        {
            _children = new ReportCollectionItemChildren(this);
        }

        public override IList<DescriptorItem> ChildItems
        {
            get { return _children; }
        }

        public CollectionType CollectionType
        {
            get;
            set;
        }

        #region ReportCollectionItemChildren
        sealed class ReportCollectionItemChildren : Collection<DescriptorItem>
        {
            DescriptorCollectionItem _item;

            public ReportCollectionItemChildren(DescriptorCollectionItem item)
            {
                Debug.Assert(item != null);
                _item = item;
            }

            protected override void ClearItems()
            {
                foreach (var item in this) { item.ParentItem = null; }
                base.ClearItems();
            }

            protected override void InsertItem(int index, DescriptorItem item)
            {
                Throw.If.Null(item).False(item.ParentItem == null);
                item.ParentItem = _item;
                base.InsertItem(index, item);
            }

            protected override void RemoveItem(int index)
            {
                this[index].ParentItem = null;
                base.RemoveItem(index);
            }

            protected override void SetItem(int index, DescriptorItem item)
            {
                throw new NotImplementedException();
            }
        }
        #endregion
    }
}
