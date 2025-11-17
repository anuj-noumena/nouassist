using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.DebtManagement.ValueObjects
{
    public class AmortizationScheduleEntry : ValueObject
    {
        public int PaymentNumber { get; init; }
        public DateTime PaymentDate { get; init; }
        public decimal PaymentAmount { get; init; }
        public decimal PrincipalAmount { get; init; }
        public decimal InterestAmount { get; init; }
        public decimal RemainingBalance { get; init; }

        public AmortizationScheduleEntry(
            int paymentNumber,
            DateTime paymentDate,
            decimal paymentAmount,
            decimal principalAmount,
            decimal interestAmount,
            decimal remainingBalance)
        {
            PaymentNumber = paymentNumber;
            PaymentDate = paymentDate;
            PaymentAmount = paymentAmount;
            PrincipalAmount = principalAmount;
            InterestAmount = interestAmount;
            RemainingBalance = remainingBalance;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return PaymentNumber;
            yield return PaymentDate;
            yield return PaymentAmount;
            yield return PrincipalAmount;
            yield return InterestAmount;
            yield return RemainingBalance;
        }
    }
}
