using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Interfaces.Services
{
    /// <summary>
    /// Defines operations for generating and managing recommendations
    /// </summary>
    public interface IRecommendationService
    {
        /// <summary>
        /// Generates task recommendations based on user context
        /// </summary>
        Task<List<RecommendationInstance>> GenerateTaskRecommendationsAsync(Guid userId);
        
        /// <summary>
        /// Generates workload optimization recommendations
        /// </summary>
        Task<RecommendationInstance> GenerateWorkloadOptimizationAsync(Guid userId);
        
        /// <summary>
        /// Generates schedule optimization recommendations
        /// </summary>
        Task<RecommendationInstance> OptimizeScheduleAsync(Guid userId, DateTime targetDate);
        
        /// <summary>
        /// Generates task prioritization recommendations
        /// </summary>
        Task<RecommendationInstance> SuggestTaskPrioritizationAsync(Guid userId);
        
        /// <summary>
        /// Identifies tasks suitable for automation
        /// </summary>
        Task<RecommendationInstance> SuggestTasksForAutomationAsync(Guid userId);
        
        /// <summary>
        /// Marks a recommendation as viewed
        /// </summary>
        Task MarkRecommendationAsViewedAsync(Guid recommendationId);
        
        /// <summary>
        /// Marks a recommendation as acted on
        /// </summary>
        Task MarkRecommendationAsActedOnAsync(Guid recommendationId);
        
        /// <summary>
        /// Marks a recommendation as dismissed
        /// </summary>
        Task MarkRecommendationAsDismissedAsync(Guid recommendationId);
        
        /// <summary>
        /// Sets user rating for a recommendation
        /// </summary>
        Task SetRecommendationRatingAsync(Guid recommendationId, int rating);
    }
}
