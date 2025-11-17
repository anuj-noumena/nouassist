using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskActionStepItemRemoved")]
    public class TaskActionStepItemRemovedEto : IMultiTenant
    {
        public Guid TaskId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid ActionStepId { get; set; }
        public Guid ActionStepItemId { get; set; }
        public int Version { get; set; }
        public DateTime RemovedAt { get; set; }
    }
}