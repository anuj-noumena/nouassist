using System;
using NouAssist.FinancialManagement.Shared.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.GeneralLedger.Events
{
    [EventName("NouAssist.GL.AccountBalanceUpdated")]
    public class AccountBalanceUpdatedEto : IMultiTenant
    {
        public string IdempotencyKey { get; set; }
        public Guid AccountId { get; set; }
        public Guid? TenantId { get; set; }
        public MonetaryValue PreviousBalance { get; set; }
        public MonetaryValue NewBalance { get; set; }
        public MonetaryValue ChangeAmount { get; set; }
        public DebitCreditIndicator ChangeType { get; set; }
        public Guid? CausingBatchId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
