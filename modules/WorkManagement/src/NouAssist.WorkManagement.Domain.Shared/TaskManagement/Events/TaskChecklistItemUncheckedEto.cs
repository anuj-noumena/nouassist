using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskChecklistItemUnchecked")]
    public class TaskChecklistItemUncheckedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid ChecklistItemId { get; set; }
        public DateTime UncheckedAt { get; set; }
    }
}
