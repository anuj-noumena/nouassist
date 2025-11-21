using System.Collections.Generic;
using NouAssist.DecisionSupport.Constants;
using NouAssist.IntelligenceStrategy.DecisionSupport.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.DecisionSupport.ValueObjects
{
    /// <summary>
    /// Represents a description of a scenario in scenario analysis
    /// </summary>
    public class ScenarioDescription : ValueObject
    {
        /// <summary>
        /// The title of the scenario
        /// </summary>
        public string Title { get; private set; }
        
        /// <summary>
        /// The detailed description of the scenario
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// The key assumptions of the scenario
        /// </summary>
        public string Assumptions { get; private set; }

        private ScenarioDescription()
        {
        }

        /// <summary>
        /// Creates a new instance of ScenarioDescription
        /// </summary>
        /// <param name="title">The title of the scenario</param>
        /// <param name="description">The detailed description of the scenario</param>
        /// <param name="assumptions">The key assumptions of the scenario</param>
        public ScenarioDescription(string title, string description, string assumptions)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 100);
            
            Description = Check.NotNullOrWhiteSpace(
                description, 
                nameof(description),
                maxLength: DecisionSupportConsts.MaxScenarioDescriptionLength);
                
            Assumptions = Check.NotNullOrWhiteSpace(assumptions, nameof(assumptions));
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Title;
            yield return Description;
            yield return Assumptions;
        }
    }
}
