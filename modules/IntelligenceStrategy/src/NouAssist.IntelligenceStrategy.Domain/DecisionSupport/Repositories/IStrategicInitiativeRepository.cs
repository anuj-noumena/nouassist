using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Repositories
{
    /// <summary>
    /// Repository for the Strategic Initiative aggregate
    /// </summary>
    public interface IStrategicInitiativeRepository : IRepository<StrategicInitiativeProposal, Guid>
    {
        /// <summary>
        /// Gets initiatives matching the given specification
        /// </summary>
        Task<List<StrategicInitiativeProposal>> GetListAsync(
            Specification<StrategicInitiativeProposal> specification,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets initiatives with their supporting analyses
        /// </summary>
        Task<List<StrategicInitiativeProposal>> GetListWithAnalysesAsync(
            Expression<Func<StrategicInitiativeProposal, bool>> predicate,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets a single initiative with its supporting analyses
        /// </summary>
        Task<StrategicInitiativeProposal> GetWithAnalysesAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
