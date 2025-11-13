using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.Shared.ValueObjects;

namespace NouAssist.FinancialManagement.GeneralLedger.Dtos
{
    public class BankReconciliationReportDto
    {
        public Guid StatementId { get; set; }
        public DateTime StatementDate { get; set; }
        public MonetaryValue StatementOpeningBalance { get; set; }
        public MonetaryValue StatementClosingBalance { get; set; }
        public MonetaryValue BookBalance { get; set; }
        public MonetaryValue ReconciledBalance { get; set; }
        public MonetaryValue Variance { get; set; }
        public bool IsReconciled { get; set; }
        
        public List<UnmatchedTransactionDto> UnmatchedBankTransactions { get; set; }
        public List<UnmatchedTransactionDto> UnmatchedBookTransactions { get; set; }
        
        public int TotalTransactions { get; set; }
        public int MatchedTransactions { get; set; }
        public decimal ReconciliationPercentage { get; set; }
    }

    public class UnmatchedTransactionDto
    {
        public Guid Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public MonetaryValue Amount { get; set; }
        public string Reference { get; set; }
        public string Type { get; set; } // "Bank" or "Book"
    }

    public class BulkMatchDto
    {
        public List<MatchPairDto> Matches { get; set; }
    }

    public class MatchPairDto
    {
        public Guid BankLineId { get; set; }
        public Guid JournalEntryBatchId { get; set; }
    }

    public class AutoMatchSettingsDto
    {
        public decimal AmountTolerancePercentage { get; set; } = 0.01m; // 1% tolerance
        public int DateToleranceDays { get; set; } = 3;
        public bool MatchByReference { get; set; } = true;
        public bool MatchByDescription { get; set; } = true;
        public decimal MinimumConfidenceScore { get; set; } = 0.8m;
    }

    public class SuggestedMatchDto
    {
        public Guid BankLineId { get; set; }
        public Guid JournalEntryBatchId { get; set; }
        public decimal ConfidenceScore { get; set; }
        public List<string> MatchReasons { get; set; }
        public BankStatementLineDto BankLine { get; set; }
        public JournalEntryBatchDto JournalEntry { get; set; }
    }
}
