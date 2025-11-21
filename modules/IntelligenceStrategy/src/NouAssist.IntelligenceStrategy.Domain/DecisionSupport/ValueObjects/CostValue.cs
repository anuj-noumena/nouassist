using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.ValueObjects
{
    /// <summary>
    /// Represents a cost value for cost-benefit analysis
    /// </summary>
    public class CostValue : ValueObject
    {
        /// <summary>
        /// The monetary value of the cost
        /// </summary>
        public decimal Value { get; private set; }
        
        /// <summary>
        /// The description of the cost
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Indicates if this is a one-time or recurring cost
        /// </summary>
        public bool IsRecurring { get; private set; }

        private CostValue()
        {
        }

        /// <summary>
        /// Creates a new instance of CostValue
        /// </summary>
        /// <param name="value">The monetary value of the cost</param>
        /// <param name="description">The description of the cost</param>
        /// <param name="isRecurring">Indicates if this is a recurring cost</param>
        public CostValue(decimal value, string description, bool isRecurring = false)
        {
            Value = Check.NotNull(value, nameof(value));
            Description = Check.NotNullOrWhiteSpace(description, nameof(description));
            IsRecurring = isRecurring;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return Description;
            yield return IsRecurring;
        }
    }
}
