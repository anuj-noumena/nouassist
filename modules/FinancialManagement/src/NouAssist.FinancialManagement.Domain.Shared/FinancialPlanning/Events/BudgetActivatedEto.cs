using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.FinancialPlanning.Events
{
    [EventName("NouAssist.FinancialPlanning.BudgetActivated")]
    public class BudgetActivatedEto : IMultiTenant
    {
        public Guid BudgetId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalBudget { get; set; }
        public string Currency { get; set; }
        public DateTime ActivatedAt { get; set; }
    }
}
