using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.ValueObjects
{
    /// <summary>
    /// Represents a weight value for criteria in decision models
    /// </summary>
    public class Weight : ValueObject
    {
        /// <summary>
        /// The numeric value of the weight
        /// </summary>
        public decimal Value { get; private set; }

        private Weight()
        {
        }

        /// <summary>
        /// Creates a new instance of Weight
        /// </summary>
        /// <param name="value">The weight value (must be between 0 and 1)</param>
        public Weight(decimal value)
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
