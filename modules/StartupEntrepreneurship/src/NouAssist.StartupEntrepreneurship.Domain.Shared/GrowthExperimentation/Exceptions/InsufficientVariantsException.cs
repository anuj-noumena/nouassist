using System;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Exceptions
{
    /// <summary>
    /// Exception thrown when an experiment has insufficient variants
    /// </summary>
    public class InsufficientVariantsException : GrowthExperimentationDomainException
    {
        public Guid ExperimentId { get; }
        public int CurrentCount { get; }
        public int RequiredCount { get; }

        public InsufficientVariantsException(Guid experimentId, int currentCount, int requiredCount)
            : base("GrowthExperimentation:ErrorMessage:InsufficientVariants")
        {
            ExperimentId = experimentId;
            CurrentCount = currentCount;
            RequiredCount = requiredCount;
            
            WithData("experimentId", experimentId);
            WithData("currentCount", currentCount);
            WithData("requiredCount", requiredCount);
        }
    }
}
