using System;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Performance.Dtos
{
    /// <summary>
    /// DTO for performance trend data point
    /// </summary>
    public class PerformanceTrendPointDto
    {
        /// <summary>
        /// Date of the data point
        /// </summary>
        public DateTime Date { get; set; }
        
        /// <summary>
        /// Value of the data point
        /// </summary>
        public double Value { get; set; }
    }
}
