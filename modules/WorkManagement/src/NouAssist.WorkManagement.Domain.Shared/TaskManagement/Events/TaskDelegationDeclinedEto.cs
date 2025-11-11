using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.DelegationDeclined")]
    public class TaskDelegationDeclinedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid DeclinedBy { get; set; }
        public DateTime DeclinedAt { get; set; }
        public string Reason { get; set; }
    }
}
