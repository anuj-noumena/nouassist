using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Recommendations.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Recommendations
{
    /// <summary>
    /// Application service for recommendations
    /// </summary>
    public interface IRecommendationAppService : IApplicationService
    {
        /// <summary>
        /// Gets a recommendation by ID
        /// </summary>
        Task<RecommendationDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets recommendations for a user
        /// </summary>
        Task<PagedResultDto<RecommendationDto>> GetListAsync(GetRecommendationsDto input);
        
        /// <summary>
        /// Generates task recommendations
        /// </summary>
        Task<PagedResultDto<RecommendationDto>> GenerateTaskRecommendationsAsync(Guid userId);
        
        /// <summary>
        /// Generates workload optimization recommendation
        /// </summary>
        Task<RecommendationDto> GenerateWorkloadOptimizationAsync(Guid userId);
        
        /// <summary>
        /// Generates schedule optimization recommendation
        /// </summary>
        Task<RecommendationDto> OptimizeScheduleAsync(Guid userId, DateTime targetDate);
        
        /// <summary>
        /// Generates task prioritization recommendation
        /// </summary>
        Task<RecommendationDto> SuggestTaskPrioritizationAsync(Guid userId);
        
        /// <summary>
        /// Suggests tasks for automation
        /// </summary>
        Task<RecommendationDto> SuggestTasksForAutomationAsync(Guid userId);
        
        /// <summary>
        /// Marks a recommendation as viewed
        /// </summary>
        Task MarkAsViewedAsync(MarkRecommendationDto input);
        
        /// <summary>
        /// Marks a recommendation as acted on
        /// </summary>
        Task MarkAsActedOnAsync(MarkRecommendationDto input);
        
        /// <summary>
        /// Marks a recommendation as dismissed
        /// </summary>
        Task MarkAsDismissedAsync(MarkRecommendationDto input);
        
        /// <summary>
        /// Rates a recommendation
        /// </summary>
        Task RateRecommendationAsync(RateRecommendationDto input);
    }
}
