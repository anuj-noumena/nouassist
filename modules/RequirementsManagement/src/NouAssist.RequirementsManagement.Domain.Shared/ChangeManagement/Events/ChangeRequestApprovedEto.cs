namespace NouAssist.RequirementsManagement.ChangeManagement.Events
{
    /// <summary>
    /// Event published when a change request is approved
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.ChangeManagement.ChangeRequestApproved")]
    public class ChangeRequestApprovedEto : ChangeRequestEtoBase
    {
        public Guid ApprovalStepId { get; set; }
        public Guid ApproverId { get; set; }
        public ApproverRole ApproverRole { get; set; }
        public string DecisionReason { get; set; }
        public DateTime ApprovalDate { get; set; }
    }
}
