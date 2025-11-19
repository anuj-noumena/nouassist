using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskDependencyRemoved")]
    public class TaskDependencyRemovedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid DependsOnTaskId { get; set; }
        public DateTime RemovedAt { get; set; }
    }
}
