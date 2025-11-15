using System;
using System.Collections.Generic;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.GeneralLedger.Events
{
    [EventName("NouAssist.GL.AccountingPeriodClosed")]
    public class AccountingPeriodClosedEto : IMultiTenant
    {
        public string IdempotencyKey { get; set; }
        public Guid PeriodId { get; set; }
        public Guid? TenantId { get; set; }
        public int FiscalYear { get; set; }
        public int PeriodNumber { get; set; }
        public DateTime ClosedDate { get; set; }
        public List<Guid> ClosingEntries { get; set; }
        public Guid ClosedBy { get; set; }
    }
}
