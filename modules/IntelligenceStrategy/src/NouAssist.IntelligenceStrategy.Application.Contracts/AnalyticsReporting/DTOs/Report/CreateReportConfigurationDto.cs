using System.ComponentModel.DataAnnotations;
using NouAssist.Analytics.Constants;
using NouAssist.Analytics.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;

namespace NouAssist.Analytics.DTOs.Report
{
    /// <summary>
    /// DTO for creating a new report configuration
    /// </summary>
    public class CreateReportConfigurationDto
    {
        [Required]
        [StringLength(AnalyticsConsts.Report.MaxNameLength)]
        public string Name { get; set; }
        
        [StringLength(AnalyticsConsts.Report.MaxDescriptionLength)]
        public string Description { get; set; }
        
        public ReportFormat DefaultFormat { get; set; }
        
        public ReportScheduleDto Schedule { get; set; }
    }
}
