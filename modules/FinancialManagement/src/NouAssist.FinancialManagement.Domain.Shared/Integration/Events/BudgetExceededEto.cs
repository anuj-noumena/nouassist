using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.Integration.Events
{
    [EventName("NouAssist.Finance.BudgetExceeded")]
    public class BudgetExceededEto : IMultiTenant
    {
        public Guid BudgetId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public string BudgetName { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public MonetaryValue AllocatedAmount { get; set; }
        public MonetaryValue ActualSpent { get; set; }
        public MonetaryValue ExcessAmount { get; set; }
        public decimal ExcessPercent { get; set; }
        public DateTime ExceededAt { get; set; }
    }
}
