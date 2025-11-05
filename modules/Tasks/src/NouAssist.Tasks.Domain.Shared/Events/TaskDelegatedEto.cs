using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Tasks.Events
{
    [EventName("NouAssist.Tasks.TaskDelegated")]
    public class TaskDelegatedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid DelegatedBy { get; set; }
        public Guid DelegatedTo { get; set; }
        public DateTime DelegatedAt { get; set; }
        public string Note { get; set; }
    }
}
