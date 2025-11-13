using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.FinancialPlanning.Events
{
    [EventName("FinancialManagement.FinancialPlanning.BudgetVarianceExceeded")]
    public class BudgetVarianceExceededEto : IMultiTenant
    {
        public string IdempotencyKey { get; set; }
        public Guid BudgetId { get; set; }
        public Guid CategoryId { get; set; }
        public MonetaryValue BudgetedAmount { get; set; }
        public MonetaryValue ActualAmount { get; set; }
        public decimal VariancePercentage { get; set; }
        public DateTime DetectedDate { get; set; }
        public Guid? TenantId { get; set; }
    }
}
