using System;
using System.Collections.Generic;
using System.Linq;

namespace HidSharp.Reports.Input
{
    // TODO: This works, but it's certainly not the most efficient. Rework it if we need better performance.
    //       We aren't processing *that* much data, though, so it may be OK.
    public class DeviceItemInputParser
    {
        DeviceItem _deviceItem;
        DataValue[] _oldDataValues;
        DataValue[] _newDataValues;
        Queue<int> _changedElements;
        int[][] _rangesOfElementsByIndexOfDataItem;
        int[] _rangeOfElementsByIndexOfReport;
        Dictionary<byte, int> _reportIDtoIndex;
        object _syncRoot;

        public DeviceItemInputParser(DeviceItem deviceItem)
        {
            Throw.If.Null(deviceItem);
            _deviceItem = deviceItem;
            _changedElements = new Queue<int>();
            _reportIDtoIndex = new Dictionary<byte, int>();

            int totalElements = 0;

            var rangesOfElementsByIndexOfDataItem = new List<int[]>();
            var rangeOfElementsByIndexOfReport = new List<int>() { 0 };
            int indexOfReport = 0;

            var dataValues = new List<DataValue>();
            foreach (var inputReport in deviceItem.InputReports)
            {
                _reportIDtoIndex[inputReport.ReportID] = indexOfReport;

                var rangeOfElementsByIndexOfDataItem = new List<int>() { 0 };
                int indexOfDataItem = 0;

                foreach (var dataItem in inputReport.DataItems)
                {
                    int elementCount = dataItem.ElementCount;
                    var usages = dataItem.Usages;
                    int usageCount = usages.Count;

                    if ((dataItem.IsArray && dataItem.LogicalMaximum - dataItem.LogicalMinimum + 1 == usageCount) ||
                        (!dataItem.IsArray && elementCount == usageCount))
                    {
                        for (int dataIndex = 0; dataIndex < usageCount; dataIndex++)
                        {
                            var dataValue = new DataValue() { DataItem = dataItem, DataIndex = dataIndex };
                            SetDefaultValue(ref dataValue);
                            dataValues.Add(dataValue);
                            totalElements++;
                        }
                    }

                    indexOfDataItem++;
                    rangeOfElementsByIndexOfDataItem.Add(totalElements);
                }

                indexOfReport++;
                rangesOfElementsByIndexOfDataItem.Add(rangeOfElementsByIndexOfDataItem.ToArray());
                rangeOfElementsByIndexOfReport.Add(totalElements);
            }

            _oldDataValues = dataValues.ToArray();
            _newDataValues = dataValues.ToArray();
            _rangesOfElementsByIndexOfDataItem = rangesOfElementsByIndexOfDataItem.ToArray();
            _rangeOfElementsByIndexOfReport = rangeOfElementsByIndexOfReport.ToArray();
            _syncRoot = new object();
        }

        void SetDefaultValue(ref DataValue dataValue)
        {
            // TODO: The Variable case, if all 32-bit values are OK, this won't work. Add a way to force IsNull.
            dataValue.SetLogicalValue(dataValue.DataItem.IsArray ? 0 : dataValue.DataItem.LogicalMaximum + 1);
        }

        /// <summary>
        /// Parses a received report.
        /// </summary>
        /// <param name="buffer">The buffer to read the report from.</param>
        /// <param name="offset">The offset to begin reading the report at.</param>
        /// <param name="report"><see cref="HidSharp.Reports.Report"/> the buffer conforms to.</param>
        /// <returns><c>true</c> if the report is for this <see cref="DeviceItem"/>.</returns>
        public bool TryParseReport(byte[] buffer, int offset, Report report)
        {
            Throw.If.Null(buffer).Null(report).OutOfRange(buffer, offset, report.Length);

            lock (_syncRoot)
            {
                int reportIndex;
                if (!_reportIDtoIndex.TryGetValue(report.ReportID, out reportIndex)) { return false; }

                int reportStartElement = _rangeOfElementsByIndexOfReport[reportIndex];
                int reportEndElement = _rangeOfElementsByIndexOfReport[reportIndex + 1];
                int reportElementCount = reportEndElement - reportStartElement;
                Array.Copy(_newDataValues, reportStartElement, _oldDataValues, reportStartElement, reportElementCount);
                for (int elementIndex = reportStartElement; elementIndex < reportEndElement; elementIndex++) { SetDefaultValue(ref _newDataValues[elementIndex]); }

                var rangeOfElementsByIndexOfDataItem = _rangesOfElementsByIndexOfDataItem[reportIndex];
                report.Read(buffer, offset, (reportBuffer, bitOffset, dataItem, indexOfDataItem) =>
                    {
                        // This usage value is of no use.
                        if (dataItem.Usages.GetAllValues().FirstOrDefault() == 0)
                        {
                            return;
                        }

                        int dataItemStartElement = rangeOfElementsByIndexOfDataItem[indexOfDataItem];
                        int dataItemEndElement = rangeOfElementsByIndexOfDataItem[indexOfDataItem + 1];
                        // We skipped this one.
                        if (dataItemStartElement == dataItemEndElement)
                        {
                            return;
                        }

                        int elementCount = dataItem.ElementCount;
                        for (int elementIndex = 0; elementIndex < elementCount; elementIndex++)
                        {
                            DataValue newDataValue;
                            if (dataItem.TryReadValue(reportBuffer, bitOffset, elementIndex, out newDataValue))
                            {
                                int indexOfElement = dataItemStartElement + newDataValue.DataIndex;
                                // This index is out of bounds.
                                if (indexOfElement >= _newDataValues.Count())
                                {
                                    continue;
                                }

                                _newDataValues[indexOfElement] = newDataValue;
                            }
                        }
                    });

                _changedElements.Clear();
                for (int indexOfElement = reportStartElement; indexOfElement < reportEndElement; indexOfElement++)
                {
                    var oldDataValue = _oldDataValues[indexOfElement];
                    var newDataValue = _newDataValues[indexOfElement];
                    if (oldDataValue.GetLogicalValue() != newDataValue.GetLogicalValue())
                    {
                        _changedElements.Enqueue(indexOfElement);
                    }
                }

                return true;
            }
        }

        public DataValue GetPreviousValue(int index)
        {
            return _oldDataValues[index];
        }

        public DataValue GetValue(int index)
        {
            return _newDataValues[index];
        }

        public int GetNextChangedIndex()
        {
            lock (_syncRoot)
            {
                return HasChanged ? _changedElements.Dequeue() : -1;
            }
        }

        public DeviceItem DeviceItem
        {
            get { return _deviceItem; }
        }

        /// <summary>
        /// The number of unique values in the <see cref="HidSharp.Reports.DeviceItem"/>.
        /// </summary>
        public int ValueCount
        {
            get { return _newDataValues.Length; }
        }

        public bool HasChanged
        {
            get { return _changedElements.Count > 0; }
        }
    }
}
