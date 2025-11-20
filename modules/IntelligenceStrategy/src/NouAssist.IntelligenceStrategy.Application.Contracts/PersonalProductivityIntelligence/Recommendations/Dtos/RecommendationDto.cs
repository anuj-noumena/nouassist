using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Recommendations.Dtos
{
    /// <summary>
    /// DTO for recommendations
    /// </summary>
    public class RecommendationDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// User ID
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Type of recommendation
        /// </summary>
        public string RecommendationType { get; set; }
        
        /// <summary>
        /// Title of the recommendation
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Description of the recommendation
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// When the recommendation was generated
        /// </summary>
        public DateTime GeneratedAt { get; set; }
        
        /// <summary>
        /// When the recommendation expires
        /// </summary>
        public DateTime? ExpiresAt { get; set; }
        
        /// <summary>
        /// Priority value (0-100)
        /// </summary>
        public int PriorityValue { get; set; }
        
        /// <summary>
        /// Priority level as a readable category
        /// </summary>
        public string PriorityLevel { get; set; }
        
        /// <summary>
        /// Whether the recommendation has been viewed
        /// </summary>
        public bool IsViewed { get; set; }
        
        /// <summary>
        /// Whether the recommendation has been acted on
        /// </summary>
        public bool IsActedOn { get; set; }
        
        /// <summary>
        /// When the recommendation was acted on
        /// </summary>
        public DateTime? ActedOnAt { get; set; }
        
        /// <summary>
        /// Whether the recommendation has been dismissed
        /// </summary>
        public bool IsDismissed { get; set; }
        
        /// <summary>
        /// When the recommendation was dismissed
        /// </summary>
        public DateTime? DismissedAt { get; set; }
        
        /// <summary>
        /// User rating (1-5)
        /// </summary>
        public int? UserRating { get; set; }
        
        /// <summary>
        /// Target entity type
        /// </summary>
        public string TargetEntityType { get; set; }
        
        /// <summary>
        /// Target entity ID
        /// </summary>
        public Guid? TargetEntityId { get; set; }
        
        /// <summary>
        /// Rules that generated this recommendation
        /// </summary>
        public List<RecommendationRuleDto> Rules { get; set; }
        
        /// <summary>
        /// Explainability logs
        /// </summary>
        public List<ExplainabilityLogDto> ExplainabilityLogs { get; set; }
    }
}
