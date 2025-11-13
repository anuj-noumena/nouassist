using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.AccountsReceivable.Events
{
    [EventName("FinancialManagement.AccountsReceivable.PaymentReceived")]
    public class PaymentReceivedEto : IMultiTenant
    {
        public string IdempotencyKey { get; set; }
        public Guid PaymentId { get; set; }
        public Guid CustomerId { get; set; }
        public MonetaryValue Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentSource { get; set; }
        public string Reference { get; set; }
        public Guid RecordedBy { get; set; }
        public Guid? TenantId { get; set; }
    }
}
