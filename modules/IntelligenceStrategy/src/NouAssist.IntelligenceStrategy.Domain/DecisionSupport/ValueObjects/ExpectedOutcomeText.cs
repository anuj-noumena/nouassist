using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.DecisionSupport.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.ValueObjects
{
    /// <summary>
    /// Value object representing the expected outcomes of a strategic initiative
    /// </summary>
    public class ExpectedOutcomeText : ValueObject
    {
        /// <summary>
        /// The text content of the expected outcomes
        /// </summary>
        public string Value { get; private set; }

        private ExpectedOutcomeText() { }

        /// <summary>
        /// Creates a new instance of <see cref="ExpectedOutcomeText"/>
        /// </summary>
        /// <param name="value">The expected outcomes text</param>
        public ExpectedOutcomeText(string value)
        {
            SetValue(value);
        }

        private void SetValue(string value)
        {
            Value = Check.NotNullOrWhiteSpace(
                value,
                nameof(value),
                maxLength: DecisionSupportConsts.MaxExpectedOutcomesLength
            );
        }

        /// <inheritdoc/>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
        
        /// <summary>
        /// Implicitly converts from string to <see cref="ExpectedOutcomeText"/>
        /// </summary>
        public static implicit operator string(ExpectedOutcomeText outcomeText) => outcomeText.Value;
        
        /// <summary>
        /// Returns the string representation of this expected outcome text
        /// </summary>
        public override string ToString() => Value;
    }
}
