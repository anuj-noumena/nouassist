using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.GTD.Events
{
    [EventName("NouAssist.GTD.TicklerSurfaced")]
    public class TicklerSurfacedEto : IMultiTenant
    {
        public Guid TicklerId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime SurfacedAt { get; set; }
        public string Title { get; set; }
    }
}