using System;
using NouAssist.FinancialManagement.InvestmentManagement.Enums;
using NouAssist.SharedKernel.Extension;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.FinancialManagement.InvestmentManagement.Entities
{
    public class InvestmentTransaction : CreationAuditedEntity<Guid>
    {
        public Guid PortfolioId { get; private set; }
        public TransactionType Type { get; private set; }
        public string Symbol { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal Price { get; private set; }
        public decimal TotalAmount { get; private set; }
        public decimal Fees { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public string? Notes { get; set; }

        protected InvestmentTransaction() { /* ORM */ }

        public InvestmentTransaction(
            Guid id,
            Guid portfolioId,
            TransactionType type,
            string symbol,
            decimal quantity,
            decimal price,
            decimal fees,
            DateTime transactionDate)
            : base(id)
        {
            PortfolioId = portfolioId;
            Type = type;
            Symbol = Check.NotNullOrWhiteSpace(symbol, nameof(symbol));
            Quantity = quantity;
            Price = Ensure.NotNegative(price, nameof(price));
            Fees = Ensure.NotNegative(fees, nameof(fees));
            TransactionDate = transactionDate;
            
            CalculateTotalAmount();
        }

        private void CalculateTotalAmount()
        {
            TotalAmount = Type switch
            {
                TransactionType.Buy => (Quantity * Price) + Fees,
                TransactionType.Sell => (Quantity * Price) - Fees,
                TransactionType.Dividend or TransactionType.Interest => Quantity * Price,
                _ => Quantity * Price
            };
        }
    }
}
