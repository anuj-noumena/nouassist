using System;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using NouAssist.FinancialManagement.GeneralLedger.Events;
using NouAssist.FinancialManagement.Shared.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.GeneralLedger.Aggregates
{
    public class LedgerAccount : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public AccountNumber AccountNumber { get; private set; }
        public string Name { get; private set; }
        public AccountType Type { get; private set; }
        public MonetaryValue CurrentBalance { get; private set; }
        public Guid? ParentAccountId { get; private set; }
        public bool IsActive { get; private set; }
        public bool AllowsPosting { get; private set; }
        public string Description { get; private set; }

        protected LedgerAccount() { } // EF Core

        public LedgerAccount(
            Guid id,
            Guid? tenantId,
            AccountNumber accountNumber,
            string name,
            AccountType type,
            string currency,
            Guid? parentAccountId = null,
            string description = null) : base(id)
        {
            TenantId = tenantId;
            AccountNumber = Check.NotNull(accountNumber, nameof(accountNumber));
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Type = type;
            CurrentBalance = MonetaryValue.Zero(currency);
            ParentAccountId = parentAccountId;
            IsActive = true;
            AllowsPosting = true;
            Description = description;

            AddLocalEvent(new AccountCreatedEvent
            {
                AccountId = Id,
                TenantId = TenantId,
                AccountNumber = AccountNumber,
                Name = Name,
                Type = Type
            });
        }

        public void UpdateBalance(MonetaryValue amount, DebitCreditIndicator indicator)
        {
            Check.NotNull(amount, nameof(amount));

            if (amount.Currency != CurrentBalance.Currency)
                throw new BusinessException("GL:CurrencyMismatch", 
                    $"Cannot update balance with different currency");

            var previousBalance = CurrentBalance;

            CurrentBalance = indicator == DebitCreditIndicator.Debit
                ? CurrentBalance.Add(amount)
                : CurrentBalance.Subtract(amount);

            AddLocalEvent(new AccountBalanceUpdatedEvent
            {
                AccountId = Id,
                TenantId = TenantId,
                PreviousBalance = previousBalance,
                NewBalance = CurrentBalance,
                ChangeAmount = amount,
                ChangeType = indicator
            });
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new BusinessException("GL:AccountAlreadyInactive", 
                    "Account is already inactive");

            IsActive = false;
        }

        public void Activate()
        {
            if (IsActive)
                throw new BusinessException("GL:AccountAlreadyActive", 
                    "Account is already active");

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
            Name = Check.NotNullOrWhiteSpace(newName, nameof(newName));
        }

        public void UpdateDescription(string newDescription)
        {
            Description = newDescription;
        }

        public void MoveTo(Guid? newParentAccountId)
        {
            if (newParentAccountId == Id)
                throw new BusinessException("GL:CannotMoveToSelf", 
                    "Cannot move account to itself");

            ParentAccountId = newParentAccountId;
        }
    }

    // Local events (not published outside aggregate)
    public class AccountCreatedEvent
    {
        public Guid AccountId { get; set; }
        public Guid? TenantId { get; set; }
        public AccountNumber AccountNumber { get; set; }
        public string Name { get; set; }
        public AccountType Type { get; set; }
    }

    public class AccountBalanceUpdatedEvent
    {
        public Guid AccountId { get; set; }
        public Guid? TenantId { get; set; }
        public MonetaryValue PreviousBalance { get; set; }
        public MonetaryValue NewBalance { get; set; }
        public MonetaryValue ChangeAmount { get; set; }
        public DebitCreditIndicator ChangeType { get; set; }
    }
}
