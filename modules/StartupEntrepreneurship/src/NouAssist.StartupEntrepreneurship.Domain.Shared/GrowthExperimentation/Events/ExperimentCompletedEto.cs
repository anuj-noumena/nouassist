namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Events
{
    /// <summary>
    /// Event published when an experiment is completed
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.GrowthExperimentation.ExperimentCompleted")]
    public class ExperimentCompletedEto : ExperimentEtoBase
    {
        /// <summary>
        /// Whether the experiment achieved statistical significance
        /// </summary>
        public bool IsSignificant { get; set; }
        
        /// <summary>
        /// The ID of the winning variant, if any
        /// </summary>
        public Guid? WinningVariantId { get; set; }
        
        /// <summary>
        /// When the experiment was completed
        /// </summary>
        public DateTime CompletionTime { get; set; }
        
        /// <summary>
        /// Duration of the experiment in days
        /// </summary>
        public int DurationInDays { get; set; }
    }
}
