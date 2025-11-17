using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskTypeUpdated")]
    public class TaskTypeUpdatedEto : IMultiTenant
    {
        public Guid TaskId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid? OldTaskTypeId { get; set; }
        public Guid? NewTaskTypeId { get; set; }
        public int Version { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
