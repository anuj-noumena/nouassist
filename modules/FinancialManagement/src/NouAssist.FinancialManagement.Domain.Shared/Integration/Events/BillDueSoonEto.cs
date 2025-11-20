using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.Integration.Events
{
    [EventName("NouAssist.Finance.BillDueSoon")]
    public class BillDueSoonEto : IMultiTenant
    {
        public Guid BillId { get; set; }
        public Guid? TenantId { get; set; }
        public string BillNumber { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public MonetaryValue Amount { get; set; }
        public DateTime DueDate { get; set; }
        public int DaysUntilDue { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? ProjectId { get; set; }
        public DateTime AlertTime { get; set; }
    }
}
