using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskRecurrenceSet")]
    public class TaskRecurrenceSetEto : IMultiTenant
    {
        public Guid TaskId { get; set; }
        public Guid? TenantId { get; set; }
        public string RecurrencePattern { get; set; }
        public int Version { get; set; }
        public DateTime SetAt { get; set; }
    }
}