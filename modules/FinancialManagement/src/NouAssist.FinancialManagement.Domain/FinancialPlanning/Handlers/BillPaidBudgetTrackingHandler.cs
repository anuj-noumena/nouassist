using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.AccountsPayable.Events;
using NouAssist.FinancialManagement.FinancialPlanning.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.FinancialPlanning.Handlers
{
    /// <summary>
    /// Handles BillPaidEto event from AccountsPayable module
    /// Tracks actual spending against budget
    /// </summary>
    public class BillPaidBudgetTrackingHandler :
        IDistributedEventHandler<BillPaidEto>,
        ITransientDependency
    {
        private readonly BudgetTrackingService _budgetTrackingService;
        private readonly ILogger<BillPaidBudgetTrackingHandler> _logger;

        public BillPaidBudgetTrackingHandler(
            BudgetTrackingService budgetTrackingService,
            ILogger<BillPaidBudgetTrackingHandler> logger)
        {
            _budgetTrackingService = budgetTrackingService;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(BillPaidEto eventData)
        {
            try
            {
                _logger.LogInformation(
                    "Tracking bill payment against budget: {BillNumber} - {VendorName} - {Amount}",
                    eventData.BillNumber,
                    eventData.VendorName,
                    eventData.Amount);

                var result = await _budgetTrackingService.TrackActualSpendingAsync(
                    budgetId: null, // Auto-find active budget
                    categoryId: eventData.CategoryId,
                    amount: eventData.Amount,
                    transactionDate: eventData.PaymentDate,
                    description: $"Bill #{eventData.BillNumber} - {eventData.VendorName}",
                    tenantId: eventData.TenantId
                );

                if (result.Success)
                {
                    _logger.LogInformation(
                        "Budget tracking successful for bill {BillNumber}, Budget: {BudgetId}",
                        eventData.BillNumber,
                        result.BudgetId);

                    if (result.VarianceEvent != null)
                    {
                        _logger.LogWarning(
                            "Budget variance detected after bill payment: {CategoryName} - {VariancePercent}%",
                            result.VarianceEvent.CategoryName,
                            result.VarianceEvent.VariancePercent);
                    }
                }
                else
                {
                    _logger.LogWarning(
                        "No active budget found for bill payment: {BillNumber}",
                        eventData.BillNumber);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to track bill payment against budget: {BillNumber}",
                    eventData.BillNumber);
                // Don't throw - budget tracking failure shouldn't break bill payment
            }
        }
    }
}
