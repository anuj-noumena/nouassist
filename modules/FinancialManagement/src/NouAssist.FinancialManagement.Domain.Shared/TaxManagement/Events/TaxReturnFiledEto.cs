using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.TaxManagement.Events
{
    [EventName("NouAssist.TaxManagement.TaxReturnFiled")]
    public class TaxReturnFiledEto : IMultiTenant
    {
        public Guid TaxReturnId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public int TaxYear { get; set; }
        public DateTime FiledDate { get; set; }
        public decimal TotalTaxLiability { get; set; }
        public decimal RefundAmount { get; set; }
        public string ConfirmationNumber { get; set; }
    }
}
