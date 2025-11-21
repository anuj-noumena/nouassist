using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects
{
    /// <summary>
    /// Value object that represents the goal of an experiment
    /// </summary>
    public class ExperimentGoal : ValueObject
    {
        /// <summary>
        /// Brief description of the goal
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Expected improvement percentage
        /// </summary>
        public double ExpectedImprovement { get; private set; }
        
        /// <summary>
        /// Minimum viable improvement percentage
        /// </summary>
        public double MinimumViableImprovement { get; private set; }
        
        protected ExperimentGoal() { }
        
        public ExperimentGoal(string description, double expectedImprovement, double minimumViableImprovement)
        {
            SetDescription(description);
            SetExpectedImprovement(expectedImprovement);
            SetMinimumViableImprovement(minimumViableImprovement);
        }
        
        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description, 
                nameof(description), 
                maxLength: GrowthExperimentationConstants.Experiments.MaxDescriptionLength
            );
        }
        
        private void SetExpectedImprovement(double expectedImprovement)
        {
            if (expectedImprovement <= 0)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidValue")
                    .WithData("value", expectedImprovement)
                    .WithData("property", "ExpectedImprovement");
            }
            
            ExpectedImprovement = expectedImprovement;
        }
        
        private void SetMinimumViableImprovement(double minimumViableImprovement)
        {
            if (minimumViableImprovement <= 0)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidValue")
                    .WithData("value", minimumViableImprovement)
                    .WithData("property", "MinimumViableImprovement");
            }
            
            MinimumViableImprovement = minimumViableImprovement;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Description;
            yield return ExpectedImprovement;
            yield return MinimumViableImprovement;
        }
    }
}
