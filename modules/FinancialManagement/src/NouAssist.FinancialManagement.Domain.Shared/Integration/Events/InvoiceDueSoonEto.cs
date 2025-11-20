using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.Integration.Events
{
    [EventName("NouAssist.Finance.InvoiceDueSoon")]
    public class InvoiceDueSoonEto : IMultiTenant
    {
        public Guid InvoiceId { get; set; }
        public Guid? TenantId { get; set; }
        public string InvoiceNumber { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public MonetaryValue TotalAmount { get; set; }
        public MonetaryValue AmountDue { get; set; }
        public DateTime DueDate { get; set; }
        public int DaysUntilDue { get; set; }
        public Guid? ProjectId { get; set; }
        public DateTime AlertTime { get; set; }
    }
}
