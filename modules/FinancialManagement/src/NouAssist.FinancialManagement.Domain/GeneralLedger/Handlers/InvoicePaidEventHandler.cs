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
    /// Handles InvoicePaidEto event from AccountsReceivable module
    /// Creates automatic journal entry: Debit Cash, Credit Accounts Receivable
    /// </summary>
    public class InvoicePaidEventHandler :
        IDistributedEventHandler<InvoicePaidEto>,
        ITransientDependency
    {
        private readonly JournalEntryAutomationService _automationService;
        private readonly ILogger<InvoicePaidEventHandler> _logger;

        public InvoicePaidEventHandler(
            JournalEntryAutomationService automationService,
            ILogger<InvoicePaidEventHandler> logger)
        {
            _automationService = automationService;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(InvoicePaidEto eventData)
        {
            try
            {
                _logger.LogInformation(
                    "Creating journal entry for invoice payment: {InvoiceNumber} - {CustomerName} - {PaymentAmount}",
                    eventData.InvoiceNumber,
                    eventData.CustomerName,
                    eventData.PaymentAmount);

                var journalEntry = await _automationService.CreateInvoicePaymentEntryAsync(
                    invoiceId: eventData.InvoiceId,
                    invoiceNumber: eventData.InvoiceNumber,
                    paymentAmount: eventData.PaymentAmount,
                    depositAccountId: eventData.DepositAccountId,
                    receivableAccountId: eventData.RevenueAccountId, // Using RevenueAccountId as receivable
                    paymentDate: eventData.PaymentDate,
                    customerId: eventData.CustomerId,
                    customerName: eventData.CustomerName,
                    paymentReference: eventData.PaymentReference,
                    tenantId: eventData.TenantId
                );

                _logger.LogInformation(
                    "Journal entry created successfully: {BatchNumber} for invoice {InvoiceNumber}",
                    journalEntry.BatchNumber,
                    eventData.InvoiceNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to create journal entry for invoice payment: {InvoiceNumber}",
                    eventData.InvoiceNumber);
                throw;
            }
        }
    }
}
