using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.AuditTrail.Services;
using NouAssist.FinancialManagement.GeneralLedger.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.AuditTrail.Handlers
{
    /// <summary>
    /// Handles JournalBatchPostedEto event for audit trail
    /// Creates comprehensive audit entry for GAAP compliance
    /// </summary>
    public class JournalBatchPostedAuditHandler :
        IDistributedEventHandler<JournalBatchPostedEto>,
        ITransientDependency
    {
        private readonly FinancialAuditService _auditService;
        private readonly ILogger<JournalBatchPostedAuditHandler> _logger;

        public JournalBatchPostedAuditHandler(
            FinancialAuditService auditService,
            ILogger<JournalBatchPostedAuditHandler> logger)
        {
            _auditService = auditService;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(JournalBatchPostedEto eventData)
        {
            try
            {
                var description = $"Journal batch posted with {eventData.Lines?.Count ?? 0} entries";

                // Determine source type from idempotency key pattern
                var sourceType = "Manual";
                if (!string.IsNullOrEmpty(eventData.IdempotencyKey))
                {
                    if (eventData.IdempotencyKey.Contains("BillPaid"))
                        sourceType = "BillPayment";
                    else if (eventData.IdempotencyKey.Contains("InvoiceSent"))
                        sourceType = "InvoiceAccrual";
                    else if (eventData.IdempotencyKey.Contains("InvoicePaid"))
                        sourceType = "InvoicePayment";
                    else if (eventData.IdempotencyKey.Contains("DebtPayment"))
                        sourceType = "DebtPayment";
                    else if (eventData.IdempotencyKey.Contains("Expense"))
                        sourceType = "Expense";
                }

                await _auditService.AuditJournalEntryAsync(
                    batchId: eventData.BatchId,
                    batchNumber: eventData.BatchNumber,
                    totalAmount: eventData.TotalAmount.Amount,
                    description: description,
                    postedBy: eventData.PostedBy,
                    sourceType: sourceType,
                    tenantId: eventData.TenantId
                );

                _logger.LogDebug(
                    "Audit entry created for journal batch: {BatchNumber}, Source: {SourceType}",
                    eventData.BatchNumber,
                    sourceType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to create audit entry for journal batch: {BatchId}",
                    eventData.BatchId);
                // Don't throw - audit failure shouldn't break posting
            }
        }
    }
}
