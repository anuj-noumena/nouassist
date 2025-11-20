namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Exceptions
{
    /// <summary>
    /// Exception thrown when an experiment is not found
    /// </summary>
    public class ExperimentNotFoundException : GrowthExperimentationDomainException
    {
        public Guid ExperimentId { get; }

        public ExperimentNotFoundException(Guid experimentId)
            : base("GrowthExperimentation:ErrorMessage:ExperimentNotFound")
        {
            ExperimentId = experimentId;
            WithData("experimentId", experimentId);
        }
    }
}
