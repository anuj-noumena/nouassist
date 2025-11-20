using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.DecisionSupport.Aggregates;
using NouAssist.DecisionSupport.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.DecisionSupport.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for strategic initiatives
    /// </summary>
    public interface IStrategicInitiativeRepository : IRepository<StrategicInitiativeProposal, Guid>
    {
        /// <summary>
        /// Gets strategic initiatives by status
        /// </summary>
        Task<List<StrategicInitiativeProposal>> GetByStatusAsync(ProposalStatus status);
        
        /// <summary>
        /// Gets strategic initiatives linked to a specific project
        /// </summary>
        Task<StrategicInitiativeProposal> GetByProjectIdAsync(Guid projectId);
        
        /// <summary>
        /// Gets strategic initiatives linked to a specific decision model
        /// </summary>
        Task<List<StrategicInitiativeProposal>> GetByDecisionModelIdAsync(Guid decisionModelId);
    }
}
