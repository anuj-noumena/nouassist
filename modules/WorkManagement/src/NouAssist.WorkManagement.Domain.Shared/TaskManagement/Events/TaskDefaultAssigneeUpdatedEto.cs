using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskDefaultAssigneeUpdated")]
    public class TaskDefaultAssigneeUpdatedEto : IMultiTenant
    {
        public Guid TaskId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid? OldDefaultAssigneeId { get; set; }
        public Guid? NewDefaultAssigneeId { get; set; }
        public int Version { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
