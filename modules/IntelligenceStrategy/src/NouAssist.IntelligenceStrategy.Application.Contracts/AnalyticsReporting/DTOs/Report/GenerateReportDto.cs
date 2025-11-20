using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.Analytics.Enums;

namespace NouAssist.Analytics.DTOs.Report
{
    /// <summary>
    /// DTO for generating a report
    /// </summary>
    public class GenerateReportDto
    {
        [Required]
        public Dictionary<string, string> ParameterValues { get; set; }
        
        public ReportFormat? Format { get; set; }
    }
}
