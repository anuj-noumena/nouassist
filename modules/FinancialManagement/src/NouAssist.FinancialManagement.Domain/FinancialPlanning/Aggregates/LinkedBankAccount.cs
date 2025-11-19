using System;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.FinancialPlanning.Aggregates
{
    public class LinkedBankAccount : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public Guid UserId { get; private set; }
        public string BankName { get; private set; }
        public string AccountNumber { get; private set; }
        public BankAccountType AccountType { get; private set; }
        public MonetaryValue CurrentBalance { get; private set; }
        public DateTime LastSyncDate { get; private set; }
        public bool IsActive { get; private set; }
        public ConnectionStatus ConnectionStatus { get; private set; }

        protected LinkedBankAccount() { }

        public LinkedBankAccount(
            Guid id,
            Guid? tenantId,
            Guid userId,
            string bankName,
            string accountNumber,
            BankAccountType accountType,
            string currency) : base(id)
        {
            TenantId = tenantId;
            UserId = userId;
            BankName = Check.NotNullOrWhiteSpace(bankName, nameof(bankName));
            AccountNumber = Check.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));
            AccountType = accountType;
            CurrentBalance = MonetaryValue.Zero(currency);
            LastSyncDate = DateTime.UtcNow;
            IsActive = true;
            ConnectionStatus = ConnectionStatus.Active;
        }

        public void UpdateBalance(MonetaryValue newBalance)
        {
            CurrentBalance = newBalance;
            LastSyncDate = DateTime.UtcNow;
        }

        public void Disconnect()
        {
            IsActive = false;
            ConnectionStatus = ConnectionStatus.Disconnected;
        }

        public void Reconnect()
        {
            IsActive = true;
            ConnectionStatus = ConnectionStatus.Active;
        }
    }
}
