using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.GeneralLedger.Dtos
{
    public class BankStatementDto : FullAuditedEntityDto<Guid>
    {
        public Guid BankAccountId { get; set; }
        public DateTime StatementDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public string Currency { get; set; }
        public ReconciliationStatus Status { get; set; }
        public List<BankStatementLineDto> Lines { get; set; }
        public int TotalLines { get; set; }
        public int ReconciledLines { get; set; }
        public decimal ReconciliationPercentage { get; set; }
    }

    public class BankStatementLineDto
    {
        public Guid Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public decimal Amount { get; set; }
        public bool IsMatched { get; set; }
        public Guid? MatchedJournalBatchId { get; set; }
        public Guid? MatchedBy { get; set; }
        public DateTime? MatchedDate { get; set; }
    }

    public class CreateBankStatementDto
    {
        public Guid BankAccountId { get; set; }
        public DateTime StatementDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public string Currency { get; set; }
    }

    public class ImportBankStatementDto
    {
        public Guid BankAccountId { get; set; }
        public DateTime StatementDate { get; set; }
        public string FileContent { get; set; }
        public string FileFormat { get; set; } // CSV, OFX, QFX, etc.
    }

    public class MatchStatementLineDto
    {
        public Guid StatementId { get; set; }
        public Guid LineId { get; set; }
        public Guid JournalBatchId { get; set; }
    }

    public class GetBankStatementListDto : PagedAndSortedResultRequestDto
    {
        public Guid? BankAccountFilter { get; set; }
        public ReconciliationStatus? StatusFilter { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
