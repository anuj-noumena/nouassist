using System;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.GeneralLedger.Aggregates
{
    public class LedgerAccount : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public string AccountNumber { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public AccountType Type { get; private set; }
        public MonetaryValue CurrentBalance { get; private set; }
        public Guid? ParentAccountId { get; private set; }
        public bool IsActive { get; private set; }
        public bool AllowsPosting { get; private set; }

        protected LedgerAccount() { } // For ORM

        public LedgerAccount(
            Guid id,
            Guid? tenantId,
            string accountNumber,
            string name,
            AccountType type,
            string currency,
            Guid? parentAccountId = null,
            string description = null)
            : base(id)
        {
            TenantId = tenantId;
            AccountNumber = Check.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber), 20);
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), 200);
            Description = description;
            Type = type;
            CurrentBalance = new MonetaryValue(0, currency);
            ParentAccountId = parentAccountId;
            IsActive = true;
            AllowsPosting = true;
        }

        public void UpdateBalance(MonetaryValue amount, DebitCreditIndicator indicator)
        {
            if (!IsActive)
                throw new BusinessException("GeneralLedger:CannotUpdateInactiveAccount", 
                    "Cannot update balance of inactive account");

            var previousBalance = CurrentBalance;

            CurrentBalance = CalculateNewBalance(CurrentBalance, amount, indicator);

            AddDistributedEvent(new GeneralLedger.Events.AccountBalanceUpdatedEto
            {
                IdempotencyKey = Guid.NewGuid().ToString(),
                AccountId = Id,
                PreviousBalance = previousBalance,
                NewBalance = CurrentBalance,
                ChangeAmount = amount,
                ChangeType = indicator,
                TenantId = TenantId
            });
        }

        public void Deactivate()
        {
            if (CurrentBalance.Amount != 0)
                throw new BusinessException("GeneralLedger:CannotDeactivateWithBalance",
                    "Cannot deactivate account with non-zero balance");

            IsActive = false;
        }

        public void Reactivate()
        {
            IsActive = true;
        }

        public void DisablePosting()
        {
            AllowsPosting = false;
        }

        public void EnablePosting()
        {
            AllowsPosting = true;
        }

        public void UpdateName(string newName)
        {
            Name = Check.NotNullOrWhiteSpace(newName, nameof(newName), 200);
        }

        public void UpdateDescription(string newDescription)
        {
            Description = newDescription;
        }

        private MonetaryValue CalculateNewBalance(
            MonetaryValue currentBalance, 
            MonetaryValue amount, 
            DebitCreditIndicator indicator)
        {
            // For Asset and Expense accounts: Debit increases, Credit decreases
            // For Liability, Equity, and Revenue: Credit increases, Debit decreases
            var isNormalDebit = Type == AccountType.Asset || Type == AccountType.Expense;

            if (indicator == DebitCreditIndicator.Debit)
            {
                return isNormalDebit 
                    ? currentBalance.Add(amount) 
                    : currentBalance.Subtract(amount);
            }
            else
            {
                return isNormalDebit 
                    ? currentBalance.Subtract(amount) 
                    : currentBalance.Add(amount);
            }
        }
    }
}
