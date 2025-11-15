using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.DebtManagement.Domain.Shared.Events
{
    [EventName("NouAssist.DebtManagement.DebtPaymentMade")]
    public class DebtPaymentMadeEto : IMultiTenant
    {
        public Guid PaymentId { get; set; }
        public Guid DebtAccountId { get; set; }
        public Guid? TenantId { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal RemainingBalance { get; set; }
    }
}
