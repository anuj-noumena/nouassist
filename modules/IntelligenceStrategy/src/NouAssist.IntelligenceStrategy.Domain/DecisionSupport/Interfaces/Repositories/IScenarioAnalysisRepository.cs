using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for scenario analyses
    /// </summary>
    public interface IScenarioAnalysisRepository : IRepository<ScenarioAnalysis, Guid>
    {
        /// <summary>
        /// Gets a scenario analysis with all models and outcomes
        /// </summary>
        Task<ScenarioAnalysis> GetWithDetailsAsync(Guid id);
        
        /// <summary>
        /// Gets scenario analyses linked to a specific decision model
        /// </summary>
        Task<List<ScenarioAnalysis>> GetByDecisionModelIdAsync(Guid decisionModelId);
    }
}
