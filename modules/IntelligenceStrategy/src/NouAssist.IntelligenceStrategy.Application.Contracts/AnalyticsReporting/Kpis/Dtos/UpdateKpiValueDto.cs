using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Kpis.Dtos
{
    /// <summary>
    /// DTO for updating a KPI value
    /// </summary>
    public class UpdateKpiValueDto
    {
        /// <summary>
        /// The numeric value of the KPI
        /// </summary>
        [Required]
        public decimal Value { get; set; }
        
        /// <summary>
        /// The timestamp when this value was measured
        /// </summary>
        public DateTime? Timestamp { get; set; }
    }
}
