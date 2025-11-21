using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.IntelligenceStrategy.DecisionSupport.Entities;
using NouAssist.IntelligenceStrategy.DecisionSupport.Enums;
using NouAssist.IntelligenceStrategy.DecisionSupport.ValueObjects;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates
{
    /// <summary>
    /// Represents a scenario analysis that explores different possible futures
    /// </summary>
    public class ScenarioAnalysis : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// The tenant ID associated with this scenario analysis
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// The title of the scenario analysis
        /// </summary>
        public string Title { get; private set; }
        
        /// <summary>
        /// The purpose or focus question of the analysis
        /// </summary>
        public string Purpose { get; private set; }
        
        /// <summary>
        /// The time horizon for the scenario analysis
        /// </summary>
        public string TimeHorizon { get; private set; }
        
        /// <summary>
        /// The scope of the analysis
        /// </summary>
        public string Scope { get; private set; }
        
        /// <summary>
        /// Key drivers or factors influencing the scenarios
        /// </summary>
        public string KeyDrivers { get; private set; }
        
        /// <summary>
        /// ID of the decision model this analysis is linked to, if any
        /// </summary>
        public Guid? DecisionModelId { get; private set; }
        
        /// <summary>
        /// The scenario models being analyzed
        /// </summary>
        private readonly List<ScenarioModel> _scenarioModels;
        public IReadOnlyCollection<ScenarioModel> ScenarioModels => _scenarioModels.AsReadOnly();

        protected ScenarioAnalysis()
        {
            _scenarioModels = new List<ScenarioModel>();
        }

        /// <summary>
        /// Creates a new scenario analysis
        /// </summary>
        public ScenarioAnalysis(
            Guid id,
            string title,
            string purpose,
            string timeHorizon,
            string scope,
            string keyDrivers,
            Guid? decisionModelId = null,
            Guid? tenantId = null)
            : base(id)
        {
            TenantId = tenantId;
            Title = title;
            Purpose = purpose;
            TimeHorizon = timeHorizon;
            Scope = scope;
            KeyDrivers = keyDrivers;
            DecisionModelId = decisionModelId;
            _scenarioModels = new List<ScenarioModel>();
        }
        
        /// <summary>
        /// Factory method to create a new ScenarioAnalysis
        /// </summary>
        public static ScenarioAnalysis Create(
            Guid id,
            string title,
            string purpose,
            string timeHorizon,
            string scope,
            string keyDrivers,
            Guid? decisionModelId = null,
            Guid? tenantId = null)
        {
            return new ScenarioAnalysis(
                id,
                title,
                purpose,
                timeHorizon,
                scope,
                keyDrivers,
                decisionModelId,
                tenantId);
        }
        
        /// <summary>
        /// Updates the scenario analysis properties
        /// </summary>
        public void Update(
            string title,
            string purpose,
            string timeHorizon,
            string scope,
            string keyDrivers)
        {
            Title = title;
            Purpose = purpose;
            TimeHorizon = timeHorizon;
            Scope = scope;
            KeyDrivers = keyDrivers;
        }
        
        /// <summary>
        /// Adds a scenario model to the analysis
        /// </summary>
        public ScenarioModel AddScenarioModel(
            string name,
            ScenarioDescription description,
            ScenarioLikelihood likelihood,
            decimal confidenceLevel,
            string likelihoodRationale)
        {
            if (_scenarioModels.Any(s => s.Name == name))
            {
                throw new ArgumentException($"A scenario named '{name}' already exists in this analysis");
            }
            
            var scenarioModel = new ScenarioModel(
                Id,
                name,
                description,
                new OutcomeLikelihood(likelihood, confidenceLevel, likelihoodRationale));
                
            _scenarioModels.Add(scenarioModel);
            
            return scenarioModel;
        }
        
        /// <summary>
        /// Updates an existing scenario model
        /// </summary>
        public ScenarioModel UpdateScenarioModel(
            Guid scenarioModelId,
            string name,
            ScenarioDescription description,
            ScenarioLikelihood likelihood,
            decimal confidenceLevel,
            string likelihoodRationale)
        {
            var scenarioModel = _scenarioModels.FirstOrDefault(s => s.Id == scenarioModelId);
            if (scenarioModel == null)
            {
                throw new ArgumentException($"No scenario with ID {scenarioModelId} exists in this analysis");
            }
            
            // Check if name is being changed and would cause a duplicate
            if (name != scenarioModel.Name && _scenarioModels.Any(s => s.Name == name))
            {
                throw new ArgumentException($"A scenario named '{name}' already exists in this analysis");
            }
            
            scenarioModel.Update(
                name,
                description,
                new OutcomeLikelihood(likelihood, confidenceLevel, likelihoodRationale));
                
            return scenarioModel;
        }
        
        /// <summary>
        /// Removes a scenario model from the analysis
        /// </summary>
        public void RemoveScenarioModel(Guid scenarioModelId)
        {
            var scenarioModel = _scenarioModels.FirstOrDefault(s => s.Id == scenarioModelId);
            if (scenarioModel == null)
            {
                throw new ArgumentException($"No scenario with ID {scenarioModelId} exists in this analysis");
            }
            
            _scenarioModels.Remove(scenarioModel);
        }
        
        /// <summary>
        /// Adds a scenario outcome to a specific scenario model
        /// </summary>
        public void AddScenarioOutcome(
            Guid scenarioModelId,
            string area,
            string description,
            Impact impact)
        {
            var scenarioModel = _scenarioModels.FirstOrDefault(s => s.Id == scenarioModelId);
            if (scenarioModel == null)
            {
                throw new ArgumentException($"No scenario with ID {scenarioModelId} exists in this analysis");
            }
            
            scenarioModel.AddOutcome(area, description, impact);
        }
        
        /// <summary>
        /// Gets the most likely scenario based on likelihood assessments
        /// </summary>
        public ScenarioModel GetMostLikelyScenario()
        {
            if (!_scenarioModels.Any())
            {
                return null;
            }
            
            return _scenarioModels
                .OrderByDescending(s => (int)s.Likelihood.Likelihood)
                .ThenByDescending(s => s.Likelihood.Confidence)
                .First();
        }
        
        /// <summary>
        /// Gets the highest impact scenario, regardless of likelihood
        /// </summary>
        public ScenarioModel GetHighestImpactScenario()
        {
            if (!_scenarioModels.Any())
            {
                return null;
            }
            
            return _scenarioModels
                .OrderByDescending(s => s.Outcomes.Average(o => o.Impact.Value))
                .First();
        }
    }
}
