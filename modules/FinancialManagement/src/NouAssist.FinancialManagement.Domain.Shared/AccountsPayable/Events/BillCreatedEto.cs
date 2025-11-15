using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.AccountsPayable.Events
{
    [EventName("NouAssist.AP.BillCreated")]
    public class BillCreatedEto : IMultiTenant
    {
        public string IdempotencyKey { get; set; }
        public Guid BillId { get; set; }
        public Guid? TenantId { get; set; }
        public string BillNumber { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public MonetaryValue Amount { get; set; }
        public DateTime DueDate { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? ProjectId { get; set; }
        public bool RequiresApproval { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
