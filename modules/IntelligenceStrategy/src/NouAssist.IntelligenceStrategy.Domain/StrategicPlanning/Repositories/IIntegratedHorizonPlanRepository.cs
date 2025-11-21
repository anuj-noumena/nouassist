using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Repositories
{
    /// <summary>
    /// Repository interface for Integrated Horizon Plan aggregate
    /// </summary>
    public interface IIntegratedHorizonPlanRepository : IRepository<IntegratedHorizonPlan, Guid>
    {
        Task<List<IntegratedHorizonPlan>> GetActivePlansAsync(
            CancellationToken cancellationToken = default);

        Task<IntegratedHorizonPlan> GetPlanWithTimelineEventsAsync(
            Guid planId,
            CancellationToken cancellationToken = default);

        Task<List<IntegratedHorizonPlan>> GetPlansByHorizonAsync(
            PlanningHorizon horizon,
            CancellationToken cancellationToken = default);
    }
}
