using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.DebtManagement.Events
{
    [EventName("NouAssist.DebtManagement.PayoffPlanCreated")]
    public class PayoffPlanCreatedEto : IMultiTenant
    {
        public string IdempotencyKey { get; set; }
        public Guid PlanId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public string PlanName { get; set; }
        public string Strategy { get; set; }  // Avalanche, Snowball, etc.
        public decimal MonthlyPaymentAmount { get; set; }
        public decimal TotalDebtAmount { get; set; }
        public int EstimatedMonthsToPayoff { get; set; }
        public decimal TotalInterestSavings { get; set; }
        public DateTime TargetPayoffDate { get; set; }
        public string Currency { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
