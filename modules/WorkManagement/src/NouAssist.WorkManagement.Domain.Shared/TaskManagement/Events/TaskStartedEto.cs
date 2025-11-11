using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Tasks.Events
{
    [EventName("NouAssist.Tasks.TaskStarted")]
    public class TaskStartedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid StartedBy { get; set; }
        public DateTime StartedAt { get; set; }
    }
}
