using System;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Exceptions
{
    /// <summary>
    /// Exception thrown when a data source is not found
    /// </summary>
    public class DataSourceNotFoundException : AnalyticsDomainException
    {
        public Guid DataSourceId { get; }
        
        public DataSourceNotFoundException(Guid dataSourceId)
            : base("Analytics:ErrorMessage:DataSourceNotFound")
        {
            DataSourceId = dataSourceId;
            WithData("dataSourceId", dataSourceId);
        }
    }
}
