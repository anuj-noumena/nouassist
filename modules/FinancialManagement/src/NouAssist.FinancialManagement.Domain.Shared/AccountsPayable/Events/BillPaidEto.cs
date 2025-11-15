using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.AccountsPayable.Events
{
    [EventName("NouAssist.AP.BillPaid")]
    public class BillPaidEto : IMultiTenant
    {
        public string IdempotencyKey { get; set; }
        public Guid BillId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid PaymentId { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public MonetaryValue Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string ConfirmationNumber { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? ProjectId { get; set; }
    }
}
