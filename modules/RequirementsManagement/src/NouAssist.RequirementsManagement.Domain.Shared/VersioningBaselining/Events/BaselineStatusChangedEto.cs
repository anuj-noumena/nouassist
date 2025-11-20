namespace NouAssist.RequirementsManagement.VersioningBaselining.Events
{
    /// <summary>
    /// Event triggered when a baseline's status is changed
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.VersioningBaselining.BaselineStatusChanged")]
    public class BaselineStatusChangedEto : VersionControlEtoBase
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
        /// Previous status of the baseline
        /// </summary>
        public BaselineStatus PreviousStatus { get; set; }
        
        /// <summary>
        /// New status of the baseline
        /// </summary>
        public BaselineStatus NewStatus { get; set; }
        
        /// <summary>
        /// ID of the user who changed the status
        /// </summary>
        public Guid ModifierId { get; set; }
        
        /// <summary>
        /// Timestamp when the status was changed
        /// </summary>
        public DateTime ModificationTime { get; set; }
    }
}
