using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.DebtManagement.ValueObjects
{
    public class InterestCalculation : ValueObject
    {
        public decimal PrincipalAmount { get; init; }
        public decimal InterestAmount { get; init; }
        public decimal TotalPayment { get; init; }
        public decimal RemainingBalance { get; init; }
        public DateTime CalculationDate { get; init; }
        public int DaysSinceLastPayment { get; init; }

        public InterestCalculation(
            decimal principalAmount,
            decimal interestAmount,
            decimal totalPayment,
            decimal remainingBalance,
            DateTime calculationDate,
            int daysSinceLastPayment)
        {
            PrincipalAmount = principalAmount;
            InterestAmount = interestAmount;
            TotalPayment = totalPayment;
            RemainingBalance = remainingBalance;
            CalculationDate = calculationDate;
            DaysSinceLastPayment = daysSinceLastPayment;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return PrincipalAmount;
            yield return InterestAmount;
            yield return TotalPayment;
            yield return RemainingBalance;
            yield return CalculationDate;
            yield return DaysSinceLastPayment;
        }
    }
}
