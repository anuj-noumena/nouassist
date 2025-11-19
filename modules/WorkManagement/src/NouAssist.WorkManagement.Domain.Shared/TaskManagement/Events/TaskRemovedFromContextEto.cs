using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskRemovedFromContext")]
    public class TaskRemovedFromContextEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid? PreviousContextId { get; set; }
        public DateTime RemovedAt { get; set; }
    }
}
