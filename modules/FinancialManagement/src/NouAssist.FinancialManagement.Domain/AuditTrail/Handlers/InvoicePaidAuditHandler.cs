using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.AccountsReceivable.Events;
using NouAssist.FinancialManagement.AuditTrail.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.AuditTrail.Handlers
{
    /// <summary>
    /// Handles InvoicePaidEto event for audit trail
    /// Creates comprehensive audit entry for revenue recognition compliance
    /// </summary>
    public class InvoicePaidAuditHandler :
        IDistributedEventHandler<InvoicePaidEto>,
        ITransientDependency
    {
        private readonly FinancialAuditService _auditService;
        private readonly ILogger<InvoicePaidAuditHandler> _logger;

        public InvoicePaidAuditHandler(
            FinancialAuditService auditService,
            ILogger<InvoicePaidAuditHandler> logger)
        {
            _auditService = auditService;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(InvoicePaidEto eventData)
        {
            try
            {
                await _auditService.AuditInvoicePaymentAsync(
                    invoiceId: eventData.InvoiceId,
                    invoiceNumber: eventData.InvoiceNumber,
                    amount: eventData.PaymentAmount.Amount,
                    currency: eventData.PaymentAmount.Currency,
                    customerId: eventData.CustomerId,
                    customerName: eventData.CustomerName,
                    paymentSource: eventData.PaymentSource,
                    tenantId: eventData.TenantId
                );

                _logger.LogDebug(
                    "Audit entry created for invoice payment: {InvoiceNumber}",
                    eventData.InvoiceNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to create audit entry for invoice payment: {InvoiceId}",
                    eventData.InvoiceId);
                // Don't throw - audit failure shouldn't break payment
            }
        }
    }
}
