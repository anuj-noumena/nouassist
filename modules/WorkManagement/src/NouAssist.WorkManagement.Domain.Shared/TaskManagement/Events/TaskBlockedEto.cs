using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskBlocked")]
    public class TaskBlockedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public string Reason { get; set; }
        public DateTime BlockedAt { get; set; }
    }
}
