using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.DecisionSupport.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.ValueObjects
{
    /// <summary>
    /// Represents the likelihood of a scenario outcome
    /// </summary>
    public class OutcomeLikelihood : ValueObject
    {
        /// <summary>
        /// The likelihood category
        /// </summary>
        public ScenarioLikelihood Likelihood { get; private set; }
        
        /// <summary>
        /// The confidence level in the assessment (0-1)
        /// </summary>
        public decimal Confidence { get; private set; }
        
        /// <summary>
        /// The rationale for the likelihood assessment
        /// </summary>
        public string Rationale { get; private set; }

        private OutcomeLikelihood()
        {
        }

        /// <summary>
        /// Creates a new instance of OutcomeLikelihood
        /// </summary>
        /// <param name="likelihood">The likelihood category</param>
        /// <param name="confidence">The confidence level (0-1)</param>
        /// <param name="rationale">The rationale for the assessment</param>
        public OutcomeLikelihood(
            ScenarioLikelihood likelihood,
            decimal confidence,
            string rationale)
        {
            Likelihood = likelihood;
            Confidence = confidence;
            Rationale = rationale;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Likelihood;
            yield return Confidence;
            yield return Rationale;
        }
    }
}
