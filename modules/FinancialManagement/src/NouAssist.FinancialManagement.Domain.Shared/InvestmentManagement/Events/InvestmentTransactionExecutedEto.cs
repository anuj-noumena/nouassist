using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.InvestmentManagement.Events
{
    [EventName("NouAssist.InvestmentManagement.InvestmentTransactionExecuted")]
    public class InvestmentTransactionExecutedEto : IMultiTenant
    {
        public string IdempotencyKey { get; set; }
        public Guid TransactionId { get; set; }
        public Guid PortfolioId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public string PortfolioName { get; set; }
        public string TransactionType { get; set; }  // Buy, Sell, Dividend, Interest
        public string Symbol { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public DateTime TransactionDate { get; set; }

        // For net worth tracking
        public decimal? RealizedGainLoss { get; set; }
        public decimal? PortfolioValueAfterTransaction { get; set; }

        // For GL integration
        public Guid? InvestmentAssetAccountId { get; set; }
        public Guid? CashAccountId { get; set; }
        public Guid? GainLossAccountId { get; set; }
    }
}
