using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.ValueObjects
{
    /// <summary>
    /// Represents a probability value for risk assessment
    /// </summary>
    public class Probability : ValueObject
    {
        /// <summary>
        /// The numeric value of the probability
        /// </summary>
        public decimal Value { get; private set; }

        private Probability()
        {
        }

        /// <summary>
        /// Creates a new instance of Probability
        /// </summary>
        /// <param name="value">The probability value (must be between 0 and 1)</param>
        public Probability(decimal value)
        {
            SetValue(value);
        }

        private void SetValue(decimal value)
        {
            Check.Range(value, nameof(value), 0, 1);
            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
