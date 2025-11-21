using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.ValueObjects
{
    /// <summary>
    /// Represents a score value for alternatives in decision models
    /// </summary>
    public class Score : ValueObject
    {
        /// <summary>
        /// The numeric value of the score
        /// </summary>
        public decimal Value { get; private set; }

        private Score()
        {
        }

        /// <summary>
        /// Creates a new instance of Score
        /// </summary>
        /// <param name="value">The score value (must be between 0 and 10)</param>
        public Score(decimal value)
        {
            SetValue(value);
        }

        private void SetValue(decimal value)
        {
            Check.Range(value, nameof(value), 0, 10);
            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
