using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.InvestmentManagement.Events;
using NouAssist.FinancialManagement.FinancialPlanning.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.FinancialPlanning.Handlers
{
    /// <summary>
    /// Handles PortfolioValueUpdatedEto event from InvestmentManagement module
    /// Updates net worth and retirement goals based on portfolio value changes
    /// </summary>
    public class PortfolioValueUpdatedHandler :
        IDistributedEventHandler<PortfolioValueUpdatedEto>,
        ITransientDependency
    {
        private readonly InvestmentGoalIntegrationService _investmentGoalService;
        private readonly ILogger<PortfolioValueUpdatedHandler> _logger;

        public PortfolioValueUpdatedHandler(
            InvestmentGoalIntegrationService investmentGoalService,
            ILogger<PortfolioValueUpdatedHandler> logger)
        {
            _investmentGoalService = investmentGoalService;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(PortfolioValueUpdatedEto eventData)
        {
            try
            {
                _logger.LogInformation(
                    "Updating financial goals from portfolio value: {PortfolioName} - {CurrentValue} ({Change:+0.00;-0.00}%)",
                    eventData.PortfolioName,
                    eventData.CurrentValue,
                    eventData.ChangePercent);

                // Update net worth goal
                await _investmentGoalService.UpdateNetWorthGoalFromPortfolioAsync(
                    portfolioId: eventData.PortfolioId,
                    portfolioName: eventData.PortfolioName,
                    portfolioCurrentValue: eventData.CurrentValue,
                    currency: eventData.Currency,
                    userId: eventData.UserId,
                    tenantId: eventData.TenantId
                );

                // Update retirement goal (if linked)
                await _investmentGoalService.UpdateRetirementGoalFromPortfolioAsync(
                    portfolioId: eventData.PortfolioId,
                    portfolioName: eventData.PortfolioName,
                    portfolioCurrentValue: eventData.CurrentValue,
                    currency: eventData.Currency,
                    userId: eventData.UserId,
                    tenantId: eventData.TenantId
                );

                _logger.LogInformation(
                    "Financial goals updated successfully from portfolio {PortfolioName}",
                    eventData.PortfolioName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to update financial goals from portfolio: {PortfolioId}",
                    eventData.PortfolioId);
                // Don't throw - goal update failure shouldn't break portfolio update
            }
        }
    }
}
