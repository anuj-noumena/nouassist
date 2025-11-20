using System;
using NouAssist.Analytics.Enums;

namespace NouAssist.Analytics.DTOs.Report
{
    /// <summary>
    /// DTO for a generated report
    /// </summary>
    public class GeneratedReportDto
    {
        public Guid ReportConfigurationId { get; set; }
        
        public string ReportName { get; set; }
        
        public string StoragePath { get; set; }
        
        public ReportFormat Format { get; set; }
        
        public DateTime GeneratedAt { get; set; }
        
        public bool IsSuccessful { get; set; }
        
        public string ErrorMessage { get; set; }
    }
}
