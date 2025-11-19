using System;
using NouAssist.FinancialManagement.InvestmentManagement.Enums;
using NouAssist.SharedKernel.Extension;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.FinancialManagement.InvestmentManagement.Entities
{
    public class Holding : Entity
    {
        public Guid PortfolioId { get; private set; }
        public string Symbol { get; private set; }
        public string Name { get; private set; }
        public InvestmentType Type { get; private set; }
        public AssetClass AssetClass { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal AverageCostBasis { get; private set; }
        public decimal CurrentPrice { get; private set; }
        public decimal MarketValue { get; private set; }
        public DateTime LastPriceUpdate { get; private set; }

        protected Holding() { /* ORM */ }

        public Holding(
            Guid portfolioId,
            string symbol,
            string name,
            InvestmentType type,
            AssetClass assetClass,
            decimal quantity,
            decimal averageCostBasis,
            decimal currentPrice)
        {
            PortfolioId = portfolioId;
            Symbol = Check.NotNullOrWhiteSpace(symbol, nameof(symbol));
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Type = type;
            AssetClass = assetClass;
            Quantity = Ensure.NotNegative(quantity, nameof(quantity));
            AverageCostBasis = Ensure.NotNegative(averageCostBasis, nameof(averageCostBasis));
            UpdatePrice(currentPrice);
        }

        public void UpdatePrice(decimal newPrice)
        {
            CurrentPrice = Ensure.NotNegative(newPrice, nameof(newPrice));
            MarketValue = Quantity * CurrentPrice;
            LastPriceUpdate = DateTime.UtcNow;
        }

        public void AddQuantity(decimal additionalQuantity, decimal purchasePrice)
        {
            if (additionalQuantity <= 0)
            {
                throw new BusinessException("InvestmentManagement:QuantityMustBePositive");
            }

            var totalCost = (Quantity * AverageCostBasis) + (additionalQuantity * purchasePrice);
            Quantity += additionalQuantity;
            AverageCostBasis = totalCost / Quantity;
            MarketValue = Quantity * CurrentPrice;
        }

        public void ReduceQuantity(decimal quantityToSell)
        {
            if (quantityToSell <= 0 || quantityToSell > Quantity)
            {
                throw new BusinessException("InvestmentManagement:InvalidSellQuantity");
            }

            Quantity -= quantityToSell;
            MarketValue = Quantity * CurrentPrice;
        }

        public decimal GetUnrealizedGainLoss()
        {
            return MarketValue - (Quantity * AverageCostBasis);
        }

        public decimal GetUnrealizedGainLossPercentage()
        {
            var totalCost = Quantity * AverageCostBasis;
            return totalCost > 0 ? (GetUnrealizedGainLoss() / totalCost) * 100 : 0;
        }

        public override object[] GetKeys() => new object[] { PortfolioId, Symbol };
    }
}
