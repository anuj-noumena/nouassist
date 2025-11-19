using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskActionStepCompleted")]
    public class TaskActionStepCompletedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid ActionStepId { get; set; }
        public Guid CompletedBy { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
