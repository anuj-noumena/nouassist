using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.DecisionSupport.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.ValueObjects
{
    /// <summary>
    /// Value object representing the rationale for a decision or proposal
    /// </summary>
    public class DecisionRationaleText : ValueObject
    {
        /// <summary>
        /// The text content of the rationale
        /// </summary>
        public string Value { get; private set; }

        private DecisionRationaleText() { }

        /// <summary>
        /// Creates a new instance of <see cref="DecisionRationaleText"/>
        /// </summary>
        /// <param name="value">The rationale text</param>
        public DecisionRationaleText(string value)
        {
            SetValue(value);
        }

        private void SetValue(string value)
        {
            Value = Check.NotNullOrWhiteSpace(
                value,
                nameof(value),
                maxLength: DecisionSupportConsts.MaxRationaleLength
            );
        }

        /// <inheritdoc/>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
        
        /// <summary>
        /// Implicitly converts from string to <see cref="DecisionRationaleText"/>
        /// </summary>
        public static implicit operator string(DecisionRationaleText rationaleText) => rationaleText.Value;
        
        /// <summary>
        /// Returns the string representation of this rationale text
        /// </summary>
        public override string ToString() => Value;
    }
}
