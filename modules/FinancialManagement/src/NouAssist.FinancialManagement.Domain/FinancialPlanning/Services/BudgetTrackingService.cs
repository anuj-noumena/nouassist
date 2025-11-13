using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Data;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.FinancialPlanning.Services
{
    public class BudgetTrackingService : DomainService
    {
        private readonly IBudgetRepository _budgetRepository;

        public BudgetTrackingService(IBudgetRepository budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        public async Task TrackExpenseAgainstBudgetsAsync(
            Guid userId,
            Guid categoryId,
            MonetaryValue amount,
            DateTime transactionDate)
        {
            var activeBudgets = await _budgetRepository.GetActiveBudgetsAsync(userId);

            foreach (var budget in activeBudgets)
            {
                if (transactionDate >= budget.PeriodStart && transactionDate <= budget.PeriodEnd)
                {
                    var allocation = budget.Allocations.FirstOrDefault(a => a.CategoryId == categoryId);
                    if (allocation != null)
                    {
                        budget.TrackActual(categoryId, amount, transactionDate);
                        await _budgetRepository.UpdateAsync(budget);
                    }
                }
            }
        }

        public async Task<List<BudgetAlert>> EvaluateBudgetVariancesAsync(Guid budgetId)
        {
            var budget = await _budgetRepository.GetWithAllocationsAsync(budgetId);
            var alerts = new List<BudgetAlert>();

            foreach (var allocation in budget.Allocations)
            {
                var variancePercentage = allocation.CalculateVariancePercentage();

                if (variancePercentage >= 80 && variancePercentage < 90)
                {
                    alerts.Add(new BudgetAlert(
                        BudgetAlertType.Warning,
                        allocation.CategoryId,
                        $"Budget is {variancePercentage:F1}% used",
                        AlertSeverity.Medium));
                }
                else if (variancePercentage >= 90 && variancePercentage < 100)
                {
                    alerts.Add(new BudgetAlert(
                        BudgetAlertType.Critical,
                        allocation.CategoryId,
                        $"Budget is {variancePercentage:F1}% used",
                        AlertSeverity.High));
                }
                else if (variancePercentage >= 100)
                {
                    alerts.Add(new BudgetAlert(
                        BudgetAlertType.Exceeded,
                        allocation.CategoryId,
                        $"Budget exceeded by {variancePercentage - 100:F1}%",
                        AlertSeverity.Critical));
                }
            }

            return alerts;
        }
    }

    public class BudgetAlert
    {
        public BudgetAlertType Type { get; }
        public Guid CategoryId { get; }
        public string Message { get; }
        public AlertSeverity Severity { get; }

        public BudgetAlert(BudgetAlertType type, Guid categoryId, string message, AlertSeverity severity)
        {
            Type = type;
            CategoryId = categoryId;
            Message = message;
            Severity = severity;
        }
    }

    public enum BudgetAlertType
    {
        Warning,
        Critical,
        Exceeded
    }

    public enum AlertSeverity
    {
        Low,
        Medium,
        High,
        Critical
    }
}
