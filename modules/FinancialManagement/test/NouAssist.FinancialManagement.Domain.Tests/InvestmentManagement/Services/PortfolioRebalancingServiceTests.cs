using System;
using System.Collections.Generic;
using NouAssist.InvestmentManagement.Aggregates;
using NouAssist.InvestmentManagement.Enums;
using NouAssist.InvestmentManagement.Services;
using NouAssist.InvestmentManagement.ValueObjects;
using Shouldly;
using Xunit;

namespace NouAssist.InvestmentManagement.Tests.Services
{
    public class PortfolioRebalancingServiceTests
    {
        private readonly PortfolioRebalancingService _service;

        public PortfolioRebalancingServiceTests()
        {
            _service = new PortfolioRebalancingService();
        }

        [Fact]
        public void GetTargetAllocationForRiskTolerance_Conservative_ShouldReturnBalancedAllocation()
        {
            // Act
            var allocation = _service.GetTargetAllocationForRiskTolerance(RiskTolerance.Conservative);

            // Assert
            allocation.AssetClassPercentages.ShouldContainKey("FixedIncome");
            allocation.AssetClassPercentages["FixedIncome"].ShouldBe(60m);
            allocation.IsBalanced().ShouldBeTrue();
        }

        [Fact]
        public void CreateRebalancingPlan_WithUnbalancedPortfolio_ShouldCreateTrades()
        {
            // Arrange
            var portfolio = CreatePortfolioWithUnbalancedAllocation();
            var targetAllocation = _service.GetTargetAllocationForRiskTolerance(RiskTolerance.Moderate);

            // Act
            var plan = _service.CreateRebalancingPlan(portfolio, targetAllocation);

            // Assert
            plan.NeedsRebalancing.ShouldBeTrue();
            plan.Trades.ShouldNotBeEmpty();
        }

        [Fact]
        public void CreateRebalancingPlan_WithBalancedPortfolio_ShouldNotNeedRebalancing()
        {
            // Arrange
            var portfolio = CreateBalancedPortfolio();
            var targetAllocation = _service.GetTargetAllocationForRiskTolerance(RiskTolerance.Moderate);

            // Act
            var plan = _service.CreateRebalancingPlan(portfolio, targetAllocation);

            // Assert
            plan.NeedsRebalancing.ShouldBeFalse();
            plan.Trades.ShouldBeEmpty();
        }

        private Portfolio CreatePortfolioWithUnbalancedAllocation()
        {
            var portfolio = new Portfolio(
                Guid.NewGuid(),
                Guid.NewGuid(),
                null,
                "Test Portfolio",
                RiskTolerance.Moderate
            );

            portfolio.AddCash(100000m, DateTime.UtcNow);

            // Buy 90% stocks, 10% bonds - unbalanced for moderate risk
            portfolio.BuyHolding(
                "SPY",
                "S&P 500 ETF",
                InvestmentType.ETF,
                AssetClass.Equity,
                100,
                900m,
                0,
                DateTime.UtcNow
            );

            portfolio.BuyHolding(
                "AGG",
                "Bond ETF",
                InvestmentType.ETF,
                AssetClass.FixedIncome,
                100,
                100m,
                0,
                DateTime.UtcNow
            );

            return portfolio;
        }

        private Portfolio CreateBalancedPortfolio()
        {
            var portfolio = new Portfolio(
                Guid.NewGuid(),
                Guid.NewGuid(),
                null,
                "Balanced Portfolio",
                RiskTolerance.Moderate
            );

            portfolio.AddCash(100000m, DateTime.UtcNow);

            // 60% stocks, 30% bonds, 10% cash - balanced for moderate risk
            portfolio.BuyHolding(
                "SPY",
                "S&P 500 ETF",
                InvestmentType.ETF,
                AssetClass.Equity,
                100,
                600m,
                0,
                DateTime.UtcNow
            );

            portfolio.BuyHolding(
                "AGG",
                "Bond ETF",
                InvestmentType.ETF,
                AssetClass.FixedIncome,
                100,
                300m,
                0,
                DateTime.UtcNow
            );

            return portfolio;
        }
    }
}
