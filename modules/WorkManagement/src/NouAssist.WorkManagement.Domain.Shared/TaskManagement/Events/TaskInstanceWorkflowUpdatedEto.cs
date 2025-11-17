using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskInstanceWorkflowUpdated")]
    public class TaskInstanceWorkflowUpdatedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid OldWorkflowId { get; set; }
        public Guid NewWorkflowId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
