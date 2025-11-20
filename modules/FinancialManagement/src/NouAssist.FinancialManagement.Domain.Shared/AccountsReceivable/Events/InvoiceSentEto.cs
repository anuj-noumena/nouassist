using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.AccountsReceivable.Events
{
    [EventName("NouAssist.Finance.InvoiceSent")]
    public class InvoiceSentEto : IMultiTenant
    {
        public string IdempotencyKey { get; set; }
        public Guid InvoiceId { get; set; }
        public Guid? TenantId { get; set; }
        public string InvoiceNumber { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public MonetaryValue TotalAmount { get; set; }
        public MonetaryValue TaxAmount { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public Guid RevenueAccountId { get; set; }
        public Guid ReceivableAccountId { get; set; }
        public Guid? ProjectId { get; set; }
    }
}
