using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.DataSources.Dtos
{
    /// <summary>
    /// DTO for data source view model
    /// </summary>
    public class DataSourceDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// Name of the data source
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the data source
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Type of data source
        /// </summary>
        public DataSourceType Type { get; set; }
        
        /// <summary>
        /// When this data source was last refreshed
        /// </summary>
        public DateTime? LastRefreshed { get; set; }
        
        /// <summary>
        /// Refresh interval setting
        /// </summary>
        public RefreshIntervalDto RefreshInterval { get; set; }
        
        /// <summary>
        /// Status of data validation
        /// </summary>
        public ValidationStatus ValidationStatus { get; set; }
        
        /// <summary>
        /// Schema definition summary (field names and types)
        /// </summary>
        public string SchemaDefinition { get; set; }
        
        /// <summary>
        /// Whether this data source is active
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Tags for organizing data sources
        /// </summary>
        public string[] Tags { get; set; }
        
        /// <summary>
        /// Transformation steps for this data source
        /// </summary>
        public DataTransformationStepDto[] TransformationSteps { get; set; }
    }
}
