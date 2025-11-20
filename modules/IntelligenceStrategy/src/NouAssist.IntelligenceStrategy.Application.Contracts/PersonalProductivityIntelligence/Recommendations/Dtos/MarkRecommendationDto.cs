using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Recommendations.Dtos
{
    /// <summary>
    /// DTO for marking a recommendation
    /// </summary>
    public class MarkRecommendationDto
    {
        /// <summary>
        /// Recommendation ID
        /// </summary>
        [Required]
        public Guid RecommendationId { get; set; }
    }
}
