using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.DecisionSupport.ValueObjects
{
    /// <summary>
    /// Represents the impact level of a decision or scenario
    /// </summary>
    public class Impact : ValueObject
    {
        /// <summary>
        /// The numeric value of the impact (1-5)
        /// </summary>
        public int Value { get; private set; }
        
        /// <summary>
        /// The descriptive text of the impact
        /// </summary>
        public string Description { get; private set; }

        private Impact()
        {
        }

        /// <summary>
        /// Creates a new instance of Impact
        /// </summary>
        /// <param name="value">The impact value (1-5)</param>
        /// <param name="description">Optional description of the impact</param>
        public Impact(int value, string description = null)
        {
            SetValue(value);
            Description = description;
        }

        private void SetValue(int value)
        {
            Check.Range(value, nameof(value), 1, 5);
            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return Description;
        }
    }
}
