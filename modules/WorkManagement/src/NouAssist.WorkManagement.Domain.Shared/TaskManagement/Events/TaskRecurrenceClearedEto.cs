using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskRecurrenceCleared")]
    public class TaskRecurrenceClearedEto : IMultiTenant
    {
        public Guid TaskId { get; set; }
        public Guid? TenantId { get; set; }
        public int Version { get; set; }
        public DateTime ClearedAt { get; set; }
    }
}