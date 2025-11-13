using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.AccountsReceivable.Events
{
    [EventName("FinancialManagement.AccountsReceivable.InvoicePaymentApplied")]
    public class InvoicePaymentAppliedEto : IMultiTenant
    {
        public string IdempotencyKey { get; set; }
        public Guid InvoiceId { get; set; }
        public Guid PaymentId { get; set; }
        public MonetaryValue AppliedAmount { get; set; }
        public MonetaryValue RemainingBalance { get; set; }
        public bool IsFullyPaid { get; set; }
        public Guid AppliedBy { get; set; }
        public Guid? TenantId { get; set; }
    }
}
