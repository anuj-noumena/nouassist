using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.InvestmentManagement.ValueObjects
{
    public class HoldingPosition : ValueObject
    {
        public decimal Quantity { get; init; }
        public decimal AverageCostBasis { get; init; }
        public decimal CurrentPrice { get; init; }
        public decimal MarketValue { get; init; }
        public decimal UnrealizedGainLoss { get; init; }
        public decimal UnrealizedGainLossPercentage { get; init; }
        public DateTime LastUpdated { get; init; }

        public HoldingPosition(
            decimal quantity,
            decimal averageCostBasis,
            decimal currentPrice,
            DateTime lastUpdated)
        {
            Quantity = quantity;
            AverageCostBasis = averageCostBasis;
            CurrentPrice = currentPrice;
            MarketValue = quantity * currentPrice;
            
            var totalCost = quantity * averageCostBasis;
            UnrealizedGainLoss = MarketValue - totalCost;
            UnrealizedGainLossPercentage = totalCost > 0 
                ? (UnrealizedGainLoss / totalCost) * 100 
                : 0;
            
            LastUpdated = lastUpdated;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Quantity;
            yield return AverageCostBasis;
            yield return CurrentPrice;
            yield return MarketValue;
            yield return UnrealizedGainLoss;
            yield return UnrealizedGainLossPercentage;
            yield return LastUpdated;
        }
    }
}
