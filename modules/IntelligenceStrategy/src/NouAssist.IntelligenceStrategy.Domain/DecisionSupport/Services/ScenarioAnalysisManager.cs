using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.DecisionSupport.Aggregates;
using NouAssist.DecisionSupport.Enums;
using NouAssist.DecisionSupport.Interfaces.Repositories;
using NouAssist.DecisionSupport.Interfaces.Services;
using NouAssist.DecisionSupport.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.DecisionSupport.Services
{
    /// <summary>
    /// Domain service for managing scenario analyses
    /// </summary>
    public class ScenarioAnalysisManager : DomainService, IScenarioAnalysisManager
    {
        private readonly IScenarioAnalysisRepository _scenarioRepository;
        private readonly ILogger<ScenarioAnalysisManager> _logger;

        public ScenarioAnalysisManager(
            IScenarioAnalysisRepository scenarioRepository,
            ILogger<ScenarioAnalysisManager> logger)
        {
            _scenarioRepository = scenarioRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new scenario analysis
        /// </summary>
        public async Task<ScenarioAnalysis> CreateScenarioAnalysisAsync(
            string title,
            string purpose,
            string timeHorizon,
            string scope,
            string keyDrivers,
            Guid? decisionModelId = null)
        {
            try
            {
                var analysis = ScenarioAnalysis.Create(
                    GuidGenerator.Create(),
                    title,
                    purpose,
                    timeHorizon,
                    scope,
                    keyDrivers,
                    decisionModelId,
                    CurrentTenant.Id);
                    
                await _scenarioRepository.InsertAsync(analysis);
                
                _logger.LogInformation("Created scenario analysis {Title} with ID {AnalysisId}", 
                    title, analysis.Id);
                    
                return analysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create scenario analysis {Title}", title);
                throw;
            }
        }
        
        /// <summary>
        /// Adds a scenario model to an analysis
        /// </summary>
        public async Task<ScenarioAnalysis> AddScenarioModelAsync(
            Guid analysisId,
            string name,
            string title,
            string description,
            string assumptions,
            ScenarioLikelihood likelihood,
            decimal confidenceLevel,
            string likelihoodRationale)
        {
            try
            {
                var analysis = await _scenarioRepository.GetAsync(analysisId);
                
                var scenarioDescription = new ScenarioDescription(title, description, assumptions);
                
                analysis.AddScenarioModel(
                    name,
                    scenarioDescription,
                    likelihood,
                    confidenceLevel,
                    likelihoodRationale);
                    
                await _scenarioRepository.UpdateAsync(analysis);
                
                _logger.LogInformation("Added scenario model {Name} to analysis {AnalysisId}", 
                    name, analysisId);
                    
                return analysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add scenario model to analysis {AnalysisId}", 
                    analysisId);
                throw;
            }
        }
        
        /// <summary>
        /// Adds an outcome to a scenario model
        /// </summary>
        public async Task<ScenarioAnalysis> AddScenarioOutcomeAsync(
            Guid analysisId,
            Guid scenarioModelId,
            string area,
            string description,
            int impactValue,
            string impactDescription = null)
        {
            try
            {
                var analysis = await _scenarioRepository.GetAsync(analysisId);
                
                var impact = new Impact(impactValue, impactDescription);
                
                analysis.AddScenarioOutcome(
                    scenarioModelId,
                    area,
                    description,
                    impact);
                    
                await _scenarioRepository.UpdateAsync(analysis);
                
                _logger.LogInformation("Added outcome to scenario model {ScenarioId} in analysis {AnalysisId}", 
                    scenarioModelId, analysisId);
                    
                return analysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add outcome to scenario model {ScenarioId}", 
                    scenarioModelId);
                throw;
            }
        }
    }
}
