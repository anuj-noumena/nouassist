using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.TaxManagement.Domain.Shared.Events
{
    [EventName("NouAssist.TaxManagement.TaxReturnCreated")]
    public class TaxReturnCreatedEto : IMultiTenant
    {
        public Guid TaxReturnId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public int TaxYear { get; set; }
        public string FilingStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
