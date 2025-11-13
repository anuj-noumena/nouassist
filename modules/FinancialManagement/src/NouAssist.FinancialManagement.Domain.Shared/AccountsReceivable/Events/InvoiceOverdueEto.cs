using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.AccountsReceivable.Events
{
    [EventName("FinancialManagement.AccountsReceivable.InvoiceOverdue")]
    public class InvoiceOverdueEto : IMultiTenant
    {
        public string IdempotencyKey { get; set; }
        public Guid InvoiceId { get; set; }
        public Guid CustomerId { get; set; }
        public MonetaryValue OverdueAmount { get; set; }
        public int DaysOverdue { get; set; }
        public DateTime OriginalDueDate { get; set; }
        public TimeZoneInfo TimeZone { get; set; }
        public Guid? TenantId { get; set; }
    }
}
