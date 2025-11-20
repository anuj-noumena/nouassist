namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Events
{
    /// <summary>
    /// Event published when a new experiment is created
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.GrowthExperimentation.ExperimentCreated")]
    public class ExperimentCreatedEto : ExperimentEtoBase
    {
        /// <summary>
        /// The description of the experiment
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// The initial status of the experiment
        /// </summary>
        public ExperimentStatus Status { get; set; }
        
        /// <summary>
        /// When the experiment was created
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
