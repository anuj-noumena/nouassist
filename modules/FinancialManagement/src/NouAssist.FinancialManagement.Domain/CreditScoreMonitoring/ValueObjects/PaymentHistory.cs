using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.CreditScoreMonitoring.ValueObjects
{
    public class PaymentHistory : ValueObject
    {
        public int TotalAccounts { get; init; }
        public int AccountsWithLatePayments { get; init; }
        public int DaysPastDue { get; init; }
        public DateTime? MostRecentLatePayment { get; init; }
        public decimal OnTimePaymentPercentage { get; init; }

        public PaymentHistory(
            int totalAccounts,
            int accountsWithLatePayments,
            int daysPastDue,
            DateTime? mostRecentLatePayment)
        {
            TotalAccounts = totalAccounts;
            AccountsWithLatePayments = accountsWithLatePayments;
            DaysPastDue = daysPastDue;
            MostRecentLatePayment = mostRecentLatePayment;
            OnTimePaymentPercentage = totalAccounts > 0
                ? ((decimal)(totalAccounts - accountsWithLatePayments) / totalAccounts) * 100
                : 100;
        }

        public bool IsExcellent => OnTimePaymentPercentage == 100;
        public bool IsGood => OnTimePaymentPercentage >= 95;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return TotalAccounts;
            yield return AccountsWithLatePayments;
            yield return DaysPastDue;
            yield return MostRecentLatePayment ?? DateTime.MinValue;
            yield return OnTimePaymentPercentage;
        }
    }
}
