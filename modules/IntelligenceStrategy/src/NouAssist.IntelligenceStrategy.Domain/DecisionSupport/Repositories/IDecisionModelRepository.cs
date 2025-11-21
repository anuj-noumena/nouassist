using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Repositories
{
    /// <summary>
    /// Repository for the Decision Model aggregate
    /// </summary>
    public interface IDecisionModelRepository : IRepository<DecisionModelInstance, Guid>
    {
        /// <summary>
        /// Gets a decision model with its criteria and alternatives
        /// </summary>
        Task<DecisionModelInstance> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets decision models with a specific status
        /// </summary>
        Task<List<DecisionModelInstance>> GetByFinalizedStatusAsync(
            bool isFinalized,
            CancellationToken cancellationToken = default);
    }
}
