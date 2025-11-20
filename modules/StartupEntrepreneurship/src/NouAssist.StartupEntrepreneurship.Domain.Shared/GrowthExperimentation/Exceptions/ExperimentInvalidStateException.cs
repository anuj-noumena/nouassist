namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Exceptions
{
    /// <summary>
    /// Exception thrown when an operation is attempted on an experiment in an invalid state
    /// </summary>
    public class ExperimentInvalidStateException : GrowthExperimentationDomainException
    {
        public Guid ExperimentId { get; }
        public ExperimentStatus CurrentStatus { get; }
        public ExperimentStatus[] ExpectedStatuses { get; }

        public ExperimentInvalidStateException(Guid experimentId, ExperimentStatus currentStatus, params ExperimentStatus[] expectedStatuses)
            : base("GrowthExperimentation:ErrorMessage:InvalidExperimentState")
        {
            ExperimentId = experimentId;
            CurrentStatus = currentStatus;
            ExpectedStatuses = expectedStatuses;
            
            WithData("experimentId", experimentId);
            WithData("currentStatus", currentStatus);
            WithData("expectedStatuses", expectedStatuses);
        }
    }
}
