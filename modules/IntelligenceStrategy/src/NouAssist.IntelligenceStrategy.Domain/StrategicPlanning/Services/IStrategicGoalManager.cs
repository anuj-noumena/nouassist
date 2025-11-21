using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Services
{
    /// <summary>
    /// Domain service interface for strategic goal management
    /// </summary>
    public interface IStrategicGoalManager
    {
        Task<StrategicGoalDefinition> CreateAsync(
            string title,
            string description,
            GoalPriority priority,
            PlanningHorizon horizon,
            DateTime targetDate,
            Guid? ownerId = null);

        Task<bool> IsTitleUniqueAsync(string title, Guid? existingGoalId = null);
        
        Task ValidateGoalTargetsAsync(StrategicGoalDefinition goal);
        
        Task<List<StrategicGoalDefinition>> GetGoalsRequiringAttentionAsync();
    }
}
