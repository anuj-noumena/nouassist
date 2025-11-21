using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Repositories
{
    /// <summary>
    /// Repository interface for Strategic Goal Definition aggregate
    /// </summary>
    public interface IStrategicGoalRepository : IRepository<StrategicGoalDefinition, Guid>
    {
        Task<List<StrategicGoalDefinition>> GetListAsync(
            Expression<Func<StrategicGoalDefinition, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<int> CountAsync(
            Expression<Func<StrategicGoalDefinition, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(
            Expression<Func<StrategicGoalDefinition, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<StrategicGoalDefinition> FindAsync(
            Expression<Func<StrategicGoalDefinition, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<List<StrategicGoalDefinition>> GetGoalsByOwnerAsync(
            Guid ownerId,
            CancellationToken cancellationToken = default);

        Task<List<StrategicGoalDefinition>> GetGoalsByHorizonAsync(
            PlanningHorizon horizon,
            CancellationToken cancellationToken = default);
    }
}
