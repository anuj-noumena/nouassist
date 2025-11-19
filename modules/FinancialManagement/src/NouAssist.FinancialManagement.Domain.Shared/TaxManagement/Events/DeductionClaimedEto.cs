using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.TaxManagement.Events
{
    [EventName("NouAssist.TaxManagement.DeductionClaimed")]
    public class DeductionClaimedEto : IMultiTenant
    {
        public Guid DeductionId { get; set; }
        public Guid TaxReturnId { get; set; }
        public Guid? TenantId { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime ClaimedAt { get; set; }
    }
}
