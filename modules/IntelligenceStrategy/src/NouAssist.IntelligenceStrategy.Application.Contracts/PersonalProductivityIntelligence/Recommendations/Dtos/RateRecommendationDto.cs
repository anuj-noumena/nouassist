using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Recommendations.Dtos
{
    /// <summary>
    /// DTO for rating a recommendation
    /// </summary>
    public class RateRecommendationDto
    {
        /// <summary>
        /// Recommendation ID
        /// </summary>
        [Required]
        public Guid RecommendationId { get; set; }
        
        /// <summary>
        /// Rating (1-5)
        /// </summary>
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
    }
}
