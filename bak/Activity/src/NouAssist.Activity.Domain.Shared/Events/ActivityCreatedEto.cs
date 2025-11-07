using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Activity.Events
{
    [EventName("NouAssist.Activity.Created")]
    public class ActivityCreatedEto : IMultiTenant
    {
        public Guid ActivityId { get; set; }
        public Guid ActorId { get; set; }
        public string Action { get; set; }
        public string EntityType { get; set; }
        public Guid EntityId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime OccurredAt { get; set; }
    }
}
