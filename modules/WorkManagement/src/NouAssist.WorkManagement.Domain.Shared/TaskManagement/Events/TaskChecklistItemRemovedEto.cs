using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskChecklistItemRemoved")]
    public class TaskChecklistItemRemovedEto : IMultiTenant
    {
        public Guid TaskId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid ChecklistItemId { get; set; }
        public int Version { get; set; }
        public DateTime RemovedAt { get; set; }
    }
}