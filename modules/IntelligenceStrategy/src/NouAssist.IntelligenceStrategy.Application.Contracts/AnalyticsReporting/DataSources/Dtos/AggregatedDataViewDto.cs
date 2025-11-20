using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.DataSources.Dtos
{
    /// <summary>
    /// DTO for aggregated data view view model
    /// </summary>
    public class AggregatedDataViewDto : EntityDto<Guid>
    {
        /// <summary>
        /// Name of the aggregated view
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Description of what this view represents
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Source data sources for this view
        /// </summary>
        public AggregatedDataSourceDto[] SourceDataSources { get; set; }
        
        /// <summary>
        /// When this view was last refreshed
        /// </summary>
        public DateTime LastRefreshed { get; set; }
        
        /// <summary>
        /// Refresh interval setting
        /// </summary>
        public RefreshIntervalDto RefreshInterval { get; set; }
        
        /// <summary>
        /// Status of data validation
        /// </summary>
        public ValidationStatus ValidationStatus { get; set; }
    }
    
    /// <summary>
    /// DTO for a data source used in an aggregated view
    /// </summary>
    public class AggregatedDataSourceDto
    {
        /// <summary>
        /// ID of the data source
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Name of the data source
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Type of the data source
        /// </summary>
        public DataSourceType Type { get; set; }
    }
}
