using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Reports.Dtos
{
    /// <summary>
    /// DTO for generating a report
    /// </summary>
    public class GenerateReportDto
    {
        /// <summary>
        /// Format to generate the report in
        /// </summary>
        public ReportFormat Format { get; set; }
        
        /// <summary>
        /// Parameter values for the report
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; }
    }
}
