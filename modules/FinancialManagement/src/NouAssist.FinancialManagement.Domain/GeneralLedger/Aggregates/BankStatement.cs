using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.GeneralLedger.Entities;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.GeneralLedger.Aggregates
{
    public class BankStatement : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public Guid BankAccountId { get; private set; }
        public DateTime StatementDate { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public MonetaryValue OpeningBalance { get; private set; }
        public MonetaryValue ClosingBalance { get; private set; }
        public ReconciliationStatus Status { get; private set; }

        private readonly List<BankStatementLine> _lines = new();
        public IReadOnlyCollection<BankStatementLine> Lines => _lines.AsReadOnly();

        protected BankStatement() { }

        public BankStatement(
            Guid id,
            Guid? tenantId,
            Guid bankAccountId,
            DateTime statementDate,
            DateTime startDate,
            DateTime endDate,
            MonetaryValue openingBalance,
            MonetaryValue closingBalance) : base(id)
        {
            TenantId = tenantId;
            BankAccountId = bankAccountId;
            StatementDate = statementDate;
            StartDate = startDate;
            EndDate = endDate;
            OpeningBalance = Check.NotNull(openingBalance, nameof(openingBalance));
            ClosingBalance = Check.NotNull(closingBalance, nameof(closingBalance));
            Status = ReconciliationStatus.Unreconciled;
        }

        public void AddLine(BankStatementLine line)
        {
            Check.NotNull(line, nameof(line));
            _lines.Add(line);
        }

        public void MatchLine(Guid lineId, Guid journalBatchId, Guid matchedBy)
        {
            var line = _lines.FirstOrDefault(l => l.Id == lineId);
            if (line == null)
            {
                throw new BusinessException("GL:LineNotFound", 
                    "Statement line not found");
            }

            line.Match(journalBatchId, matchedBy);
            UpdateReconciliationStatus();
        }

        public void UnmatchLine(Guid lineId)
        {
            var line = _lines.FirstOrDefault(l => l.Id == lineId);
            if (line == null)
            {
                throw new BusinessException("GL:LineNotFound", 
                    "Statement line not found");
            }

            line.Unmatch();
            UpdateReconciliationStatus();
        }

        public MonetaryValue GetUnreconciledAmount()
        {
            var unreconciledLines = _lines.Where(l => !l.IsMatched);
            var total = unreconciledLines
                .Aggregate(0m, (sum, line) => sum + line.Amount.Amount);
            
            return new MonetaryValue(total, ClosingBalance.Currency);
        }

        private void UpdateReconciliationStatus()
        {
            var matchedCount = _lines.Count(l => l.IsMatched);
            var totalCount = _lines.Count;

            if (matchedCount == 0)
            {
                Status = ReconciliationStatus.Unreconciled;
            }
            else if (matchedCount == totalCount)
            {
                Status = ReconciliationStatus.FullyReconciled;
            }
            else
            {
                Status = ReconciliationStatus.PartiallyReconciled;
            }
        }
    }
}
