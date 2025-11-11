using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskCompleted")]
    public class TaskCompletedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid CompletedBy { get; set; }
        public DateTime CompletedAt { get; set; }
        public int ActualMinutes { get; set; }
        public string Resolution { get; set; }
    }
}
