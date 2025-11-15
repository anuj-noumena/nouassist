using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.DebtManagement.ValueObjects
{
    public class PayoffProjection : ValueObject
    {
        public DateTime ProjectedPayoffDate { get; init; }
        public int MonthsToPayoff { get; init; }
        public decimal TotalInterestToPay { get; init; }
        public decimal TotalAmountToPay { get; init; }
        public decimal MonthlyPayment { get; init; }

        public PayoffProjection(
            DateTime projectedPayoffDate,
            int monthsToPayoff,
            decimal totalInterestToPay,
            decimal totalAmountToPay,
            decimal monthlyPayment)
        {
            ProjectedPayoffDate = projectedPayoffDate;
            MonthsToPayoff = monthsToPayoff;
            TotalInterestToPay = totalInterestToPay;
            TotalAmountToPay = totalAmountToPay;
            MonthlyPayment = monthlyPayment;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ProjectedPayoffDate;
            yield return MonthsToPayoff;
            yield return TotalInterestToPay;
            yield return TotalAmountToPay;
            yield return MonthlyPayment;
        }
    }
}
