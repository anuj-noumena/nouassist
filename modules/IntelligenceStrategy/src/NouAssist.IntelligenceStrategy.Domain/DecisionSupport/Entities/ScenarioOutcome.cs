using System;
using NouAssist.DecisionSupport.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.DecisionSupport.Entities
{
    /// <summary>
    /// Represents a potential outcome in a scenario model
    /// </summary>
    public class ScenarioOutcome : Entity
    {
        /// <summary>
        /// The ID of the scenario model this outcome belongs to
        /// </summary>
        public Guid ScenarioModelId { get; private set; }
        
        /// <summary>
        /// The area or aspect this outcome affects
        /// </summary>
        public string Area { get; private set; }
        
        /// <summary>
        /// The description of the outcome
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// The impact of this outcome
        /// </summary>
        public Impact Impact { get; private set; }

        protected ScenarioOutcome()
        {
        }

        /// <summary>
        /// Creates a new scenario outcome
        /// </summary>
        public ScenarioOutcome(
            Guid scenarioModelId,
            string area,
            string description,
            Impact impact)
        {
            ScenarioModelId = scenarioModelId;
            Area = area;
            Description = description;
            Impact = impact;
        }
        
        /// <summary>
        /// Updates the outcome properties
        /// </summary>
        public void Update(string description, Impact impact)
        {
            Description = description;
            Impact = impact;
        }
        
        public override object[] GetKeys()
        {
            return new object[] { ScenarioModelId, Area };
        }
    }
}
