using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.DebtManagement.Events;
using NouAssist.FinancialManagement.GeneralLedger.Services;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.GeneralLedger.Handlers
{
    /// <summary>
    /// Handles DebtPaymentMadeEto event from DebtManagement module
    /// Creates automatic journal entry for debt payment:
    /// Debit: Debt Liability Account (Principal) + Interest Expense (Interest)
    /// Credit: Cash/Bank Account (Total Payment)
    /// </summary>
    public class DebtPaymentEventHandler :
        IDistributedEventHandler<DebtPaymentMadeEto>,
        ITransientDependency
    {
        private readonly JournalEntryAutomationService _automationService;
        private readonly ILogger<DebtPaymentEventHandler> _logger;

        public DebtPaymentEventHandler(
            JournalEntryAutomationService automationService,
            ILogger<DebtPaymentEventHandler> logger)
        {
            _automationService = automationService;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(DebtPaymentMadeEto eventData)
        {
            // Skip if GL accounts not specified
            if (!eventData.DebtAccountGLId.HasValue || !eventData.PaymentAccountGLId.HasValue)
            {
                _logger.LogWarning(
                    "Skipping GL entry for debt payment {PaymentId} - GL accounts not specified",
                    eventData.PaymentId);
                return;
            }

            try
            {
                _logger.LogInformation(
                    "Creating journal entry for debt payment: {DebtAccountName} - Principal: {Principal}, Interest: {Interest}",
                    eventData.DebtAccountName,
                    eventData.PrincipalAmount,
                    eventData.InterestAmount);

                await _automationService.CreateDebtPaymentEntryAsync(
                    paymentId: eventData.PaymentId,
                    debtAccountId: eventData.DebtAccountId,
                    debtAccountName: eventData.DebtAccountName,
                    principalAmount: new MonetaryValue(eventData.PrincipalAmount, eventData.Currency),
                    interestAmount: new MonetaryValue(eventData.InterestAmount, eventData.Currency),
                    debtLiabilityGLAccountId: eventData.DebtAccountGLId.Value,
                    interestExpenseGLAccountId: eventData.InterestExpenseGLId ?? eventData.DebtAccountGLId.Value,
                    paymentGLAccountId: eventData.PaymentAccountGLId.Value,
                    paymentDate: eventData.PaymentDate,
                    userId: eventData.UserId,
                    tenantId: eventData.TenantId
                );

                _logger.LogInformation(
                    "Journal entry created successfully for debt payment {PaymentId}",
                    eventData.PaymentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to create journal entry for debt payment: {PaymentId}",
                    eventData.PaymentId);
                throw;
            }
        }
    }
}
