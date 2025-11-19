using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Entities;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.FinancialPlanning.Services
{
    public class BudgetTrackingService : DomainService
    {
        public BudgetPerformance CalculateBudgetPerformance(
            Budget budget,
            List<Expense> actualExpenses)
        {
            var categoryPerformances = new List<CategoryPerformance>();
            var totalBudgeted = budget.TotalBudgetedAmount.Amount;
            var totalActual = 0m;

            foreach (var allocation in budget.CategoryAllocations)
            {
                var categoryExpenses = actualExpenses
                    .Where(e => e.CategoryId == allocation.CategoryId
                        && e.ExpenseDate >= budget.PeriodStart
                        && e.ExpenseDate <= budget.PeriodEnd)
                    .ToList();

                var actualAmount = categoryExpenses.Sum(e => e.Amount.Amount);
                totalActual += actualAmount;

                var variance = allocation.AllocatedAmount.Amount - actualAmount;
                var percentUsed = allocation.AllocatedAmount.Amount > 0
                    ? (actualAmount / allocation.AllocatedAmount.Amount) * 100
                    : 0;

                categoryPerformances.Add(new CategoryPerformance(
                    allocation.CategoryId,
                    allocation.CategoryName,
                    allocation.AllocatedAmount.Amount,
                    actualAmount,
                    variance,
                    percentUsed
                ));
            }

            var overallVariance = totalBudgeted - totalActual;
            var overallPercentUsed = totalBudgeted > 0 ? (totalActual / totalBudgeted) * 100 : 0;

            return new BudgetPerformance(
                budget.Id,
                budget.PeriodStart,
                budget.PeriodEnd,
                totalBudgeted,
                totalActual,
                overallVariance,
                overallPercentUsed,
                categoryPerformances
            );
        }

        public List<CategoryAllocation> GetOverspentCategories(
            Budget budget,
            List<Expense> actualExpenses)
        {
            return budget.CategoryAllocations
                .Where(allocation =>
                {
                    var actualAmount = actualExpenses
                        .Where(e => e.CategoryId == allocation.CategoryId
                            && e.ExpenseDate >= budget.PeriodStart
                            && e.ExpenseDate <= budget.PeriodEnd)
                        .Sum(e => e.Amount.Amount);

                    return actualAmount > allocation.AllocatedAmount.Amount;
                })
                .ToList();
        }

        public List<BudgetAlert> GenerateBudgetAlerts(
            Budget budget,
            List<Expense> actualExpenses,
            decimal warningThresholdPercent = 80)
        {
            var alerts = new List<BudgetAlert>();

            foreach (var allocation in budget.CategoryAllocations)
            {
                var actualAmount = actualExpenses
                    .Where(e => e.CategoryId == allocation.CategoryId
                        && e.ExpenseDate >= budget.PeriodStart
                        && e.ExpenseDate <= budget.PeriodEnd)
                    .Sum(e => e.Amount.Amount);

                var percentUsed = allocation.AllocatedAmount.Amount > 0
                    ? (actualAmount / allocation.AllocatedAmount.Amount) * 100
                    : 0;

                if (percentUsed >= 100)
                {
                    alerts.Add(new BudgetAlert(
                        allocation.CategoryId,
                        allocation.CategoryName,
                        BudgetAlertSeverity.Critical,
                        $"Category has exceeded budget by {percentUsed - 100:F1}%"
                    ));
                }
                else if (percentUsed >= warningThresholdPercent)
                {
                    alerts.Add(new BudgetAlert(
                        allocation.CategoryId,
                        allocation.CategoryName,
                        BudgetAlertSeverity.Warning,
                        $"Category has used {percentUsed:F1}% of budget"
                    ));
                }
            }

            return alerts;
        }

        public decimal CalculateRemainingBudget(Budget budget, List<Expense> actualExpenses)
        {
            var totalActual = actualExpenses
                .Where(e => e.ExpenseDate >= budget.PeriodStart && e.ExpenseDate <= budget.PeriodEnd)
                .Sum(e => e.Amount.Amount);

            return Math.Max(0, budget.TotalBudgetedAmount.Amount - totalActual);
        }

        public int CalculateDaysRemainingInPeriod(Budget budget)
        {
            return Math.Max(0, (budget.PeriodEnd - Clock.Now).Days);
        }

        public decimal CalculateDailyBudgetRate(Budget budget, List<Expense> actualExpenses)
        {
            var remainingBudget = CalculateRemainingBudget(budget, actualExpenses);
            var daysRemaining = CalculateDaysRemainingInPeriod(budget);

            return daysRemaining > 0 ? remainingBudget / daysRemaining : 0;
        }
    }

    public class BudgetPerformance
    {
        public Guid BudgetId { get; }
        public DateTime PeriodStart { get; }
        public DateTime PeriodEnd { get; }
        public decimal TotalBudgeted { get; }
        public decimal TotalActual { get; }
        public decimal Variance { get; }
        public decimal PercentUsed { get; }
        public List<CategoryPerformance> CategoryPerformances { get; }

        public BudgetPerformance(
            Guid budgetId,
            DateTime periodStart,
            DateTime periodEnd,
            decimal totalBudgeted,
            decimal totalActual,
            decimal variance,
            decimal percentUsed,
            List<CategoryPerformance> categoryPerformances)
        {
            BudgetId = budgetId;
            PeriodStart = periodStart;
            PeriodEnd = periodEnd;
            TotalBudgeted = totalBudgeted;
            TotalActual = totalActual;
            Variance = variance;
            PercentUsed = percentUsed;
            CategoryPerformances = categoryPerformances;
        }
    }

    public class CategoryPerformance
    {
        public Guid CategoryId { get; }
        public string CategoryName { get; }
        public decimal Budgeted { get; }
        public decimal Actual { get; }
        public decimal Variance { get; }
        public decimal PercentUsed { get; }
        public bool IsOverBudget => Actual > Budgeted;

        public CategoryPerformance(
            Guid categoryId,
            string categoryName,
            decimal budgeted,
            decimal actual,
            decimal variance,
            decimal percentUsed)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            Budgeted = budgeted;
            Actual = actual;
            Variance = variance;
            PercentUsed = percentUsed;
        }
    }

    public class BudgetAlert
    {
        public Guid CategoryId { get; }
        public string CategoryName { get; }
        public BudgetAlertSeverity Severity { get; }
        public string Message { get; }

        public BudgetAlert(Guid categoryId, string categoryName, BudgetAlertSeverity severity, string message)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            Severity = severity;
            Message = message;
        }
    }

    public enum BudgetAlertSeverity
    {
        Warning = 1,
        Critical = 2
    }
}
