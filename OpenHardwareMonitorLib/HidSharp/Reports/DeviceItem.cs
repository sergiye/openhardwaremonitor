using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace HidSharp.Reports
{
    public class DeviceItem : DescriptorCollectionItem
    {
        ReportCollectionItemReports _reports;

        public DeviceItem()
        {
            _reports = new ReportCollectionItemReports(this);
        }

        /// <summary>
        /// Creates a <see cref="HidSharp.Reports.Input.DeviceItemInputParser"/> appropriate for parsing reports for this device item.
        /// </summary>
        /// <returns>The new <see cref="HidSharp.Reports.Input.DeviceItemInputParser"/>.</returns>
        /// <remarks>
        /// Pair this with a <see cref="HidSharp.Reports.Input.HidDeviceInputReceiver"/> for the <see cref="ReportDescriptor"/>.
        /// </remarks>
        public Input.DeviceItemInputParser CreateDeviceItemInputParser()
        {
            return new Input.DeviceItemInputParser(this);
        }

        public IList<Report> Reports
        {
            get { return _reports; }
        }

        public IEnumerable<Report> InputReports
        {
            get { return Reports.Where(report => report.ReportType == ReportType.Input); }
        }

        public IEnumerable<Report> OutputReports
        {
            get { return Reports.Where(report => report.ReportType == ReportType.Output); }
        }

        public IEnumerable<Report> FeatureReports
        {
            get { return Reports.Where(report => report.ReportType == ReportType.Feature); }
        }

        #region ReportCollectionItemReports
        sealed class ReportCollectionItemReports : Collection<Report>
        {
            DeviceItem _item;

            public ReportCollectionItemReports(DeviceItem item)
            {
                Debug.Assert(item != null);
                _item = item;
            }

            protected override void ClearItems()
            {
                foreach (var item in this) { item.DeviceItem = null; }
                base.ClearItems();
            }

            protected override void InsertItem(int index, Report item)
            {
                Throw.If.Null(item).False(item.DeviceItem == null);
                item.DeviceItem = _item;
                base.InsertItem(index, item);
            }

            protected override void RemoveItem(int index)
            {
                this[index].DeviceItem = null;
                base.RemoveItem(index);
            }

            protected override void SetItem(int index, Report item)
            {
                throw new NotImplementedException();
            }
        }
        #endregion
    }
}
