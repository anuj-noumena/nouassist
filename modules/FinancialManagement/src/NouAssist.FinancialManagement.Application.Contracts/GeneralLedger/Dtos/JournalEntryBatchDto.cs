using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.GeneralLedger.Dtos
{
    public class JournalEntryBatchDto : FullAuditedEntityDto<Guid>
    {
        public string BatchNumber { get; set; }
        public DateTime PostingDate { get; set; }
        public string Description { get; set; }
        public BatchStatus Status { get; set; }
        public string Reference { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public bool IsBalanced { get; set; }
        public List<JournalEntryLineDto> Lines { get; set; }
    }

    public class JournalEntryLineDto
    {
        public Guid AccountId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public decimal? DebitAmount { get; set; }
        public decimal? CreditAmount { get; set; }
        public string Currency { get; set; }
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
        public string Currency { get; set; } = "USD";
        public string Memo { get; set; }
        public string Reference { get; set; }
    }

    public class GetJournalEntryBatchListDto : PagedAndSortedResultRequestDto
    {
        public BatchStatus? StatusFilter { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? AccountFilter { get; set; }
    }

    public class PostJournalEntryBatchDto
    {
        public Guid BatchId { get; set; }
    }

    public class ReverseJournalEntryBatchDto
    {
        public Guid BatchId { get; set; }
        public string ReversalReason { get; set; }
        public DateTime ReversalDate { get; set; }
    }
}
