using System;
using NouAssist.Analytics.Enums;

namespace NouAssist.Analytics.DTOs.Report
{
    /// <summary>
    /// DTO for report generation log information
    /// </summary>
    public class ReportGenerationLogDto
    {
        public Guid Id { get; set; }
        
        public DateTime GeneratedAt { get; set; }
        
        public Guid? GeneratedBy { get; set; }
        
        public string GeneratedByName { get; set; }
        
        public bool IsSuccessful { get; set; }
        
        public ReportFormat Format { get; set; }
        
        public string ErrorMessage { get; set; }
        
        public string StoragePath { get; set; }
        
        public bool IsScheduled { get; set; }
        
        public TimeSpan Duration { get; set; }
    }
}
