namespace NouAssist.IntelligenceStrategy.DecisionSupport.Events
{
    /// <summary>
    /// Event triggered when a strategic initiative is approved
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.DecisionSupport.StrategicInitiativeApproved")]
    public class StrategicInitiativeApprovedEto : StrategicInitiativeEtoBase
    {
        /// <summary>
        /// Optional ID of the corresponding project that will be created
        /// </summary>
        public Guid? ProjectId { get; set; }
        
        /// <summary>
        /// User ID of the approver
        /// </summary>
        public Guid ApproverId { get; set; }
        
        /// <summary>
        /// Timestamp when the initiative was approved
        /// </summary>
        public DateTime ApprovedOn { get; set; }
    }
}
