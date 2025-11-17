using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskExpectedResultUpdated")]
    public class TaskExpectedResultUpdatedEto : IMultiTenant
    {
        public Guid TaskId { get; set; }
        public Guid? TenantId { get; set; }
        public string OldExpectedResult { get; set; }
        public string NewExpectedResult { get; set; }
        public int Version { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
