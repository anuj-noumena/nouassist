using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.DebtManagement.Events;
using NouAssist.FinancialManagement.FinancialPlanning.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.FinancialPlanning.Handlers
{
    /// <summary>
    /// Handles PayoffPlanCreatedEto event from DebtManagement module
    /// Creates a financial goal linked to the debt payoff plan
    /// </summary>
    public class PayoffPlanCreatedHandler :
        IDistributedEventHandler<PayoffPlanCreatedEto>,
        ITransientDependency
    {
        private readonly DebtGoalIntegrationService _debtGoalService;
        private readonly ILogger<PayoffPlanCreatedHandler> _logger;

        public PayoffPlanCreatedHandler(
            DebtGoalIntegrationService debtGoalService,
            ILogger<PayoffPlanCreatedHandler> logger)
        {
            _debtGoalService = debtGoalService;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(PayoffPlanCreatedEto eventData)
        {
            try
            {
                _logger.LogInformation(
                    "Creating financial goal from payoff plan: {PlanName} - Strategy: {Strategy}",
                    eventData.PlanName,
                    eventData.Strategy);

                var goal = await _debtGoalService.CreateGoalFromPayoffPlanAsync(
                    payoffPlanId: eventData.PlanId,
                    planName: eventData.PlanName,
                    strategy: eventData.Strategy,
                    totalDebtAmount: eventData.TotalDebtAmount,
                    monthlyPaymentAmount: eventData.MonthlyPaymentAmount,
                    targetPayoffDate: eventData.TargetPayoffDate,
                    currency: eventData.Currency,
                    userId: eventData.UserId,
                    tenantId: eventData.TenantId
                );

                _logger.LogInformation(
                    "Financial goal created successfully: {GoalId} - {GoalName}",
                    goal.Id,
                    goal.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to create financial goal from payoff plan: {PlanId}",
                    eventData.PlanId);
                // Don't throw - goal creation failure shouldn't break payoff plan creation
            }
        }
    }
}
