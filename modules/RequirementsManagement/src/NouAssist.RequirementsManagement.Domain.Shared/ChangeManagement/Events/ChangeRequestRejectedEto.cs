using System;
using NouAssist.RequirementsManagement.ChangeManagement.Events.Base;
using NouAssist.RequirementsManagement.ChangeManagement.ValueObjects;
using Volo.Abp.EventBus;

namespace NouAssist.RequirementsManagement.ChangeManagement.Events
{
    /// <summary>
    /// Event published when a change request is rejected
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.ChangeManagement.ChangeRequestRejected")]
    public class ChangeRequestRejectedEto : ChangeRequestEtoBase
    {
        public Guid ApprovalStepId { get; set; }
        public Guid ApproverId { get; set; }
        public ApproverRole ApproverRole { get; set; }
        public string DecisionReason { get; set; }
        public DateTime RejectionDate { get; set; }
    }
}
