using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.Integration.Events
{
    [EventName("NouAssist.Finance.InvoiceOverdue")]
    public class InvoiceOverdueEto : IMultiTenant
    {
        public Guid InvoiceId { get; set; }
        public Guid? TenantId { get; set; }
        public string InvoiceNumber { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public MonetaryValue TotalAmount { get; set; }
        public MonetaryValue AmountDue { get; set; }
        public DateTime DueDate { get; set; }
        public int DaysOverdue { get; set; }
        public Guid? ProjectId { get; set; }
        public DateTime? LastContactDate { get; set; }
        public DateTime OverdueDetectedAt { get; set; }
    }
}
