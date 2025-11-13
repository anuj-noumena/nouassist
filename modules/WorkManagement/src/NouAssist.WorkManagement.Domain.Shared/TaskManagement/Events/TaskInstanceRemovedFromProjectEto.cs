using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskInstanceRemovedFromProject")]
    public class TaskInstanceRemovedFromProjectEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime RemovedAt { get; set; }
    }
}