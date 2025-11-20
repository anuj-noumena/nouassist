using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.FinancialAnalytics.ReadModels;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.FinancialAnalytics.Services
{
    public class AnalyticsCalculationService : DomainService
    {
        public List<AccountBalanceSummary> GenerateAccountBalanceSummaries(
            List<LedgerAccount> accounts,
            DateTime reportDate)
        {
            var summaries = new List<AccountBalanceSummary>();
            var monthStart = new DateTime(reportDate.Year, reportDate.Month, 1);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);
            var yearStart = new DateTime(reportDate.Year, 1, 1);

            foreach (var account in accounts)
            {
                var currentBalance = account.CurrentBalance;

                // In a real implementation, these would query historical balances
                var monthStartBalance = currentBalance; // Simplified
                var monthEndBalance = currentBalance; // Simplified
                var yearToDateChange = MonetaryValue.Zero(currentBalance.Currency); // Simplified

                summaries.Add(new AccountBalanceSummary
                {
                    AccountId = account.Id,
                    AccountNumber = account.AccountNumber.Number,
                    AccountName = account.Name,
                    CurrentBalance = currentBalance,
                    MonthStartBalance = monthStartBalance,
                    MonthEndBalance = monthEndBalance,
                    YearToDateChange = yearToDateChange,
                    LastUpdated = Clock.Now
                });
            }

            return summaries;
        }

        public CashFlowProjection GenerateCashFlowProjection(
            List<Invoice> upcomingInvoices,
            List<Bill> upcomingBills,
            List<LinkedBankAccount> bankAccounts,
            int daysAhead = 30)
        {
            var projectionDate = Clock.Now.AddDays(daysAhead);
            var currency = "USD"; // Simplified - should be configurable

            // Calculate current balance from bank accounts
            var currentBalance = new MonetaryValue(
                bankAccounts.Sum(a => a.CurrentBalance.Amount),
                currency
            );

            // Calculate projected inflows from invoices
            var inflowDetails = upcomingInvoices
                .Where(i => i.DueDate <= projectionDate)
                .Select(i => new CashFlowItem
                {
                    Date = i.DueDate,
                    Description = $"Invoice {i.InvoiceNumber}",
                    Amount = i.TotalAmount,
                    Category = "Receivables",
                    Confidence = CalculateInvoiceConfidence(i)
                })
                .ToList();

            var projectedInflows = new MonetaryValue(
                inflowDetails.Sum(i => i.Amount.Amount * (i.Confidence / 100m)),
                currency
            );

            // Calculate projected outflows from bills
            var outflowDetails = upcomingBills
                .Where(b => b.DueDate <= projectionDate)
                .Select(b => new CashFlowItem
                {
                    Date = b.DueDate,
                    Description = $"Bill from vendor",
                    Amount = b.TotalAmount,
                    Category = "Payables",
                    Confidence = 100m // Bills are more certain
                })
                .ToList();

            var projectedOutflows = new MonetaryValue(
                outflowDetails.Sum(o => o.Amount.Amount),
                currency
            );

            var projectedBalance = new MonetaryValue(
                currentBalance.Amount + projectedInflows.Amount - projectedOutflows.Amount,
                currency
            );

            return new CashFlowProjection
            {
                ProjectionDate = projectionDate,
                CurrentBalance = currentBalance,
                ProjectedInflows = projectedInflows,
                ProjectedOutflows = projectedOutflows,
                ProjectedBalance = projectedBalance,
                InflowDetails = inflowDetails,
                OutflowDetails = outflowDetails
            };
        }

        private decimal CalculateInvoiceConfidence(Invoice invoice)
        {
            var daysUntilDue = (invoice.DueDate - Clock.Now).Days;

            // Confidence decreases as invoices get older or are far in the future
            return daysUntilDue switch
            {
                <= 7 => 95m,
                <= 14 => 90m,
                <= 30 => 85m,
                <= 60 => 75m,
                _ => 60m
            };
        }

        public BudgetPerformanceSummary GenerateBudgetPerformanceSummary(
            Budget budget,
            List<Expense> expenses)
        {
            var budgetedAmount = budget.TotalBudget.Amount;
            var actualAmount = expenses
                .Where(e => e.ExpenseDate >= budget.PeriodStart && e.ExpenseDate <= budget.PeriodEnd)
                .Sum(e => e.Amount.Amount);

            var variance = budgetedAmount - actualAmount;
            var percentUsed = budgetedAmount > 0 ? (actualAmount / budgetedAmount) * 100 : 0;

            var categoryBreakdown = budget.Allocations
                .Select(allocation =>
                {
                    var categoryActual = expenses
                        .Where(e => e.CategoryId == allocation.CategoryId
                            && e.ExpenseDate >= budget.PeriodStart
                            && e.ExpenseDate <= budget.PeriodEnd)
                        .Sum(e => e.Amount.Amount);

                    return new CategoryPerformance
                    {
                        CategoryId = allocation.CategoryId,
                        CategoryName = allocation.CategoryName,
                        Budgeted = allocation.BudgetedAmount,
                        Actual = new MonetaryValue(categoryActual, allocation.ActualAmount.Currency),
                        Variance = new MonetaryValue(
                            allocation.BudgetedAmount.Amount - categoryActual,
                            allocation.BudgetedAmount.Currency
                        )
                    };
                })
                .ToList();

            return new BudgetPerformanceSummary
            {
                BudgetId = budget.Id,
                BudgetName = budget.Name,
                PeriodStart = budget.PeriodStart,
                PeriodEnd = budget.PeriodEnd,
                TotalBudget = budget.TotalBudget,
                TotalSpent = new MonetaryValue(actualAmount, budget.TotalBudget.Currency),
                Remaining = new MonetaryValue(variance, budget.TotalBudget.Currency),
                PercentageUsed = percentUsed,
                CategoryBreakdown = categoryBreakdown,
                ReportDate = Clock.Now
            };
        }

        public FinancialHealthScore CalculateFinancialHealthScore(
            List<LedgerAccount> accounts,
            List<Bill> bills,
            List<Invoice> invoices,
            List<Budget> budgets,
            List<Expense> expenses)
        {
            var scores = new Dictionary<string, int>();

            // Liquidity score (0-25 points)
            var liquidityScore = CalculateLiquidityScore(accounts);
            scores["Liquidity"] = liquidityScore;

            // Payables health (0-25 points)
            var payablesScore = CalculatePayablesScore(bills);
            scores["Payables"] = payablesScore;

            // Receivables health (0-25 points)
            var receivablesScore = CalculateReceivablesScore(invoices);
            scores["Receivables"] = receivablesScore;

            // Budget adherence (0-25 points)
            var budgetScore = CalculateBudgetScore(budgets, expenses);
            scores["Budget"] = budgetScore;

            var totalScore = scores.Values.Sum();

            return new FinancialHealthScore(totalScore, scores, GetHealthRating(totalScore));
        }

        private int CalculateLiquidityScore(List<LedgerAccount> accounts)
        {
            // Simplified liquidity calculation
            var cashAccounts = accounts.Where(a => a.Name.Contains("Cash", StringComparison.OrdinalIgnoreCase));
            var totalCash = cashAccounts.Sum(a => a.CurrentBalance.Amount);

            return totalCash switch
            {
                > 100000 => 25,
                > 50000 => 20,
                > 25000 => 15,
                > 10000 => 10,
                > 5000 => 5,
                _ => 0
            };
        }

        private int CalculatePayablesScore(List<Bill> bills)
        {
            var overdueBills = bills.Count(b => b.DueDate < Clock.Now && b.Status != AccountsPayable.Enums.BillStatus.Paid);
            var totalBills = bills.Count;

            if (totalBills == 0) return 25;

            var overduePercent = (decimal)overdueBills / totalBills * 100;

            return overduePercent switch
            {
                0 => 25,
                < 5 => 20,
                < 10 => 15,
                < 20 => 10,
                < 30 => 5,
                _ => 0
            };
        }

        private int CalculateReceivablesScore(List<Invoice> invoices)
        {
            var overdueInvoices = invoices.Count(i => i.DueDate < Clock.Now && i.Status != AccountsReceivable.Enums.InvoiceStatus.Paid);
            var totalInvoices = invoices.Count;

            if (totalInvoices == 0) return 25;

            var overduePercent = (decimal)overdueInvoices / totalInvoices * 100;

            return overduePercent switch
            {
                0 => 25,
                < 5 => 20,
                < 10 => 15,
                < 20 => 10,
                < 30 => 5,
                _ => 0
            };
        }

        private int CalculateBudgetScore(List<Budget> budgets, List<Expense> expenses)
        {
            if (!budgets.Any()) return 25;

            var activeBudgets = budgets.Where(b => b.Status == FinancialPlanning.Enums.BudgetStatus.Active).ToList();
            if (!activeBudgets.Any()) return 25;

            var overBudgetCount = 0;
            foreach (var budget in activeBudgets)
            {
                var actualAmount = expenses
                    .Where(e => e.ExpenseDate >= budget.PeriodStart && e.ExpenseDate <= budget.PeriodEnd)
                    .Sum(e => e.Amount.Amount);

                if (actualAmount > budget.TotalBudget.Amount)
                    overBudgetCount++;
            }

            var overBudgetPercent = (decimal)overBudgetCount / activeBudgets.Count * 100;

            return overBudgetPercent switch
            {
                0 => 25,
                < 10 => 20,
                < 20 => 15,
                < 30 => 10,
                < 50 => 5,
                _ => 0
            };
        }

        private string GetHealthRating(int score)
        {
            return score switch
            {
                >= 90 => "Excellent",
                >= 75 => "Good",
                >= 60 => "Fair",
                >= 40 => "Poor",
                _ => "Critical"
            };
        }
    }

    public class FinancialHealthScore
    {
        public int TotalScore { get; }
        public Dictionary<string, int> ComponentScores { get; }
        public string HealthRating { get; }

        public FinancialHealthScore(int totalScore, Dictionary<string, int> componentScores, string healthRating)
        {
            TotalScore = totalScore;
            ComponentScores = componentScores;
            HealthRating = healthRating;
        }
    }
}
