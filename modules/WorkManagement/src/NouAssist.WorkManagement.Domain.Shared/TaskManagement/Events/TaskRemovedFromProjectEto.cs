using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskRemovedFromProject")]
    public class TaskRemovedFromProjectEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid? PreviousProjectId { get; set; }
        public DateTime RemovedAt { get; set; }
    }
}
