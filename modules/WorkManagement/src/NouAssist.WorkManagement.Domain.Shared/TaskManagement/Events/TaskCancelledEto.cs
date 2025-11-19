using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskCancelled")]
    public class TaskCancelledEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public string CancellationReason { get; set; }
        public DateTime CancelledAt { get; set; }
        public Guid? CancelledBy { get; set; }
    }
}
