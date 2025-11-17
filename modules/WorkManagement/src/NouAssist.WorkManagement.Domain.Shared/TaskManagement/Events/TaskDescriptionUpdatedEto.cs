using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskDescriptionUpdated")]
    public class TaskDescriptionUpdatedEto : IMultiTenant
    {
        public Guid TaskId { get; set; }
        public Guid? TenantId { get; set; }
        public string OldDescription { get; set; }
        public string NewDescription { get; set; }
        public int Version { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
