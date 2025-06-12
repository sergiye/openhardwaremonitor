namespace HidSharp.Reports.Encodings
{
    public enum GlobalItemTag : byte
    {
        UsagePage = 0,
        LogicalMinimum,
        LogicalMaximum,
        PhysicalMinimum,
        PhysicalMaximum,
        UnitExponent,
        Unit,
        ReportSize,
        ReportID,
        ReportCount,
        Push,
        Pop
    }
}
