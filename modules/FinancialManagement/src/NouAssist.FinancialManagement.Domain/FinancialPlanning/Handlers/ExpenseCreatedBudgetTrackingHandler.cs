using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.FinancialPlanning.Events;
using NouAssist.FinancialManagement.FinancialPlanning.Services;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.FinancialPlanning.Handlers
{
    /// <summary>
    /// Handles ExpenseCreatedEto event (internal to FinancialPlanning)
    /// Tracks actual spending against budget
    /// </summary>
    public class ExpenseCreatedBudgetTrackingHandler :
        IDistributedEventHandler<ExpenseCreatedEto>,
        ITransientDependency
    {
        private readonly BudgetTrackingService _budgetTrackingService;
        private readonly ILogger<ExpenseCreatedBudgetTrackingHandler> _logger;

        public ExpenseCreatedBudgetTrackingHandler(
            BudgetTrackingService budgetTrackingService,
            ILogger<ExpenseCreatedBudgetTrackingHandler> logger)
        {
            _budgetTrackingService = budgetTrackingService;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(ExpenseCreatedEto eventData)
        {
            try
            {
                _logger.LogInformation(
                    "Tracking expense against budget: {ExpenseId} - {CategoryName} - {Amount}",
                    eventData.ExpenseId,
                    eventData.CategoryName,
                    eventData.Amount);

                var amount = new MonetaryValue(eventData.Amount, eventData.Currency);

                var result = await _budgetTrackingService.TrackActualSpendingAsync(
                    budgetId: eventData.BudgetId, // Use specified budget or auto-find
                    categoryId: eventData.CategoryId,
                    amount: amount,
                    transactionDate: eventData.ExpenseDate,
                    description: eventData.Description,
                    tenantId: eventData.TenantId
                );

                if (result.Success)
                {
                    _logger.LogInformation(
                        "Budget tracking successful for expense {ExpenseId}, Budget: {BudgetId}",
                        eventData.ExpenseId,
                        result.BudgetId);

                    if (result.VarianceEvent != null)
                    {
                        _logger.LogWarning(
                            "Budget variance detected after expense: {CategoryName} - {VariancePercent}% - IsOverBudget: {IsOverBudget}",
                            result.VarianceEvent.CategoryName,
                            result.VarianceEvent.VariancePercent,
                            result.VarianceEvent.IsOverBudget);
                    }
                }
                else
                {
                    _logger.LogWarning(
                        "No active budget found for expense: {ExpenseId}",
                        eventData.ExpenseId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to track expense against budget: {ExpenseId}",
                    eventData.ExpenseId);
                // Don't throw - budget tracking failure shouldn't break expense creation
            }
        }
    }
}
