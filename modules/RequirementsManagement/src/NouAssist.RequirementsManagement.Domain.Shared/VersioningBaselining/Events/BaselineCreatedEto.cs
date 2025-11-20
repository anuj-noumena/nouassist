namespace NouAssist.RequirementsManagement.VersioningBaselining.Events
{
    /// <summary>
    /// Event triggered when a new baseline is created
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.VersioningBaselining.BaselineCreated")]
    public class BaselineCreatedEto : VersionControlEtoBase
    {
        /// <summary>
        /// ID of the baseline
        /// </summary>
        public Guid BaselineId { get; set; }
        
        /// <summary>
        /// Name of the baseline
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the baseline
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// ID of the user who created the baseline
        /// </summary>
        public Guid CreatorId { get; set; }
        
        /// <summary>
        /// Current status of the baseline
        /// </summary>
        public BaselineStatus Status { get; set; }
        
        /// <summary>
        /// Timestamp when the baseline was created
        /// </summary>
        public DateTime CreationTime { get; set; }
        
        /// <summary>
        /// Count of requirements included in the baseline
        /// </summary>
        public int RequirementCount { get; set; }
    }
}
