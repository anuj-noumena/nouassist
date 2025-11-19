using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.FinancialManagement.GeneralLedger.Entities
{
    public class BankStatementLine : Entity<Guid>
    {
        public Guid StatementId { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public string Description { get; private set; }
        public MonetaryValue Amount { get; private set; }
        public string ReferenceNumber { get; private set; }
        public bool IsMatched { get; private set; }
        public Guid? MatchedJournalBatchId { get; private set; }
        public Guid? MatchedBy { get; private set; }
        public DateTime? MatchedDate { get; private set; }

        protected BankStatementLine() { }

        public BankStatementLine(
            Guid id,
            Guid statementId,
            DateTime transactionDate,
            string description,
            MonetaryValue amount,
            string referenceNumber) : base(id)
        {
            StatementId = statementId;
            TransactionDate = transactionDate;
            Description = Check.NotNullOrWhiteSpace(description, nameof(description));
            Amount = Check.NotNull(amount, nameof(amount));
            ReferenceNumber = referenceNumber;
            IsMatched = false;
        }

        public void Match(Guid journalBatchId, Guid matchedBy)
        {
            if (IsMatched)
            {
                throw new BusinessException("GL:AlreadyMatched", 
                    "Statement line is already matched");
            }

            IsMatched = true;
            MatchedJournalBatchId = journalBatchId;
            MatchedBy = matchedBy;
            MatchedDate = DateTime.UtcNow;
        }

        public void Unmatch()
        {
            if (!IsMatched)
            {
                throw new BusinessException("GL:NotMatched", 
                    "Statement line is not matched");
            }

            IsMatched = false;
            MatchedJournalBatchId = null;
            MatchedBy = null;
            MatchedDate = null;
        }
    }
}
