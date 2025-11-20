using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Recommendations.Dtos
{
    /// <summary>
    /// DTO for getting recommendations
    /// </summary>
    public class GetRecommendationsDto : PagedResultRequestDto
    {
        /// <summary>
        /// User ID
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Filter by recommendation type
        /// </summary>
        public string RecommendationType { get; set; }
        
        /// <summary>
        /// Whether to include only active recommendations
        /// </summary>
        public bool OnlyActive { get; set; } = true;
        
        /// <summary>
        /// Minimum priority value
        /// </summary>
        public int? MinPriority { get; set; }
    }
}
