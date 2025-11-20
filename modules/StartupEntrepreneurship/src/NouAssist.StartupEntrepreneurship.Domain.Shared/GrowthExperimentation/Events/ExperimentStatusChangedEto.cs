namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Events
{
    /// <summary>
    /// Event published when an experiment's status changes
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.GrowthExperimentation.ExperimentStatusChanged")]
    public class ExperimentStatusChangedEto : ExperimentEtoBase
    {
        /// <summary>
        /// The previous status of the experiment
        /// </summary>
        public ExperimentStatus PreviousStatus { get; set; }
        
        /// <summary>
        /// The new status of the experiment
        /// </summary>
        public ExperimentStatus NewStatus { get; set; }
        
        /// <summary>
        /// When the status change occurred
        /// </summary>
        public DateTime StatusChangeTime { get; set; }
    }
}
