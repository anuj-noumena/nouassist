using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.DebtManagement.Events;
using NouAssist.FinancialManagement.FinancialPlanning.Services;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.FinancialPlanning.Handlers
{
    /// <summary>
    /// Handles DebtPaymentMadeEto event from DebtManagement module
    /// Tracks debt payments against budget
    /// </summary>
    public class DebtPaymentBudgetTrackingHandler :
        IDistributedEventHandler<DebtPaymentMadeEto>,
        ITransientDependency
    {
        private readonly BudgetTrackingService _budgetTrackingService;
        private readonly ILogger<DebtPaymentBudgetTrackingHandler> _logger;

        public DebtPaymentBudgetTrackingHandler(
            BudgetTrackingService budgetTrackingService,
            ILogger<DebtPaymentBudgetTrackingHandler> logger)
        {
            _budgetTrackingService = budgetTrackingService;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(DebtPaymentMadeEto eventData)
        {
            // Skip if no budget category specified
            if (!eventData.BudgetCategoryId.HasValue)
            {
                _logger.LogWarning(
                    "Skipping budget tracking for debt payment {PaymentId} - No budget category specified",
                    eventData.PaymentId);
                return;
            }

            try
            {
                _logger.LogInformation(
                    "Tracking debt payment against budget: {DebtAccountName} - {PaymentAmount}",
                    eventData.DebtAccountName,
                    eventData.PaymentAmount);

                var amount = new MonetaryValue(eventData.PaymentAmount, eventData.Currency);

                var result = await _budgetTrackingService.TrackActualSpendingAsync(
                    budgetId: null, // Auto-find active budget
                    categoryId: eventData.BudgetCategoryId.Value,
                    amount: amount,
                    transactionDate: eventData.PaymentDate,
                    description: $"Debt payment - {eventData.DebtAccountName} (Principal: {eventData.PrincipalAmount}, Interest: {eventData.InterestAmount})",
                    tenantId: eventData.TenantId
                );

                if (result.Success)
                {
                    _logger.LogInformation(
                        "Budget tracking successful for debt payment {PaymentId}, Budget: {BudgetId}",
                        eventData.PaymentId,
                        result.BudgetId);

                    if (result.VarianceEvent != null)
                    {
                        _logger.LogWarning(
                            "Budget variance detected after debt payment: Debt Payments - {VariancePercent}%",
                            result.VarianceEvent.VariancePercent);
                    }
                }
                else
                {
                    _logger.LogWarning(
                        "No active budget found for debt payment: {PaymentId}",
                        eventData.PaymentId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to track debt payment against budget: {PaymentId}",
                    eventData.PaymentId);
                // Don't throw - budget tracking failure shouldn't break debt payment
            }
        }
    }
}
