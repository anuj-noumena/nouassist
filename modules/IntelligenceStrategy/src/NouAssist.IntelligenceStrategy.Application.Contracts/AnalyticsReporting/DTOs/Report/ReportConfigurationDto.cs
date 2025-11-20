using System;
using System.Collections.Generic;
using NouAssist.Analytics.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Analytics.DTOs.Report
{
    /// <summary>
    /// DTO for report configuration information
    /// </summary>
    public class ReportConfigurationDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public ReportFormat DefaultFormat { get; set; }
        
        public ReportScheduleDto Schedule { get; set; }
        
        public List<ReportParameterDto> Parameters { get; set; }
        
        public List<Guid> DataSources { get; set; }
        
        public List<string> DataSourceNames { get; set; }
        
        public List<ReportGenerationLogDto> RecentGenerationLogs { get; set; }
    }
}
