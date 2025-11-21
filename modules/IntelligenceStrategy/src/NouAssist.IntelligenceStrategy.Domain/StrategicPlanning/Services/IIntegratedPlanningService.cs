using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Entities;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Services
{
    /// <summary>
    /// Domain service interface for integrated planning operations
    /// </summary>
    public interface IIntegratedPlanningService
    {
        Task<IntegratedHorizonPlan> CreateIntegratedPlanAsync(
            string planName,
            string description,
            DateTime startDate,
            DateTime endDate,
            PlanningHorizon primaryHorizon,
            string visionStatement);

        Task<List<TimelineEventMarker>> GenerateBackwardPlanAsync(Guid planId);
        
        Task AlignPlanWithStrategicGoalsAsync(Guid planId, List<Guid> goalIds);
    }
}
