using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.DecisionSupport.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.DecisionSupport.Entities
{
    /// <summary>
    /// Represents a specific scenario model within a scenario analysis
    /// </summary>
    public class ScenarioModel : Entity<Guid>
    {
        /// <summary>
        /// The ID of the scenario analysis this model belongs to
        /// </summary>
        public Guid ScenarioAnalysisId { get; private set; }
        
        /// <summary>
        /// The name of the scenario
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// The description of the scenario
        /// </summary>
        public ScenarioDescription Description { get; private set; }
        
        /// <summary>
        /// The likelihood assessment of this scenario
        /// </summary>
        public OutcomeLikelihood Likelihood { get; private set; }
        
        /// <summary>
        /// The potential outcomes of this scenario
        /// </summary>
        private readonly List<ScenarioOutcome> _outcomes;
        public IReadOnlyCollection<ScenarioOutcome> Outcomes => _outcomes.AsReadOnly();

        protected ScenarioModel()
        {
            _outcomes = new List<ScenarioOutcome>();
        }

        /// <summary>
        /// Creates a new scenario model
        /// </summary>
        public ScenarioModel(
            Guid scenarioAnalysisId,
            string name,
            ScenarioDescription description,
            OutcomeLikelihood likelihood)
            : base(Guid.NewGuid())
        {
            ScenarioAnalysisId = scenarioAnalysisId;
            Name = name;
            Description = description;
            Likelihood = likelihood;
            _outcomes = new List<ScenarioOutcome>();
        }
        
        /// <summary>
        /// Updates the scenario model properties
        /// </summary>
        public void Update(
            string name,
            ScenarioDescription description,
            OutcomeLikelihood likelihood)
        {
            Name = name;
            Description = description;
            Likelihood = likelihood;
        }
        
        /// <summary>
        /// Adds an outcome to the scenario
        /// </summary>
        public ScenarioOutcome AddOutcome(string area, string description, Impact impact)
        {
            if (_outcomes.Any(o => o.Area == area))
            {
                throw new ArgumentException($"An outcome for area '{area}' already exists in this scenario");
            }
            
            var outcome = new ScenarioOutcome(Id, area, description, impact);
            _outcomes.Add(outcome);
            
            return outcome;
        }
        
        /// <summary>
        /// Removes an outcome from the scenario
        /// </summary>
        public void RemoveOutcome(string area)
        {
            var outcome = _outcomes.FirstOrDefault(o => o.Area == area);
            if (outcome == null)
            {
                throw new ArgumentException($"No outcome for area '{area}' exists in this scenario");
            }
            
            _outcomes.Remove(outcome);
        }
        
        /// <summary>
        /// Gets the average impact score of all outcomes
        /// </summary>
        public decimal GetAverageImpact()
        {
            if (!_outcomes.Any())
            {
                return 0;
            }
            
            return _outcomes.Average(o => o.Impact.Value);
        }
    }
}
