using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects
{
    /// <summary>
    /// Value object that represents learnings from an experiment
    /// </summary>
    public class ExperimentLearnings : ValueObject
    {
        /// <summary>
        /// Key insights from the experiment
        /// </summary>
        public string Insights { get; private set; }
        
        /// <summary>
        /// Unexpected outcomes or observations
        /// </summary>
        public string UnexpectedOutcomes { get; private set; }
        
        /// <summary>
        /// Recommendations for future experiments
        /// </summary>
        public string Recommendations { get; private set; }
        
        /// <summary>
        /// Who documented these learnings
        /// </summary>
        public Guid DocumentedBy { get; private set; }
        
        /// <summary>
        /// When these learnings were documented
        /// </summary>
        public DateTime DocumentedAt { get; private set; }
        
        protected ExperimentLearnings() { }
        
        public ExperimentLearnings(
            string insights, 
            string unexpectedOutcomes, 
            string recommendations, 
            Guid documentedBy,
            DateTime documentedAt)
        {
            SetInsights(insights);
            UnexpectedOutcomes = unexpectedOutcomes;
            Recommendations = recommendations;
            DocumentedBy = documentedBy;
            DocumentedAt = documentedAt;
        }
        
        private void SetInsights(string insights)
        {
            Insights = Check.NotNullOrWhiteSpace(
                insights, 
                nameof(insights), 
                maxLength: GrowthExperimentationConstants.Experiments.MaxLearningsTextLength
            );
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Insights;
            
            if (!string.IsNullOrEmpty(UnexpectedOutcomes))
            {
                yield return UnexpectedOutcomes;
            }
            
            if (!string.IsNullOrEmpty(Recommendations))
            {
                yield return Recommendations;
            }
            
            yield return DocumentedBy;
            yield return DocumentedAt;
        }
    }
}
