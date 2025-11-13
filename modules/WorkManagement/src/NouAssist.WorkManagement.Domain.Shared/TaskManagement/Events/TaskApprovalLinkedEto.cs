using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskApprovalLinked")]
    public class TaskApprovalLinkedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid ApprovalRequestId { get; set; }
        public DateTime LinkedAt { get; set; }
    }
}