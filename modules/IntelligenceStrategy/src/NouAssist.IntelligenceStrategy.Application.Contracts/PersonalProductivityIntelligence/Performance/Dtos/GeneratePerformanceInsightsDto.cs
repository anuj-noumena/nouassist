using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Performance.Dtos
{
    /// <summary>
    /// DTO for generating performance insights
    /// </summary>
    public class GeneratePerformanceInsightsDto
    {
        /// <summary>
        /// User ID
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Start date for the insights
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// End date for the insights
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// Maximum number of insights to generate
        /// </summary>
        [Range(1, 10)]
        public int MaxInsights { get; set; } = 5;
    }
}
