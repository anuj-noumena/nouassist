using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.GeneralLedger.Events
{
    [EventName("FinancialManagement.GeneralLedger.JournalBatchPosted")]
    public class JournalBatchPostedEto : IMultiTenant
    {
        public string IdempotencyKey { get; set; }
        public Guid BatchId { get; set; }
        public string BatchNumber { get; set; }
        public DateTime PostingDate { get; set; }
        public List<JournalEntryLinePostedDto> Lines { get; set; }
        public MonetaryValue TotalAmount { get; set; }
        public Guid PostedBy { get; set; }
        public Guid? TenantId { get; set; }
    }

    public class JournalEntryLinePostedDto
    {
        public Guid AccountId { get; set; }
        public MonetaryValue Amount { get; set; }
        public string DebitCredit { get; set; }
        public string Memo { get; set; }
    }
}
