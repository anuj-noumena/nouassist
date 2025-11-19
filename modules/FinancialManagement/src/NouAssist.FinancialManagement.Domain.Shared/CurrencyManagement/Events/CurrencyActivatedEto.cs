using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.CurrencyManagement.Events
{
    [EventName("NouAssist.CurrencyManagement.CurrencyActivated")]
    public class CurrencyActivatedEto : IMultiTenant
    {
        public Guid CurrencyId { get; set; }
        public Guid? TenantId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public DateTime ActivatedAt { get; set; }
    }
}
