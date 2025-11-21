using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Services
{
    /// <summary>
    /// Domain service interface for transition management
    /// </summary>
    public interface ITransitionManagementService
    {
        Task<TransitionPlan> CreateTransitionPlanAsync(
            string planName,
            string description,
            TransitionType type,
            DateTime plannedStartDate,
            DateTime estimatedCompletionDate,
            decimal estimatedCost = 0);

        Task<decimal> CalculateTransitionReadinessAsync(Guid transitionPlanId);
        
        Task<List<TransitionPlan>> GetPlansRequiringAttentionAsync();
    }
}
