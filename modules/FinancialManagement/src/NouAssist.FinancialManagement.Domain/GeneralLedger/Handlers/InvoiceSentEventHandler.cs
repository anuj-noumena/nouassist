using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.AccountsReceivable.Events;
using NouAssist.FinancialManagement.GeneralLedger.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.GeneralLedger.Handlers
{
    /// <summary>
    /// Handles InvoiceSentEto event from AccountsReceivable module
    /// Creates automatic journal entry for accrual basis: Debit AR, Credit Revenue
    /// </summary>
    public class InvoiceSentEventHandler :
        IDistributedEventHandler<InvoiceSentEto>,
        ITransientDependency
    {
        private readonly JournalEntryAutomationService _automationService;
        private readonly ILogger<InvoiceSentEventHandler> _logger;

        public InvoiceSentEventHandler(
            JournalEntryAutomationService automationService,
            ILogger<InvoiceSentEventHandler> logger)
        {
            _automationService = automationService;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(InvoiceSentEto eventData)
        {
            try
            {
                _logger.LogInformation(
                    "Creating accrual journal entry for invoice: {InvoiceNumber} - {CustomerName} - {TotalAmount}",
                    eventData.InvoiceNumber,
                    eventData.CustomerName,
                    eventData.TotalAmount);

                var journalEntry = await _automationService.CreateInvoiceAccrualEntryAsync(
                    invoiceId: eventData.InvoiceId,
                    invoiceNumber: eventData.InvoiceNumber,
                    totalAmount: eventData.TotalAmount,
                    receivableAccountId: eventData.ReceivableAccountId,
                    revenueAccountId: eventData.RevenueAccountId,
                    issueDate: eventData.IssueDate,
                    customerId: eventData.CustomerId,
                    customerName: eventData.CustomerName,
                    tenantId: eventData.TenantId
                );

                _logger.LogInformation(
                    "Accrual journal entry created successfully: {BatchNumber} for invoice {InvoiceNumber}",
                    journalEntry.BatchNumber,
                    eventData.InvoiceNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to create accrual journal entry for invoice: {InvoiceNumber}",
                    eventData.InvoiceNumber);
                throw;
            }
        }
    }
}
