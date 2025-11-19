using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskRemovedFromArea")]
    public class TaskRemovedFromAreaEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid? PreviousAreaId { get; set; }
        public DateTime RemovedAt { get; set; }
    }
}
