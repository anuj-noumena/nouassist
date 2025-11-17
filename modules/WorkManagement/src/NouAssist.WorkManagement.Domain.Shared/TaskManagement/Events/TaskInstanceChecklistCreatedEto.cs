using System;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskInstanceChecklistCreated")]
    public class TaskInstanceChecklistCreatedEto : IMultiTenant
    {
        public Guid TaskInstanceChecklistId { get; set; }
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
