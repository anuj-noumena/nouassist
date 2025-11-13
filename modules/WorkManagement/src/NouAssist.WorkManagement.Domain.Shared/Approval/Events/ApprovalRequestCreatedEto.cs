using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.Approval.Events
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
}