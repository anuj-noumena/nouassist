using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Data;
using NouAssist.FinancialManagement.FinancialPlanning.Entities;
using NouAssist.FinancialManagement.FinancialPlanning.Events;
using NouAssist.FinancialManagement.Integration.Events;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.FinancialPlanning.Services
{
    public class BudgetTrackingService : DomainService
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly ILogger<BudgetTrackingService> _logger;
        private readonly IDistributedEventBus _distributedEventBus;
        public BudgetTrackingService(
            IBudgetRepository budgetRepository,
            ILogger<BudgetTrackingService> logger,
            IDistributedEventBus distributedEventBus)
        {
            _budgetRepository = budgetRepository;
            _logger = logger;
            _distributedEventBus = distributedEventBus;
        }

        /// <summary>
        /// Track actual spending from bills, expenses, or payments against budget
        /// Automatically checks for variance and publishes alerts if needed
        /// </summary>
        [UnitOfWork]
        public virtual async Task<BudgetTrackingResult> TrackActualSpendingAsync(
            Guid? budgetId,
            Guid categoryId,
            MonetaryValue amount,
            DateTime transactionDate,
            string description,
            Guid? tenantId = null)
        {
            Check.NotNull(amount, nameof(amount));

            Budget budget;

            // If no budget ID provided, find active budget for the period
            if (budgetId.HasValue)
            {
                budget = await _budgetRepository.GetAsync(budgetId.Value);
            }
            else
            {
                budget = await FindActiveBudgetForDateAsync(tenantId, transactionDate);
                if (budget == null)
                {
                    _logger.LogWarning(
                        "No active budget found for date {TransactionDate}, skipping budget tracking",
                        transactionDate);
                    return new BudgetTrackingResult(false, null, null);
                }
            }

            // Update actual spent
            try
            {
                budget.TrackActual(categoryId, amount);
                await _budgetRepository.UpdateAsync(budget, autoSave: true);

                _logger.LogInformation(
                    "Tracked actual spending: {Amount} for category {CategoryId} in budget {BudgetId}",
                    amount,
                    categoryId,
                    budget.Id);

                // Check for variance
                var variancePercent = budget.GetVariancePercentage();
                var allocation = budget.Allocations.FirstOrDefault(a => a.CategoryId == categoryId);

                BudgetVarianceDetectedEto varianceEvent = null;

                if (allocation != null)
                {
                    var categoryVariancePercent = allocation.BudgetedAmount.Amount > 0
                        ? (allocation.ActualAmount.Amount / allocation.BudgetedAmount.Amount) * 100
                        : 0;

                    // Publish variance event if over threshold (>10% variance or exceeded budget)
                    if (categoryVariancePercent > 110 || (categoryVariancePercent > 90 && categoryVariancePercent < 110))
                    {
                        varianceEvent = new BudgetVarianceDetectedEto
                        {
                            IdempotencyKey = Guid.NewGuid().ToString(),
                            BudgetId = budget.Id,
                            TenantId = budget.TenantId,
                            UserId = budget.UserId,
                            BudgetName = budget.Name,
                            CategoryId = categoryId,
                            CategoryName = allocation.CategoryName,
                            BudgetedAmount = allocation.BudgetedAmount,
                            ActualSpent = allocation.ActualAmount,
                            Variance = new MonetaryValue(
                                allocation.ActualAmount.Amount - allocation.BudgetedAmount.Amount,
                                allocation.ActualAmount.Currency),
                            VariancePercent = categoryVariancePercent - 100,
                            IsOverBudget = categoryVariancePercent > 100,
                            PeriodStart = budget.PeriodStart,
                            PeriodEnd = budget.PeriodEnd,
                            DetectedAt = Clock.Now
                        };

                        _distributedEventBus.PublishAsync(varianceEvent);

                        _logger.LogWarning(
                            "Budget variance detected: Category {CategoryName} is at {VariancePercent}% ({IsOverBudget})",
                            allocation.CategoryName,
                            categoryVariancePercent,
                            categoryVariancePercent > 100 ? "OVER BUDGET" : "WARNING");
                    }
                }

                return new BudgetTrackingResult(true, budget.Id, varianceEvent);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Failed to track actual spending for budget {BudgetId}", budget.Id);
                throw;
            }
        }

        /// <summary>
        /// Find active budget for a specific date and tenant
        /// </summary>
        public virtual async Task<Budget> FindActiveBudgetForDateAsync(Guid? tenantId, DateTime date)
        {
            var budgets = await _budgetRepository.GetListAsync();

            return budgets
                .Where(b => b.TenantId == tenantId
                    && b.Status == Enums.BudgetStatus.Active
                    && b.PeriodStart <= date
                    && b.PeriodEnd >= date)
                .OrderByDescending(b => b.CreationTime)
                .FirstOrDefault();
        }

        /// <summary>
        /// Check if adding an expense would exceed budget
        /// </summary>
        public virtual async Task<BudgetCheckResult> CheckBudgetAvailabilityAsync(
            Guid categoryId,
            MonetaryValue plannedAmount,
            DateTime transactionDate,
            Guid? tenantId = null)
        {
            var budget = await FindActiveBudgetForDateAsync(tenantId, transactionDate);
            if (budget == null)
            {
                return new BudgetCheckResult(true, null, "No active budget found");
            }

            var allocation = budget.Allocations.FirstOrDefault(a => a.CategoryId == categoryId);
            if (allocation == null)
            {
                return new BudgetCheckResult(true, null, "Category not in budget");
            }

            var projectedSpent = allocation.ActualAmount.Add(plannedAmount);
            var wouldExceed = projectedSpent.Amount > allocation.BudgetedAmount.Amount;
            var projectedPercent = (projectedSpent.Amount / allocation.BudgetedAmount.Amount) * 100;

            return new BudgetCheckResult(
                !wouldExceed,
                budget.Id,
                wouldExceed
                    ? $"Would exceed budget by {projectedPercent - 100:F1}%"
                    : $"Within budget ({projectedPercent:F1}% used)");
        }

        public BudgetPerformance CalculateBudgetPerformance(
            Budget budget,
            List<Expense> actualExpenses)
        {
            var categoryPerformances = new List<CategoryPerformance>();
            var totalBudgeted = budget.TotalBudget.Amount;
            var totalActual = 0m;

            foreach (var allocation in budget.Allocations)
            {
                var categoryExpenses = actualExpenses
                    .Where(e => e.CategoryId == allocation.CategoryId
                        && e.ExpenseDate >= budget.PeriodStart
                        && e.ExpenseDate <= budget.PeriodEnd)
                    .ToList();

                var actualAmount = categoryExpenses.Sum(e => e.Amount.Amount);
                totalActual += actualAmount;

                var variance = allocation.BudgetedAmount.Amount - actualAmount;
                var percentUsed = allocation.BudgetedAmount.Amount > 0
                    ? (actualAmount / allocation.BudgetedAmount.Amount) * 100
                    : 0;

                categoryPerformances.Add(new CategoryPerformance(
                    allocation.CategoryId,
                    allocation.CategoryName,
                    allocation.BudgetedAmount.Amount,
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
            return budget.Allocations
                .Where(allocation =>
                {
                    var actualAmount = actualExpenses
                        .Where(e => e.CategoryId == allocation.CategoryId
                            && e.ExpenseDate >= budget.PeriodStart
                            && e.ExpenseDate <= budget.PeriodEnd)
                        .Sum(e => e.Amount.Amount);

                    return actualAmount > allocation.BudgetedAmount.Amount;
                })
                .ToList();
        }

        public List<BudgetAlert> GenerateBudgetAlerts(
            Budget budget,
            List<Expense> actualExpenses,
            decimal warningThresholdPercent = 80)
        {
            var alerts = new List<BudgetAlert>();

            foreach (var allocation in budget.Allocations)
            {
                var actualAmount = actualExpenses
                    .Where(e => e.CategoryId == allocation.CategoryId
                        && e.ExpenseDate >= budget.PeriodStart
                        && e.ExpenseDate <= budget.PeriodEnd)
                    .Sum(e => e.Amount.Amount);

                var percentUsed = allocation.BudgetedAmount.Amount > 0
                    ? (actualAmount / allocation.BudgetedAmount.Amount) * 100
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

            return Math.Max(0, budget.TotalBudget.Amount - totalActual);
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

    public class BudgetTrackingResult
    {
        public bool Success { get; }
        public Guid? BudgetId { get; }
        public BudgetVarianceDetectedEto VarianceEvent { get; }

        public BudgetTrackingResult(bool success, Guid? budgetId, BudgetVarianceDetectedEto varianceEvent)
        {
            Success = success;
            BudgetId = budgetId;
            VarianceEvent = varianceEvent;
        }
    }

    public class BudgetCheckResult
    {
        public bool IsWithinBudget { get; }
        public Guid? BudgetId { get; }
        public string Message { get; }

        public BudgetCheckResult(bool isWithinBudget, Guid? budgetId, string message)
        {
            IsWithinBudget = isWithinBudget;
            BudgetId = budgetId;
            Message = message;
        }
    }
}
