using System;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Reports.Dtos
{
    /// <summary>
    /// DTO for report data source view model
    /// </summary>
    public class ReportDataSourceDto
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
        public string Type { get; set; }
    }
}
