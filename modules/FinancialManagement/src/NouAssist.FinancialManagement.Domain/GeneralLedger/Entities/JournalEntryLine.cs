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
        public MonetaryValue DebitAmount { get; private set; }
        public MonetaryValue CreditAmount { get; private set; }
        public string Memo { get; private set; }
        public string Reference { get; private set; }
        public Guid? SourceEntityId { get; private set; }
        public string SourceEntityType { get; private set; }

        protected JournalEntryLine() { } // For ORM

        public JournalEntryLine(
            Guid batchId,
            Guid accountId,
            MonetaryValue debitAmount,
            MonetaryValue creditAmount,
            string memo,
            string reference = null,
            Guid? sourceEntityId = null,
            string sourceEntityType = null)
        {
            BatchId = batchId;
            AccountId = accountId;

            if (debitAmount != null && creditAmount != null)
                throw new BusinessException("GeneralLedger:BothDebitAndCredit",
                    "A journal line cannot have both debit and credit amounts");

            if (debitAmount == null && creditAmount == null)
                throw new BusinessException("GeneralLedger:NoDebitOrCredit",
                    "A journal line must have either a debit or credit amount");

            if (debitAmount?.IsNegative == true || creditAmount?.IsNegative == true)
                throw new BusinessException("GeneralLedger:NegativeAmount",
                    "Journal line amounts must be positive");

            DebitAmount = debitAmount;
            CreditAmount = creditAmount;
            Memo = Check.NotNullOrWhiteSpace(memo, nameof(memo), 500);
            Reference = reference;
            SourceEntityId = sourceEntityId;
            SourceEntityType = sourceEntityType;
        }

        public override object[] GetKeys()
        {
            return new object[] { BatchId, AccountId, Memo };
        }
    }
}
