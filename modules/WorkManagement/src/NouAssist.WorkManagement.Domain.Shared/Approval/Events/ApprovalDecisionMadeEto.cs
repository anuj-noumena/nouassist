using System;
using NouAssist.WorkManagement.Approval.Enums;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;

namespace NouAssist.WorkManagement.Approval.Events
{
    [EventName("WorkManagement.Approval.DecisionMade")]
    public class ApprovalDecisionMadeEto : EtoBase
    {
        public Guid DecisionId { get; set; }
        public Guid RequestId { get; set; }
        public Guid StepId { get; set; }
        public Guid ApproverId { get; set; }
        public ApprovalAction Action { get; set; }
        public DateTime DecidedAt { get; set; }
        public Guid? TenantId { get; set; }
    }
}
