using System;
using NouAssist.CreditScoreMonitoring.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.FinancialManagement.CreditScoreMonitoring.Entities
{
    public class CreditAccount : Entity
    {
        public Guid CreditReportId { get; private set; }
        public string AccountNumber { get; private set; }
        public string CreditorName { get; private set; }
        public string AccountType { get; private set; }
        public AccountStatus Status { get; private set; }
        public decimal CreditLimit { get; private set; }
        public decimal CurrentBalance { get; private set; }
        public decimal UtilizationPercentage { get; private set; }
        public DateTime OpenedDate { get; private set; }
        public DateTime? ClosedDate { get; private set; }
        public int PaymentHistory30Days { get; private set; }
        public int PaymentHistory60Days { get; private set; }
        public int PaymentHistory90Days { get; private set; }

        protected CreditAccount() { /* ORM */ }

        public CreditAccount(
            Guid creditReportId,
            string accountNumber,
            string creditorName,
            string accountType,
            AccountStatus status,
            decimal creditLimit,
            decimal currentBalance,
            DateTime openedDate)
        {
            CreditReportId = creditReportId;
            AccountNumber = Check.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));
            CreditorName = Check.NotNullOrWhiteSpace(creditorName, nameof(creditorName));
            AccountType = Check.NotNullOrWhiteSpace(accountType, nameof(accountType));
            Status = status;
            CreditLimit = Check.Positive(creditLimit, nameof(creditLimit));
            CurrentBalance = Check.Positive(currentBalance, nameof(currentBalance));
            OpenedDate = openedDate;
            CalculateUtilization();
        }

        public void UpdateBalance(decimal newBalance)
        {
            CurrentBalance = Check.Positive(newBalance, nameof(newBalance));
            CalculateUtilization();
        }

        public void UpdatePaymentHistory(int days30, int days60, int days90)
        {
            PaymentHistory30Days = Check.Positive(days30, nameof(days30));
            PaymentHistory60Days = Check.Positive(days60, nameof(days60));
            PaymentHistory90Days = Check.Positive(days90, nameof(days90));
        }

        public void Close(DateTime closedDate)
        {
            Status = AccountStatus.Closed;
            ClosedDate = closedDate;
        }

        private void CalculateUtilization()
        {
            if (CreditLimit > 0)
            {
                UtilizationPercentage = (CurrentBalance / CreditLimit) * 100;
            }
        }

        public bool HasDelinquentPayments()
        {
            return PaymentHistory30Days > 0 || PaymentHistory60Days > 0 || PaymentHistory90Days > 0;
        }

        public override object[] GetKeys() => new object[] { CreditReportId, AccountNumber };
    }
}
