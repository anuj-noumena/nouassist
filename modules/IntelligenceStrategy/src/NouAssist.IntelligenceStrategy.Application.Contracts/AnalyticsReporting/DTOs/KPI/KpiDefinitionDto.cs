using System;
using System.Collections.Generic;
using NouAssist.Analytics.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Analytics.DTOs.KPI
{
    /// <summary>
    /// DTO for KPI definition information
    /// </summary>
    public class KpiDefinitionDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string CalculationExpression { get; set; }
        
        public decimal CurrentValue { get; set; }
        
        public KpiStatus Status { get; set; }
        
        public DateTime LastUpdated { get; set; }
        
        public List<KpiThresholdDto> Thresholds { get; set; }
        
        public List<Guid> DataSources { get; set; }
        
        public List<string> DataSourceNames { get; set; }
        
        public List<KpiAlertLogDto> RecentAlerts { get; set; }
    }
}
