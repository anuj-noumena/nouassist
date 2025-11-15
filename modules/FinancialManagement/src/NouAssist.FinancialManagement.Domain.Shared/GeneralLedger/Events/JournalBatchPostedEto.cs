using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.GeneralLedger.Events
{
    [EventName("NouAssist.GL.JournalBatchPosted")]
    public class JournalBatchPostedEto : IMultiTenant
    {
        public string IdempotencyKey { get; set; }
        public Guid BatchId { get; set; }
        public Guid? TenantId { get; set; }
        public string BatchNumber { get; set; }
        public DateTime PostingDate { get; set; }
        public List<JournalEntryLinePostedEto> Lines { get; set; }
        public MonetaryValue TotalAmount { get; set; }
        public Guid PostedBy { get; set; }
        public DateTime PostedAt { get; set; }
    }

    public class JournalEntryLinePostedEto
    {
        public Guid AccountId { get; set; }
        public string AccountNumber { get; set; }
        public MonetaryValue? DebitAmount { get; set; }
        public MonetaryValue? CreditAmount { get; set; }
        public string Memo { get; set; }
    }
}
