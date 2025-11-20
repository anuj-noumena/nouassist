using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.AccountsPayable.Events;
using NouAssist.FinancialManagement.AuditTrail.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.AuditTrail.Handlers
{
    /// <summary>
    /// Handles BillPaidEto event for audit trail
    /// Creates comprehensive audit entry for compliance
    /// </summary>
    public class BillPaidAuditHandler :
        IDistributedEventHandler<BillPaidEto>,
        ITransientDependency
    {
        private readonly FinancialAuditService _auditService;
        private readonly ILogger<BillPaidAuditHandler> _logger;

        public BillPaidAuditHandler(
            FinancialAuditService auditService,
            ILogger<BillPaidAuditHandler> logger)
        {
            _auditService = auditService;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(BillPaidEto eventData)
        {
            try
            {
                await _auditService.AuditBillPaymentAsync(
                    billId: eventData.BillId,
                    billNumber: eventData.BillNumber,
                    amount: eventData.Amount.Amount,
                    currency: eventData.Amount.Currency,
                    vendorId: eventData.VendorId,
                    vendorName: eventData.VendorName,
                    paymentMethod: eventData.PaymentMethod,
                    paidBy: eventData.PaidBy,
                    tenantId: eventData.TenantId
                );

                _logger.LogDebug(
                    "Audit entry created for bill payment: {BillNumber}",
                    eventData.BillNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to create audit entry for bill payment: {BillId}",
                    eventData.BillId);
                // Don't throw - audit failure shouldn't break payment
            }
        }
    }
}
