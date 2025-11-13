using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.GeneralLedger.Dtos
{
    public class BankStatementDto : FullAuditedEntityDto<Guid>
    {
        public Guid BankAccountId { get; set; }
        public DateTime StatementDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public MonetaryValue OpeningBalance { get; set; }
        public MonetaryValue ClosingBalance { get; set; }
        public ReconciliationStatus Status { get; set; }
        public List<BankStatementLineDto> Lines { get; set; }
        public int UnmatchedCount { get; set; }
    }

    public class BankStatementLineDto
    {
        public Guid Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public MonetaryValue Amount { get; set; }
        public string ReferenceNumber { get; set; }
        public MatchStatus MatchStatus { get; set; }
        public List<Guid> MatchedBatches { get; set; }
    }

    public class ImportBankStatementDto
    {
        public Guid BankAccountId { get; set; }
        public DateTime StatementDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public string Currency { get; set; }
        public List<ImportBankStatementLineDto> Lines { get; set; }
    }

    public class ImportBankStatementLineDto
    {
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string ReferenceNumber { get; set; }
    }

    public class MatchBankStatementLineDto
    {
        public Guid LineId { get; set; }
        public Guid JournalEntryBatchId { get; set; }
    }
}
