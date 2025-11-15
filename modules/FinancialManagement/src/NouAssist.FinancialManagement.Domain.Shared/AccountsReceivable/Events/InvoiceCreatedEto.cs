using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.AccountsReceivable.Events
{
    [EventName("NouAssist.AR.InvoiceCreated")]
    public class InvoiceCreatedEto : IMultiTenant
    {
        public string IdempotencyKey { get; set; }
        public Guid InvoiceId { get; set; }
        public Guid? TenantId { get; set; }
        public string InvoiceNumber { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public MonetaryValue TotalAmount { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
