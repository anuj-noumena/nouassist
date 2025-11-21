using System;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities
{
    /// <summary>
    /// Entity that represents a hypothesis for an experiment
    /// </summary>
    public class ExperimentHypothesis : Entity
    {
        /// <summary>
        /// The experiment ID this hypothesis belongs to
        /// </summary>
        public Guid ExperimentId { get; private set; }
        
        /// <summary>
        /// Hypothesis statement
        /// </summary>
        public string Statement { get; private set; }
        
        /// <summary>
        /// Rationale or explanation for the hypothesis
        /// </summary>
        public string Rationale { get; private set; }
        
        /// <summary>
        /// Whether this hypothesis was supported by experiment results
        /// </summary>
        public bool? IsSupported { get; private set; }
        
        protected ExperimentHypothesis() { }
        
        internal ExperimentHypothesis(Guid experimentId, string statement, string rationale)
        {
            ExperimentId = experimentId;
            SetStatement(statement);
            Rationale = rationale;
            IsSupported = null;
        }
        
        private void SetStatement(string statement)
        {
            Statement = Check.NotNullOrWhiteSpace(
                statement, 
                nameof(statement), 
                maxLength: GrowthExperimentationConstants.Experiments.MaxHypothesisTextLength
            );
        }
        
        internal void SetIsSupported(bool isSupported)
        {
            IsSupported = isSupported;
        }
        
        public override object[] GetKeys()
        {
            return new object[] { ExperimentId, Statement };
        }
    }
}
