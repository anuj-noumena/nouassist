using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.AccountsPayable.Events;
using NouAssist.FinancialManagement.GeneralLedger.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.GeneralLedger.Handlers
{
    /// <summary>
    /// Handles BillPaidEto event from AccountsPayable module
    /// Creates automatic journal entry: Debit Expense, Credit Cash
    /// </summary>
    public class BillPaidEventHandler :
        IDistributedEventHandler<BillPaidEto>,
        ITransientDependency
    {
        private readonly JournalEntryAutomationService _automationService;
        private readonly ILogger<BillPaidEventHandler> _logger;

        public BillPaidEventHandler(
            JournalEntryAutomationService automationService,
            ILogger<BillPaidEventHandler> logger)
        {
            _automationService = automationService;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(BillPaidEto eventData)
        {
            try
            {
                _logger.LogInformation(
                    "Creating journal entry for bill payment: {BillNumber} - {VendorName} - {Amount}",
                    eventData.BillNumber,
                    eventData.VendorName,
                    eventData.Amount);

                var journalEntry = await _automationService.CreateBillPaymentEntryAsync(
                    billId: eventData.BillId,
                    billNumber: eventData.BillNumber,
                    amount: eventData.Amount,
                    expenseAccountId: eventData.ExpenseAccountId,
                    paymentAccountId: eventData.PaymentAccountId,
                    paymentDate: eventData.PaymentDate,
                    vendorId: eventData.VendorId,
                    vendorName: eventData.VendorName,
                    paymentReference: eventData.PaymentReference,
                    paidBy: eventData.PaidBy,
                    tenantId: eventData.TenantId
                );

                _logger.LogInformation(
                    "Journal entry created successfully: {BatchNumber} for bill {BillNumber}",
                    journalEntry.BatchNumber,
                    eventData.BillNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to create journal entry for bill payment: {BillNumber}",
                    eventData.BillNumber);
                throw;
            }
        }
    }
}
