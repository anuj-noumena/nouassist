using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.DebtManagement.Domain.Shared.Events
{
    [EventName("NouAssist.DebtManagement.DebtPaidOff")]
    public class DebtPaidOffEto : IMultiTenant
    {
        public Guid DebtAccountId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public string DebtName { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalInterestPaid { get; set; }
        public DateTime PaidOffDate { get; set; }
        public int TotalPayments { get; set; }
    }
}
