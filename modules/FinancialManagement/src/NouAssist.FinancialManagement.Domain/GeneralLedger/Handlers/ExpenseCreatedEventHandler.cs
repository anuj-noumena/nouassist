using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.FinancialPlanning.Events;
using NouAssist.FinancialManagement.GeneralLedger.Services;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.GeneralLedger.Handlers
{
    /// <summary>
    /// Handles ExpenseCreatedEto event from FinancialPlanning module
    /// Creates automatic journal entry: Debit Expense, Credit Cash/Card
    /// </summary>
    public class ExpenseCreatedEventHandler :
        IDistributedEventHandler<ExpenseCreatedEto>,
        ITransientDependency
    {
        private readonly JournalEntryAutomationService _automationService;
        private readonly ILogger<ExpenseCreatedEventHandler> _logger;

        public ExpenseCreatedEventHandler(
            JournalEntryAutomationService automationService,
            ILogger<ExpenseCreatedEventHandler> logger)
        {
            _automationService = automationService;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(ExpenseCreatedEto eventData)
        {
            try
            {
                _logger.LogInformation(
                    "Creating journal entry for expense: {ExpenseId} - {CategoryName} - {Amount}",
                    eventData.ExpenseId,
                    eventData.CategoryName,
                    eventData.Amount);

                // Map category to expense account (simplified - in production, use category mapping service)
                var expenseAccountId = eventData.CategoryId;

                // Determine payment account from payment method (simplified)
                var paymentAccountId = GetPaymentAccountId(eventData.PaymentMethod, eventData.TenantId);

                var amount = new MonetaryValue(eventData.Amount, eventData.Currency);

                var journalEntry = await _automationService.CreateExpenseEntryAsync(
                    expenseId: eventData.ExpenseId,
                    amount: amount,
                    expenseAccountId: expenseAccountId,
                    paymentAccountId: paymentAccountId,
                    expenseDate: eventData.ExpenseDate,
                    description: eventData.Description,
                    userId: eventData.UserId,
                    paymentMethod: eventData.PaymentMethod ?? "Unknown",
                    tenantId: eventData.TenantId
                );

                _logger.LogInformation(
                    "Journal entry created successfully: {BatchNumber} for expense {ExpenseId}",
                    journalEntry.BatchNumber,
                    eventData.ExpenseId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to create journal entry for expense: {ExpenseId}",
                    eventData.ExpenseId);
                throw;
            }
        }

        private Guid GetPaymentAccountId(string paymentMethod, Guid? tenantId)
        {
            // TODO: In production, fetch from configuration or account mapping service
            // For now, return a placeholder - this should be configured per tenant
            // Common accounts: Cash, Credit Card, Debit Card, etc.

            // This is a simplified implementation
            // In a real system, you would:
            // 1. Have a mapping table: PaymentMethod -> LedgerAccountId
            // 2. Or query the LedgerAccountRepository for account by name/code
            // 3. Handle multi-tenant scenarios properly

            return Guid.Empty; // Placeholder - needs proper implementation
        }
    }
}
