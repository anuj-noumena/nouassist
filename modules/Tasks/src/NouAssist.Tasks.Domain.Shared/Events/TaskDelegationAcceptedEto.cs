using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Tasks.Events
{
    [EventName("NouAssist.Tasks.DelegationAccepted")]
    public class TaskDelegationAcceptedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid AcceptedBy { get; set; }
        public DateTime AcceptedAt { get; set; }
        public string ResponseNote { get; set; }
    }
}
