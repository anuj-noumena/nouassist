using System;
using Volo.Abp.Domain.Values;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Exceptions;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.ValueObjects
{
    /// <summary>
    /// Value object representing a risk probability
    /// </summary>
    public class RiskProbability : ValueObject
    {
        /// <summary>
        /// Value of the risk probability (between 0 and 1)
        /// </summary>
        public double Value { get; private set; }

        protected RiskProbability()
        {
        }

        /// <summary>
        /// Creates a new risk probability
        /// </summary>
        /// <param name="value">Risk value between 0 and 1</param>
        public RiskProbability(double value)
        {
            if (value < 0 || value > 1)
            {
                throw new PersonalProductivityIntelligenceDomainException(
                    "PersonalProductivityIntelligence:Validation:InvalidRiskProbability");
            }

            Value = value;
        }

        /// <summary>
        /// Gets the risk level as a readable category
        /// </summary>
        /// <returns>Risk level category</returns>
        public string GetRiskLevel()
        {
            if (Value < 0.2) return "Very Low";
            if (Value < 0.4) return "Low";
            if (Value < 0.6) return "Medium";
            if (Value < 0.8) return "High";
            return "Very High";
        }

        /// <summary>
        /// Formats the risk probability as a percentage
        /// </summary>
        /// <returns>Formatted percentage</returns>
        public string FormatAsPercentage()
        {
            return $"{Value * 100:0}%";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
