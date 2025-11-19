using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.DebtManagement.Events
{
    [EventName("NouAssist.DebtManagement.DebtAccountCreated")]
    public class DebtAccountCreatedEto : IMultiTenant
    {
        public Guid DebtAccountId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public string DebtName { get; set; }
        public string DebtType { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
