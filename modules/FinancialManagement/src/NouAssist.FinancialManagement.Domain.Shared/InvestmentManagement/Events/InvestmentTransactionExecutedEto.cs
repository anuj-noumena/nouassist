using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.InvestmentManagement.Events
{
    [EventName("NouAssist.InvestmentManagement.InvestmentTransactionExecuted")]
    public class InvestmentTransactionExecutedEto : IMultiTenant
    {
        public Guid TransactionId { get; set; }
        public Guid PortfolioId { get; set; }
        public Guid? TenantId { get; set; }
        public string TransactionType { get; set; }
        public string Symbol { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
