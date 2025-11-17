using System;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskDependencyAdded")]
    public class TaskDependencyAddedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid DependsOnTaskInstanceId { get; set; }
        public string DependencyType { get; set; }
        public int LagMinutes { get; set; }
        public bool IsBlocking { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
