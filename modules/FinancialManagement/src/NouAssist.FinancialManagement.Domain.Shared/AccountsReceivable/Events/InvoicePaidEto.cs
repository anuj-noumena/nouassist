using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.AccountsReceivable.Events
{
    [EventName("NouAssist.Finance.InvoicePaid")]
    public class InvoicePaidEto : IMultiTenant
    {
        public string IdempotencyKey { get; set; }
        public Guid InvoiceId { get; set; }
        public Guid? TenantId { get; set; }
        public string InvoiceNumber { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public MonetaryValue PaymentAmount { get; set; }
        public MonetaryValue TotalAmount { get; set; }
        public MonetaryValue RemainingBalance { get; set; }
        public DateTime PaymentDate { get; set; }
        public Guid DepositAccountId { get; set; } // Bank account where payment deposited
        public Guid RevenueAccountId { get; set; } // Revenue account to credit
        public Guid? ProjectId { get; set; }
        public string PaymentSource { get; set; } // Check, EFT, Card, Cash
        public string PaymentReference { get; set; }
        public bool IsFullPayment { get; set; }
    }
}
