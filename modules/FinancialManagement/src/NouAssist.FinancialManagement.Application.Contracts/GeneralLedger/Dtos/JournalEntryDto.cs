using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.GeneralLedger.Dtos
{
    public class JournalEntryBatchDto : FullAuditedEntityDto<Guid>
    {
        public string BatchNumber { get; set; }
        public DateTime PostingDate { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public BatchStatus Status { get; set; }
        public List<JournalEntryLineDto> Lines { get; set; }
        public bool IsBalanced { get; set; }
    }

    public class JournalEntryLineDto
    {
        public Guid AccountId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public MonetaryValue DebitAmount { get; set; }
        public MonetaryValue CreditAmount { get; set; }
        public string Memo { get; set; }
        public string Reference { get; set; }
    }

    public class CreateJournalEntryBatchDto
    {
        public DateTime PostingDate { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public List<CreateJournalEntryLineDto> Lines { get; set; }
    }

    public class CreateJournalEntryLineDto
    {
        public Guid AccountId { get; set; }
        public decimal? DebitAmount { get; set; }
        public decimal? CreditAmount { get; set; }
        public string Currency { get; set; }
        public string Memo { get; set; }
        public string Reference { get; set; }
    }

    public class GetJournalEntryListDto : PagedAndSortedResultRequestDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? AccountId { get; set; }
        public BatchStatus? Status { get; set; }
    }
}
