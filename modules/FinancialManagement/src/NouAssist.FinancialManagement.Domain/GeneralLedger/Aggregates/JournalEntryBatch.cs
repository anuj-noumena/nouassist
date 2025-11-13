using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.GeneralLedger.Entities;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using NouAssist.FinancialManagement.GeneralLedger.Events;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.GeneralLedger.Aggregates
{
    public class JournalEntryBatch : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public string BatchNumber { get; private set; }
        public DateTime PostingDate { get; private set; }
        public string Description { get; private set; }
        public string Reference { get; private set; }
        public BatchStatus Status { get; private set; }
        public Guid? SourceEntityId { get; private set; }
        public string SourceEntityType { get; private set; }

        private readonly List<JournalEntryLine> _lines = new();
        public IReadOnlyCollection<JournalEntryLine> Lines => _lines.AsReadOnly();

        public bool IsBalanced => CalculateBalance();

        protected JournalEntryBatch() { } // For ORM

        public JournalEntryBatch(
            Guid id,
            Guid? tenantId,
            string batchNumber,
            DateTime postingDate,
            string description,
            string reference = null,
            Guid? sourceEntityId = null,
            string sourceEntityType = null)
            : base(id)
        {
            TenantId = tenantId;
            BatchNumber = Check.NotNullOrWhiteSpace(batchNumber, nameof(batchNumber), 50);
            PostingDate = postingDate;
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), 500);
            Reference = reference;
            Status = BatchStatus.Draft;
            SourceEntityId = sourceEntityId;
            SourceEntityType = sourceEntityType;
        }

        public void AddLine(JournalEntryLine line)
        {
            if (Status != BatchStatus.Draft)
                throw new BusinessException("GeneralLedger:CannotModifyPostedBatch",
                    "Cannot add lines to posted or reversed batch");

            _lines.Add(line);
        }

        public void Post(Guid postedBy)
        {
            if (Status != BatchStatus.Draft)
                throw new BusinessException("GeneralLedger:BatchAlreadyPosted",
                    "Batch has already been posted");

            if (_lines.Count < 2)
                throw new BusinessException("GeneralLedger:InsufficientLines",
                    "Journal entry batch must have at least 2 lines");

            if (!IsBalanced)
                throw new BusinessException("GeneralLedger:BatchNotBalanced",
                    "Journal entry batch is not balanced (debits must equal credits)");

            Status = BatchStatus.Posted;

            var lineEvents = _lines.Select(l => new JournalEntryLinePostedDto
            {
                AccountId = l.AccountId,
                Amount = l.DebitAmount ?? l.CreditAmount,
                DebitCredit = l.DebitAmount != null ? "Debit" : "Credit",
                Memo = l.Memo
            }).ToList();

            AddDistributedEvent(new JournalBatchPostedEto
            {
                IdempotencyKey = Guid.NewGuid().ToString(),
                BatchId = Id,
                BatchNumber = BatchNumber,
                PostingDate = PostingDate,
                Lines = lineEvents,
                TotalAmount = CalculateTotalAmount(),
                PostedBy = postedBy,
                TenantId = TenantId
            });
        }

        public JournalEntryBatch Reverse(Guid reversedBy, DateTime reversalDate, string reason)
        {
            if (Status != BatchStatus.Posted)
                throw new BusinessException("GeneralLedger:CannotReverseUnposted",
                    "Only posted batches can be reversed");

            Status = BatchStatus.Reversed;

            var reversalBatch = new JournalEntryBatch(
                Guid.NewGuid(),
                TenantId,
                $"{BatchNumber}-REV",
                reversalDate,
                $"Reversal: {reason}",
                BatchNumber,
                Id,
                "JournalEntryBatch");

            foreach (var line in _lines)
            {
                var reversalLine = new JournalEntryLine(
                    reversalBatch.Id,
                    line.AccountId,
                    line.CreditAmount, // Swap debit and credit
                    line.DebitAmount,
                    $"Reversal: {line.Memo}",
                    line.Reference);

                reversalBatch.AddLine(reversalLine);
            }

            return reversalBatch;
        }

        private bool CalculateBalance()
        {
            var currencyGroups = _lines.GroupBy(l => 
                (l.DebitAmount ?? l.CreditAmount).Currency);

            foreach (var group in currencyGroups)
            {
                var totalDebits = group
                    .Where(l => l.DebitAmount != null)
                    .Sum(l => l.DebitAmount.Amount);

                var totalCredits = group
                    .Where(l => l.CreditAmount != null)
                    .Sum(l => l.CreditAmount.Amount);

                var difference = Math.Abs(totalDebits - totalCredits);
                if (difference > 0.01m) // Allow for small rounding differences
                    return false;
            }

            return true;
        }

        private MonetaryValue CalculateTotalAmount()
        {
            if (!_lines.Any()) 
                return new MonetaryValue(0, "USD");

            var firstLine = _lines.First();
            var currency = (firstLine.DebitAmount ?? firstLine.CreditAmount).Currency;

            var total = _lines
                .Where(l => l.DebitAmount != null)
                .Sum(l => l.DebitAmount.Amount);

            return new MonetaryValue(total, currency);
        }
    }
}
