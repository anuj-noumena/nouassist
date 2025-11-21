using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates;
using NouAssist.IntelligenceStrategy.DecisionSupport.Enums;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Interfaces.Services
{
    /// <summary>
    /// Interface for scenario analysis management
    /// </summary>
    public interface IScenarioAnalysisManager
    {
        /// <summary>
        /// Creates a new scenario analysis
        /// </summary>
        Task<ScenarioAnalysis> CreateScenarioAnalysisAsync(
            string title,
            string purpose,
            string timeHorizon,
            string scope,
            string keyDrivers,
            Guid? decisionModelId = null);
            
        /// <summary>
        /// Adds a scenario model to an analysis
        /// </summary>
        Task<ScenarioAnalysis> AddScenarioModelAsync(
            Guid analysisId,
            string name,
            string title,
            string description,
            string assumptions,
            ScenarioLikelihood likelihood,
            decimal confidenceLevel,
            string likelihoodRationale);
            
        /// <summary>
        /// Adds an outcome to a scenario model
        /// </summary>
        Task<ScenarioAnalysis> AddScenarioOutcomeAsync(
            Guid analysisId,
            Guid scenarioModelId,
            string area,
            string description,
            int impactValue,
            string impactDescription = null);
    }
}
