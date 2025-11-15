using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.TaxManagement.Domain.Shared.Events
{
    [EventName("NouAssist.TaxManagement.TaxEstimateCalculated")]
    public class TaxEstimateCalculatedEto : IMultiTenant
    {
        public Guid EstimateId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public int TaxYear { get; set; }
        public decimal EstimatedTaxLiability { get; set; }
        public decimal EstimatedRefund { get; set; }
        public decimal EffectiveTaxRate { get; set; }
        public DateTime CalculatedAt { get; set; }
    }
}
