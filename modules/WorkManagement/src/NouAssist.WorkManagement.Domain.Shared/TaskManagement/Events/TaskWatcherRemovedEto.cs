using System;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskWatcherRemoved")]
    public class TaskWatcherRemovedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime RemovedAt { get; set; }
    }
}
