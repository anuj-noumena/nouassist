using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskTitleUpdated")]
    public class TaskTitleUpdatedEto : IMultiTenant
    {
        public Guid TaskId { get; set; }
        public Guid? TenantId { get; set; }
        public string OldTitle { get; set; }
        public string NewTitle { get; set; }
        public int Version { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}