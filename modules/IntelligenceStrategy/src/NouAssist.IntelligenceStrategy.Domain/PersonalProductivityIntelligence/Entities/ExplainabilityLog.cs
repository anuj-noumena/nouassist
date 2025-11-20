using System;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities
{
    /// <summary>
    /// Represents a log entry explaining an AI decision
    /// </summary>
    public class ExplainabilityLog : Entity
    {
        /// <summary>
        /// Name of the factor being explained
        /// </summary>
        public string FactorName { get; protected set; }
        
        /// <summary>
        /// Human-readable explanation
        /// </summary>
        public string Explanation { get; protected set; }
        
        /// <summary>
        /// Impact of this factor on the decision (-1 to 1)
        /// </summary>
        public double Impact { get; protected set; }
        
        /// <summary>
        /// Type of evidence (e.g., "statistical", "rule-based", "historical")
        /// </summary>
        public string EvidenceType { get; protected set; }
        
        /// <summary>
        /// Additional evidence data in JSON or structured format
        /// </summary>
        public string EvidenceData { get; protected set; }

        protected ExplainabilityLog()
        {
        }

        /// <summary>
        /// Creates a new explainability log
        /// </summary>
        public ExplainabilityLog(
            string factorName,
            string explanation,
            double impact,
            string evidenceType,
            string evidenceData)
        {
            FactorName = factorName;
            Explanation = explanation;
            Impact = Math.Clamp(impact, -1, 1);
            EvidenceType = evidenceType;
            EvidenceData = evidenceData;
        }

        /// <summary>
        /// Gets the entity keys
        /// </summary>
        public override object[] GetKeys()
        {
            return new object[] { FactorName };
        }
    }
}
