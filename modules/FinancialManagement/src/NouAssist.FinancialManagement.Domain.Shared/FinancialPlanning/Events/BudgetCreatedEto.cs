using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.FinancialPlanning.Events
{
    [EventName("NouAssist.FinancialPlanning.BudgetCreated")]
    public class BudgetCreatedEto : IMultiTenant
    {
        public Guid BudgetId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string BudgetType { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public decimal TotalBudget { get; set; }
        public string Currency { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
