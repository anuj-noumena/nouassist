using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
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
        public MatchStatus MatchStatus { get; private set; }

        private readonly List<Guid> _matchedBatches = new();
        public IReadOnlyCollection<Guid> MatchedBatches => _matchedBatches.AsReadOnly();

        protected BankStatementLine() { }

        public BankStatementLine(
            Guid id,
            Guid statementId,
            DateTime transactionDate,
            string description,
            MonetaryValue amount,
            string referenceNumber)
            : base(id)
        {
            StatementId = statementId;
            TransactionDate = transactionDate;
            Description = description;
            Amount = amount;
            ReferenceNumber = referenceNumber;
            MatchStatus = MatchStatus.Unmatched;
        }

        public void Match(Guid batchId)
        {
            _matchedBatches.Add(batchId);
            MatchStatus = MatchStatus.Matched;
        }

        public void Unmatch()
        {
            _matchedBatches.Clear();
            MatchStatus = MatchStatus.Unmatched;
        }
    }
}
