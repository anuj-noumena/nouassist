using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Services.Interfaces
{
    /// <summary>
    /// Service for analyzing conversion funnels
    /// </summary>
    public interface IFunnelAnalysisService
    {
        /// <summary>
        /// Creates a new conversion funnel
        /// </summary>
        Task<ConversionFunnelDefinition> CreateFunnelAsync(
            string name,
            string description);
            
        /// <summary>
        /// Adds a stage to a funnel
        /// </summary>
        Task<FunnelStage> AddFunnelStageAsync(
            Guid funnelId,
            string name,
            string description,
            int order,
            string criteria,
            bool isExitStage = false);
            
        /// <summary>
        /// Updates conversion rate data for a funnel stage
        /// </summary>
        Task UpdateStageConversionRateAsync(
            Guid funnelId,
            Guid stageId,
            int enteredCount,
            int completedCount);
            
        /// <summary>
        /// Gets overall funnel performance metrics
        /// </summary>
        Task<Dictionary<string, object>> GetFunnelPerformanceAsync(Guid funnelId);
            
        /// <summary>
        /// Validates a funnel's configuration
        /// </summary>
        Task ValidateFunnelAsync(Guid funnelId);
    }
}
