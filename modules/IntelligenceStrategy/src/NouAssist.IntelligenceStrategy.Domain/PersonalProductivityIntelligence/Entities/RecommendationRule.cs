using System;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities
{
    /// <summary>
    /// Represents a rule that contributed to a recommendation
    /// </summary>
    public class RecommendationRule : Entity
    {
        /// <summary>
        /// Name of the rule
        /// </summary>
        public string RuleName { get; protected set; }
        
        /// <summary>
        /// Description of the rule logic
        /// </summary>
        public string RuleDescription { get; protected set; }
        
        /// <summary>
        /// Weight of this rule's contribution (0-1)
        /// </summary>
        public double ContributionWeight { get; protected set; }

        protected RecommendationRule()
        {
        }

        /// <summary>
        /// Creates a new recommendation rule
        /// </summary>
        public RecommendationRule(
            string ruleName,
            string ruleDescription,
            double contributionWeight)
        {
            RuleName = ruleName;
            RuleDescription = ruleDescription;
            ContributionWeight = Math.Clamp(contributionWeight, 0, 1);
        }

        /// <summary>
        /// Gets the entity keys
        /// </summary>
        public override object[] GetKeys()
        {
            return new object[] { RuleName };
        }
    }
}
