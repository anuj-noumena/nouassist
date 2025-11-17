using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskDefaultWorkflowUpdated")]
    public class TaskDefaultWorkflowUpdatedEto : IMultiTenant
    {
        public Guid TaskId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid? OldDefaultWorkflowId { get; set; }
        public Guid? NewDefaultWorkflowId { get; set; }
        public int Version { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
