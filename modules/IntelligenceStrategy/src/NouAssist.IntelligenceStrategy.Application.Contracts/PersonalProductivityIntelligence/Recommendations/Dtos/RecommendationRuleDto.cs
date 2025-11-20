using System;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Recommendations.Dtos
{
    /// <summary>
    /// DTO for recommendation rules
    /// </summary>
    public class RecommendationRuleDto
    {
        /// <summary>
        /// Name of the rule
        /// </summary>
        public string RuleName { get; set; }
        
        /// <summary>
        /// Description of the rule
        /// </summary>
        public string RuleDescription { get; set; }
        
        /// <summary>
        /// Contribution weight (0-1)
        /// </summary>
        public double ContributionWeight { get; set; }
    }
}
