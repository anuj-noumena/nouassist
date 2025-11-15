using System;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;

namespace NouAssist.WorkManagement.Approval.Events
{
    [EventName("WorkManagement.Approval.RequestCreated")]
    public class ApprovalRequestCreatedEto : EtoBase
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