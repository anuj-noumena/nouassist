using System;
using System.Threading.Tasks;
using NouAssist.DecisionSupport.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.DecisionSupport.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for decision models
    /// </summary>
    public interface IDecisionModelRepository : IRepository<DecisionModelInstance, Guid>
    {
        /// <summary>
        /// Gets a decision model with all related entities (criteria, alternatives, analyses, etc.)
        /// </summary>
        Task<DecisionModelInstance> GetWithDetailsAsync(Guid id);
        
        /// <summary>
        /// Checks if a decision model with the same name already exists
        /// </summary>
        Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null);
    }
}
