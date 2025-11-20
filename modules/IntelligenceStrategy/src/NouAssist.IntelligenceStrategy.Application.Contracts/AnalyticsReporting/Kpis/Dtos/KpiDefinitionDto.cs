using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Kpis.Dtos
{
    /// <summary>
    /// DTO for KPI definition view model
    /// </summary>
    public class KpiDefinitionDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// Name of the KPI
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Description of what the KPI measures
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Category of the KPI
        /// </summary>
        public string Category { get; set; }
        
        /// <summary>
        /// Units of measurement for the KPI
        /// </summary>
        public string Unit { get; set; }
        
        /// <summary>
        /// Data sources used to calculate the KPI
        /// </summary>
        public KpiDataSourceDto[] DataSources { get; set; }
        
        /// <summary>
        /// Format string for displaying the KPI value
        /// </summary>
        public string DisplayFormat { get; set; }
        
        /// <summary>
        /// Refresh interval settings
        /// </summary>
        public RefreshIntervalDto RefreshInterval { get; set; }
        
        /// <summary>
        /// The current value of the KPI
        /// </summary>
        public KpiValueDto CurrentValue { get; set; }
        
        /// <summary>
        /// Whether this KPI is active
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Tags for organizing KPIs
        /// </summary>
        public string[] Tags { get; set; }
        
        /// <summary>
        /// Thresholds for the KPI
        /// </summary>
        public KpiThresholdDto[] Thresholds { get; set; }
    }
}
