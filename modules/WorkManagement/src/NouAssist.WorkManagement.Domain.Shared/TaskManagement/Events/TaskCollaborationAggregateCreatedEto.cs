using System;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskCollaborationAggregateCreated")]
    public class TaskCollaborationAggregateCreatedEto : IMultiTenant
    {
        public Guid TaskCollaborationAggregateId { get; set; }
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
