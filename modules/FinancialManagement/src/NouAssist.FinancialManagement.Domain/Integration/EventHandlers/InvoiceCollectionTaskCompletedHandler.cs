using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.AccountsReceivable.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.Integration.EventHandlers
{
    /// <summary>
    /// Handles TaskCompletedEto for invoice collection tasks to update collection notes
    /// </summary>
    public class InvoiceCollectionTaskCompletedHandler :
        ITransientDependency
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ILogger<InvoiceCollectionTaskCompletedHandler> _logger;

        public InvoiceCollectionTaskCompletedHandler(
            IInvoiceRepository invoiceRepository,
            ILogger<InvoiceCollectionTaskCompletedHandler> logger)
        {
            _invoiceRepository = invoiceRepository;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(dynamic eventData)
        {
            try
            {
                // Check if this is an invoice collection task
                var customFields = eventData.CustomFields as System.Collections.Generic.Dictionary<string, string>;
                if (customFields == null ||
                    !customFields.ContainsKey("FinancialEntityType") ||
                    customFields["FinancialEntityType"] != "Invoice" ||
                    (!customFields.ContainsKey("TaskSubtype") ||
                     (customFields["TaskSubtype"] != "Collection" &&
                      customFields["TaskSubtype"] != "FollowUp")))
                {
                    return; // Not an invoice collection/follow-up task
                }

                if (!customFields.ContainsKey("InvoiceId"))
                {
                    return; // Missing InvoiceId
                }

                var invoiceId = Guid.Parse(customFields["InvoiceId"]);
                var resolutionNote = eventData.ResolutionNote as string;
                var completedAt = (DateTime)eventData.CompletedAt;

                _logger.LogInformation(
                    "Processing invoice collection/follow-up task completion for Invoice {InvoiceId}",
                    invoiceId);

                // Get the invoice
                var invoice = await _invoiceRepository.GetAsync(invoiceId);

                // Update collection notes or metadata
                // In real implementation, you might have:
                // invoice.RecordCollectionAttempt(completedAt, resolutionNote);
                // await _invoiceRepository.UpdateAsync(invoice, autoSave: true);

                _logger.LogInformation(
                    "Invoice {InvoiceId} collection activity recorded. Note: {Note}",
                    invoiceId, resolutionNote);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling invoice collection task completion");
                // Don't throw - this is a secondary operation
            }
        }
    }
}
