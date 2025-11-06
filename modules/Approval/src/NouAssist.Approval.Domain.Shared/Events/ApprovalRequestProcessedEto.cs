using System;
using NouAssist.Approval.Enums;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Approval.Events
{
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

    [EventName("NouAssist.Approval.RequestProcessed")]
    public class ApprovalRequestProcessedEto : IMultiTenant
    {
        public Guid RequestId { get; set; }
        public Guid? TenantId { get; set; }
        public ApprovalStatus Status { get; set; }
        public DateTime ProcessedAt { get; set; }
        public Guid ProcessedBy { get; set; }
        public string EntityType { get; set; }
        public Guid EntityId { get; set; }
    }

    [EventName("NouAssist.Approval.WorkflowCompleted")]
    public class ApprovalWorkflowCompletedEto : IMultiTenant
    {
        public Guid RequestId { get; set; }
        public Guid WorkflowId { get; set; }
        public Guid? TenantId { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CompletedAt { get; set; }
        public int TotalSteps { get; set; }
        public int CompletedSteps { get; set; }
        public string EntityType { get; set; }
        public Guid EntityId { get; set; }
    }
}
