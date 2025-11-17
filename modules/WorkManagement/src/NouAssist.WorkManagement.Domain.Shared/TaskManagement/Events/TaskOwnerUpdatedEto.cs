using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskOwnerUpdated")]
    public class TaskOwnerUpdatedEto : IMultiTenant
    {
        public Guid TaskId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid OldOwnerId { get; set; }
        public Guid NewOwnerId { get; set; }
        public int Version { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
