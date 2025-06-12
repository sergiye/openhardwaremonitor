using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HidSharp.Reports
{
    public class DescriptorItem
    {
        static readonly IList<DescriptorItem> _noChildren = new ReadOnlyCollection<DescriptorItem>(new DescriptorItem[0]);

        Indexes _designator, _string, _usage;

        public DescriptorItem()
        {

        }

        public virtual IList<DescriptorItem> ChildItems
        {
            get { return _noChildren; }
        }

        public DescriptorCollectionItem ParentItem
        {
            get;
            internal set;
        }

        public Indexes Designators
        {
            get { return _designator ?? Indexes.Unset; }
            set { _designator = value; }
        }

        public Indexes Strings
        {
            get { return _string ?? Indexes.Unset; }
            set { _string = value; }
        }

        public Indexes Usages
        {
            get { return _usage ?? Indexes.Unset; }
            set { _usage = value; }
        }
    }
}
