using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.FinancialManagement.GeneralLedger.Entities
{
    public class JournalEntryLine : Entity
    {
        public Guid BatchId { get; private set; }
        public Guid AccountId { get; private set; }
        public AccountNumber AccountNumber { get; private set; }
        public MonetaryValue DebitAmount { get; private set; }
        public MonetaryValue CreditAmount { get; private set; }
        public string Memo { get; private set; }
        public string Reference { get; private set; }
        public Guid? SourceEntityId { get; private set; }
        public EntityType? SourceEntityType { get; private set; }

        protected JournalEntryLine() { }

        public JournalEntryLine(
            Guid batchId,
            Guid accountId,
            MonetaryValue debitAmount,
            MonetaryValue creditAmount,
            string memo,
            string reference = null,
            Guid? sourceEntityId = null,
            EntityType? sourceEntityType = null)
        {
            BatchId = batchId;
            AccountId = accountId;
            DebitAmount = debitAmount;
            CreditAmount = creditAmount;
            Memo = Check.NotNullOrWhiteSpace(memo, nameof(memo));
            Reference = reference;
            SourceEntityId = sourceEntityId;
            SourceEntityType = sourceEntityType;

            ValidateAmounts();
        }

        private void ValidateAmounts()
        {
            if (DebitAmount == null && CreditAmount == null)
                throw new BusinessException("GL:NoAmount", 
                    "Line must have either debit or credit amount");

            if (DebitAmount != null && CreditAmount != null)
                throw new BusinessException("GL:BothAmounts", 
                    "Line cannot have both debit and credit amounts");
        }

        public override object[] GetKeys()
        {
            return new object[] { BatchId, AccountId, Memo };
        }
    }
}
