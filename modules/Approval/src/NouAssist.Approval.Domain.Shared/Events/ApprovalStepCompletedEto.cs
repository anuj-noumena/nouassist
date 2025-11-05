using System;
using System.Collections.Generic;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Approval.Events
{
    [EventName("NouAssist.Approval.StepCompleted")]
    public class ApprovalStepCompletedEto : IMultiTenant
    {
        public Guid RequestId { get; set; }
        public Guid StepId { get; set; }
        public Guid WorkflowId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime CompletedAt { get; set; }
        public bool IsLastStep { get; set; }
        public List<Guid> DecisionMakers { get; set; }
    }

    [EventName("NouAssist.Approval.RequestCreated")]
    public class ApprovalRequestCreatedEto : IMultiTenant
    {
        public Guid RequestId { get; set; }
        public Guid WorkflowId { get; set; }
        public string EntityType { get; set; }
        public Guid EntityId { get; set; }
        public Guid RequestedBy { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
