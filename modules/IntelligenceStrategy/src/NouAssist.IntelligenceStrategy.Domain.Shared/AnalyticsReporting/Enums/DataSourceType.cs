namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums
{
    /// <summary>
    /// Types of data sources that can be used for reporting
    /// </summary>
    public enum DataSourceType
    {
        EventStream = 1,
        DatabaseView = 2,
        API = 3,
        FileImport = 4,
        AggregatedDataView = 5,
        CustomDataSource = 6
    }
}
