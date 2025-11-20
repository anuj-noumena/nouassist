using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.DebtManagement.Events;
using NouAssist.FinancialManagement.DebtManagement.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.CreditScoreMonitoring.Handlers
{
    /// <summary>
    /// Handles DebtPaidOffEto event from DebtManagement module
    /// Records credit score milestone and provides impact analysis
    /// </summary>
    public class DebtPaidOffCreditHandler :
        IDistributedEventHandler<DebtPaidOffEto>,
        ITransientDependency
    {
        private readonly CreditScoreImpactService _creditScoreImpactService;
        private readonly ILogger<DebtPaidOffCreditHandler> _logger;

        public DebtPaidOffCreditHandler(
            CreditScoreImpactService creditScoreImpactService,
            ILogger<DebtPaidOffCreditHandler> logger)
        {
            _creditScoreImpactService = creditScoreImpactService;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(DebtPaidOffEto eventData)
        {
            try
            {
                _logger.LogInformation(
                    "Recording credit score milestone for debt payoff: {DebtName} - {TotalPaid}",
                    eventData.DebtName,
                    eventData.TotalPaid);

                // In production, would fetch actual credit scores before/after
                // For now, using placeholder values
                var scoreBefore = 650; // TODO: Fetch from CreditReport repository
                var scoreAfter = 670;  // TODO: Fetch latest credit report

                var milestone = _creditScoreImpactService.RecordDebtPayoffMilestone(
                    debtName: eventData.DebtName,
                    amountPaid: eventData.TotalPaid,
                    scoreBefore: scoreBefore,
                    scoreAfter: scoreAfter,
                    paidOffDate: eventData.PaidOffDate
                );

                _logger.LogInformation(
                    "Credit milestone recorded: {Message}",
                    milestone.Message);

                // In production, would persist milestone to repository
                // and possibly trigger notification to user

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to record credit milestone for debt payoff: {DebtAccountId}",
                    eventData.DebtAccountId);
                // Don't throw - milestone recording failure shouldn't break debt payoff
            }
        }
    }
}
