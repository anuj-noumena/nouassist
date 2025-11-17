using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskInstanceOwnerUpdated")]
    public class TaskInstanceOwnerUpdatedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid OldOwnerId { get; set; }
        public Guid NewOwnerId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
