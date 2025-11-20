using System;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Events
{
    /// <summary>
    /// Event emitted when a new recommendation is generated for a user
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.RecommendationGenerated")]
    public class RecommendationGeneratedEto : ProductivityIntelligenceEtoBase
    {
        /// <summary>
        /// ID of the recommendation instance
        /// </summary>
        public Guid RecommendationId { get; set; }
        
        /// <summary>
        /// Type of recommendation
        /// </summary>
        public RecommendationType RecommendationType { get; set; }
        
        /// <summary>
        /// Title of the recommendation
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Priority score of the recommendation (0-100)
        /// </summary>
        public int PriorityScore { get; set; }
    }
}
