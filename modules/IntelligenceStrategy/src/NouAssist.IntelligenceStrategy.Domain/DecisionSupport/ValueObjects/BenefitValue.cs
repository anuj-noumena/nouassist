using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.DecisionSupport.ValueObjects
{
    /// <summary>
    /// Represents a benefit value for cost-benefit analysis
    /// </summary>
    public class BenefitValue : ValueObject
    {
        /// <summary>
        /// The monetary value of the benefit
        /// </summary>
        public decimal? MonetaryValue { get; private set; }
        
        /// <summary>
        /// The description of the benefit
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Indicates if this is a one-time or recurring benefit
        /// </summary>
        public bool IsRecurring { get; private set; }
        
        /// <summary>
        /// The qualitative value of the benefit (1-10)
        /// </summary>
        public int QualitativeValue { get; private set; }

        private BenefitValue()
        {
        }

        /// <summary>
        /// Creates a new instance of BenefitValue
        /// </summary>
        /// <param name="description">The description of the benefit</param>
        /// <param name="qualitativeValue">The qualitative value (1-10)</param>
        /// <param name="monetaryValue">The optional monetary value</param>
        /// <param name="isRecurring">Indicates if this is a recurring benefit</param>
        public BenefitValue(
            string description,
            int qualitativeValue,
            decimal? monetaryValue = null,
            bool isRecurring = false)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description));
            QualitativeValue = Check.Range(qualitativeValue, nameof(qualitativeValue), 1, 10);
            MonetaryValue = monetaryValue;
            IsRecurring = isRecurring;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return MonetaryValue;
            yield return Description;
            yield return IsRecurring;
            yield return QualitativeValue;
        }
    }
}
