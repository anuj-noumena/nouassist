using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.InvestmentManagement.Aggregates;
using NouAssist.InvestmentManagement.Enums;
using NouAssist.InvestmentManagement.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.InvestmentManagement.Services
{
    public class PortfolioRebalancingService : DomainService
    {
        public RebalancingPlan CreateRebalancingPlan(
            Portfolio portfolio,
            PortfolioAllocation targetAllocation)
        {
            var currentAllocation = portfolio.GetCurrentAllocation();
            var trades = new List<RebalancingTrade>();

            var totalValue = portfolio.TotalValue;
            if (totalValue == 0)
            {
                return new RebalancingPlan(portfolio.Id, trades, false, "Portfolio has no value");
            }

            foreach (var targetClass in targetAllocation.AssetClassPercentages)
            {
                var currentPercentage = currentAllocation.AssetClassPercentages.ContainsKey(targetClass.Key)
                    ? currentAllocation.AssetClassPercentages[targetClass.Key]
                    : 0;

                var targetPercentage = targetClass.Value;
                var difference = targetPercentage - currentPercentage;

                // Only rebalance if difference is significant (>5%)
                if (Math.Abs(difference) > 5)
                {
                    var targetValue = (targetPercentage / 100) * totalValue;
                    var currentValue = (currentPercentage / 100) * totalValue;
                    var tradeAmount = targetValue - currentValue;

                    var action = tradeAmount > 0 ? "Buy" : "Sell";
                    
                    trades.Add(new RebalancingTrade(
                        targetClass.Key,
                        action,
                        Math.Abs(tradeAmount),
                        difference
                    ));
                }
            }

            var needsRebalancing = trades.Any();
            var message = needsRebalancing 
                ? $"Portfolio needs rebalancing with {trades.Count} trades"
                : "Portfolio is balanced within tolerance";

            return new RebalancingPlan(portfolio.Id, trades, needsRebalancing, message);
        }

        public PortfolioAllocation GetTargetAllocationForRiskTolerance(
            RiskTolerance riskTolerance)
        {
            // Model portfolio allocations based on risk tolerance
            var allocations = riskTolerance switch
            {
                RiskTolerance.Conservative => new Dictionary<string, decimal>
                {
                    { "FixedIncome", 60m },
                    { "Equity", 30m },
                    { "Cash", 10m }
                },
                RiskTolerance.ModeratelyConservative => new Dictionary<string, decimal>
                {
                    { "FixedIncome", 45m },
                    { "Equity", 45m },
                    { "Cash", 10m }
                },
                RiskTolerance.Moderate => new Dictionary<string, decimal>
                {
                    { "Equity", 60m },
                    { "FixedIncome", 30m },
                    { "Cash", 10m }
                },
                RiskTolerance.ModeratelyAggressive => new Dictionary<string, decimal>
                {
                    { "Equity", 75m },
                    { "FixedIncome", 20m },
                    { "Cash", 5m }
                },
                RiskTolerance.Aggressive => new Dictionary<string, decimal>
                {
                    { "Equity", 85m },
                    { "FixedIncome", 10m },
                    { "Cash", 5m }
                },
                _ => new Dictionary<string, decimal>()
            };

            return new PortfolioAllocation(allocations);
        }
    }

    public class RebalancingPlan
    {
        public Guid PortfolioId { get; }
        public List<RebalancingTrade> Trades { get; }
        public bool NeedsRebalancing { get; }
        public string Message { get; }

        public RebalancingPlan(
            Guid portfolioId,
            List<RebalancingTrade> trades,
            bool needsRebalancing,
            string message)
        {
            PortfolioId = portfolioId;
            Trades = trades;
            NeedsRebalancing = needsRebalancing;
            Message = message;
        }
    }

    public class RebalancingTrade
    {
        public string AssetClass { get; }
        public string Action { get; }
        public decimal Amount { get; }
        public decimal AllocationDifference { get; }

        public RebalancingTrade(
            string assetClass,
            string action,
            decimal amount,
            decimal allocationDifference)
        {
            AssetClass = assetClass;
            Action = action;
            Amount = amount;
            AllocationDifference = allocationDifference;
        }
    }
}
