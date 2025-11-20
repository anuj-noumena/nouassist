using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.InvestmentManagement.Events
{
    [EventName("NouAssist.InvestmentManagement.PortfolioValueUpdated")]
    public class PortfolioValueUpdatedEto : IMultiTenant
    {
        public string IdempotencyKey { get; set; }
        public Guid PortfolioId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public string PortfolioName { get; set; }
        public decimal PreviousValue { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal Change { get; set; }
        public decimal ChangePercent { get; set; }
        public decimal TotalCostBasis { get; set; }
        public decimal UnrealizedGainLoss { get; set; }
        public string Currency { get; set; }
        public DateTime ValueDate { get; set; }
        public string Reason { get; set; }  // "MarketChange", "Transaction", "Rebalance"
    }
}
