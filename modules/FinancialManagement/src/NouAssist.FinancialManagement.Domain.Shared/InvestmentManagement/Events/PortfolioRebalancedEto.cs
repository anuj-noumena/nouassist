using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.InvestmentManagement.Domain.Shared.Events
{
    [EventName("NouAssist.InvestmentManagement.PortfolioRebalanced")]
    public class PortfolioRebalancedEto : IMultiTenant
    {
        public Guid PortfolioId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime RebalanceDate { get; set; }
        public int TransactionsExecuted { get; set; }
        public decimal TotalValue { get; set; }
    }
}
