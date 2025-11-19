using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.GeneralLedger.Entities;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using NouAssist.FinancialManagement.GeneralLedger.Events;
using NouAssist.FinancialManagement.Shared.Enums;
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
        public TransactionDate PostingDate { get; private set; }
        public string Description { get; private set; }
        public BatchStatus Status { get; private set; }
        public string Reference { get; private set; }
        public Guid? SourceEntityId { get; private set; }
        public EntityType? SourceEntityType { get; private set; }

        private readonly List<JournalEntryLine> _lines = new();
        public IReadOnlyCollection<JournalEntryLine> Lines => _lines.AsReadOnly();

        protected JournalEntryBatch() { } // EF Core

        public JournalEntryBatch(
            Guid id,
            Guid? tenantId,
            string batchNumber,
            TransactionDate postingDate,
            string description,
            Guid? sourceEntityId = null,
            EntityType? sourceEntityType = null,
            string reference = null) : base(id)
        {
            TenantId = tenantId;
            BatchNumber = Check.NotNullOrWhiteSpace(batchNumber, nameof(batchNumber));
            PostingDate = Check.NotNull(postingDate, nameof(postingDate));
            Description = Check.NotNullOrWhiteSpace(description, nameof(description));
            Status = BatchStatus.Draft;
            SourceEntityId = sourceEntityId;
            SourceEntityType = sourceEntityType;
            Reference = reference;
        }

        public void AddLine(JournalEntryLine line)
        {
            Check.NotNull(line, nameof(line));

            if (Status != BatchStatus.Draft)
            {
                throw new BusinessException("GL:CannotModifyPostedBatch", 
                    "Cannot add lines to a posted or reversed batch");
            }

            _lines.Add(line);
        }

        public void Post(Guid postedBy)
        {
            if (Status != BatchStatus.Draft)
            {
                throw new BusinessException("GL:BatchAlreadyPosted", 
                    "Batch has already been posted");
            }

            ValidateForPosting();

            Status = BatchStatus.Posted;
            
            var totalAmount = CalculateTotalAmount();

            AddDistributedEvent(new JournalBatchPostedEto
            {
                IdempotencyKey = Guid.NewGuid().ToString(),
                BatchId = Id,
                TenantId = TenantId,
                BatchNumber = BatchNumber,
                PostingDate = PostingDate.Date,
                Lines = _lines.Select(l => new JournalEntryLinePostedEto
                {
                    AccountId = l.AccountId,
                    AccountNumber = l.AccountNumber?.Number,
                    DebitAmount = l.DebitAmount,
                    CreditAmount = l.CreditAmount,
                    Memo = l.Memo
                }).ToList(),
                TotalAmount = totalAmount,
                PostedBy = postedBy,
                PostedAt = DateTime.UtcNow
            });
        }

        public JournalEntryBatch Reverse(string reversalReason, TransactionDate reversalDate, Guid reversedBy)
        {
            if (Status != BatchStatus.Posted)
            {
                throw new BusinessException("GL:CannotReverseUnpostedBatch", 
                    "Only posted batches can be reversed");
            }

            Status = BatchStatus.Reversed;

            // Create reversal batch
            var reversalBatch = new JournalEntryBatch(
                Guid.NewGuid(),
                TenantId,
                $"{BatchNumber}-REV",
                reversalDate,
                $"Reversal: {Description} - {reversalReason}",
                Id,
                EntityType.JournalEntryBatch,
                $"Reversal of {BatchNumber}"
            );

            // Add reversed lines
            foreach (var line in _lines)
            {
                var reversedLine = new JournalEntryLine(
                    reversalBatch.Id,
                    line.AccountId,
                    line.DebitAmount != null ? null : line.CreditAmount,
                    line.DebitAmount != null ? line.DebitAmount : null,
                    $"Reversal: {line.Memo}",
                    line.Reference
                );
                reversalBatch.AddLine(reversedLine);
            }

            return reversalBatch;
        }

        public bool IsBalanced()
        {
            var debitTotal = _lines
                .Where(l => l.DebitAmount != null)
                .Sum(l => l.DebitAmount.Amount);
            
            var creditTotal = _lines
                .Where(l => l.CreditAmount != null)
                .Sum(l => l.CreditAmount.Amount);

            return Math.Abs(debitTotal - creditTotal) < 0.01m;
        }

        private void ValidateForPosting()
        {
            if (_lines.Count < 2)
            {
                throw new BusinessException("GL:InsufficientLines", 
                    "Journal entry must have at least 2 lines");
            }

            if (!IsBalanced())
            {
                throw new BusinessException("GL:BatchNotBalanced", 
                    "Journal entry batch is not balanced");
            }

            foreach (var line in _lines)
            {
                if (line.DebitAmount == null && line.CreditAmount == null)
                {
                    throw new BusinessException("GL:LineHasNoAmount", 
                        "Journal entry line must have either debit or credit amount");
                }

                if (line.DebitAmount != null && line.CreditAmount != null)
                {
                    throw new BusinessException("GL:LineBothDebitCredit", 
                        "Journal entry line cannot have both debit and credit amounts");
                }
            }
        }

        private MonetaryValue CalculateTotalAmount()
        {
            var firstLine = _lines.FirstOrDefault();
            if (firstLine == null)
            {
                return null;
            }

            var currency = firstLine.DebitAmount?.Currency ?? firstLine.CreditAmount?.Currency;
            var totalDebits = _lines
                .Where(l => l.DebitAmount != null)
                .Sum(l => l.DebitAmount.Amount);

            return new MonetaryValue(totalDebits, currency);
        }
    }
}
