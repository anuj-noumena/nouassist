using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.Integration.Events
{
    [EventName("NouAssist.Finance.BudgetVarianceDetected")]
    public class BudgetVarianceDetectedEto : IMultiTenant
    {
        public string IdempotencyKey { get; set; }
        public Guid BudgetId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public string BudgetName { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public MonetaryValue BudgetedAmount { get; set; }
        public MonetaryValue ActualSpent { get; set; }
        public MonetaryValue Variance { get; set; }
        public decimal VariancePercent { get; set; }
        public bool IsOverBudget { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public DateTime DetectedAt { get; set; }
    }
}
