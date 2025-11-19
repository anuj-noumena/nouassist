using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.InvestmentManagement.Aggregates;
using NouAssist.FinancialManagement.InvestmentManagement.Enums;
using NouAssist.FinancialManagement.InvestmentManagement.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.InvestmentManagement.Services
{
    public class PerformanceCalculationService : DomainService
    {
        public PerformanceMetrics CalculatePerformance(
            Portfolio portfolio,
            DateTime periodStart,
            DateTime periodEnd)
        {
            // Get transactions in period
            var transactions = portfolio.Transactions
                .Where(t => t.TransactionDate >= periodStart && t.TransactionDate <= periodEnd)
                .OrderBy(t => t.TransactionDate)
                .ToList();

            if (!transactions.Any())
            {
                return new PerformanceMetrics(0, 0, 0, 0, 0, 0, periodStart, periodEnd);
            }

            // Calculate returns
            var deposits = transactions
                .Where(t => t.Type == TransactionType.Deposit || t.Type == TransactionType.Buy)
                .Sum(t => t.TotalAmount);
            
            var withdrawals = transactions
                .Where(t => t.Type == TransactionType.Withdrawal || t.Type == TransactionType.Sell)
                .Sum(t => t.TotalAmount);

            var netInvested = deposits - withdrawals;
            var currentValue = portfolio.TotalValue;
            
            var totalReturn = currentValue - netInvested;
            var totalReturnPercentage = netInvested > 0 
                ? (totalReturn / netInvested) * 100 
                : 0;

            // Calculate annualized return
            var days = (periodEnd - periodStart).TotalDays;
            var years = days / 365.25;
            var annualizedReturn = years > 0 
                ? (decimal)(Math.Pow((double)(1 + totalReturnPercentage / 100), 1 / years) - 1) * 100
                : 0;

            // Simplified volatility and Sharpe ratio (would need daily returns for accuracy)
            var volatility = CalculateSimplifiedVolatility(transactions);
            var riskFreeRate = 2.5m; // Example risk-free rate
            var sharpeRatio = volatility > 0 
                ? (annualizedReturn - riskFreeRate) / volatility 
                : 0;

            // Max drawdown calculation (simplified)
            var maxDrawdown = CalculateMaxDrawdown(transactions, currentValue);

            return new PerformanceMetrics(
                totalReturn,
                totalReturnPercentage,
                annualizedReturn,
                volatility,
                sharpeRatio,
                maxDrawdown,
                periodStart,
                periodEnd
            );
        }

        private decimal CalculateSimplifiedVolatility(List<Entities.InvestmentTransaction> transactions)
        {
            if (transactions.Count < 2)
            {
                return 0;
            }

            // Simplified standard deviation calculation
            var returns = new List<decimal>();
            for (int i = 1; i < transactions.Count; i++)
            {
                var returnValue = (transactions[i].TotalAmount - transactions[i - 1].TotalAmount) / 
                                 transactions[i - 1].TotalAmount;
                returns.Add(returnValue * 100);
            }

            var average = returns.Average();
            var sumOfSquares = returns.Sum(r => (r - average) * (r - average));
            return (decimal)Math.Sqrt((double)(sumOfSquares / returns.Count));
        }

        private decimal CalculateMaxDrawdown(
            List<Entities.InvestmentTransaction> transactions,
            decimal currentValue)
        {
            if (!transactions.Any())
            {
                return 0;
            }

            var peak = transactions.Max(t => t.TotalAmount);
            var trough = transactions.Min(t => t.TotalAmount);
            
            return peak > 0 ? ((peak - trough) / peak) * 100 : 0;
        }

        public decimal CalculateExpectedReturn(
            RiskTolerance riskTolerance,
            int timeHorizonYears)
        {
            // Historical average returns by risk level
            var expectedAnnualReturn = riskTolerance switch
            {
                RiskTolerance.Conservative => 4.0m,
                RiskTolerance.ModeratelyConservative => 5.5m,
                RiskTolerance.Moderate => 7.0m,
                RiskTolerance.ModeratelyAggressive => 8.5m,
                RiskTolerance.Aggressive => 10.0m,
                _ => 5.0m
            };

            return expectedAnnualReturn * timeHorizonYears;
        }
    }
}
