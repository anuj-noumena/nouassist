using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.InvestmentManagement.ValueObjects
{
    public class PerformanceMetrics : ValueObject
    {
        public decimal TotalReturn { get; init; }
        public decimal TotalReturnPercentage { get; init; }
        public decimal AnnualizedReturn { get; init; }
        public decimal VolatilityStdDev { get; init; }
        public decimal SharpeRatio { get; init; }
        public decimal MaxDrawdown { get; init; }
        public DateTime PeriodStart { get; init; }
        public DateTime PeriodEnd { get; init; }

        public PerformanceMetrics(
            decimal totalReturn,
            decimal totalReturnPercentage,
            decimal annualizedReturn,
            decimal volatilityStdDev,
            decimal sharpeRatio,
            decimal maxDrawdown,
            DateTime periodStart,
            DateTime periodEnd)
        {
            TotalReturn = totalReturn;
            TotalReturnPercentage = totalReturnPercentage;
            AnnualizedReturn = annualizedReturn;
            VolatilityStdDev = volatilityStdDev;
            SharpeRatio = sharpeRatio;
            MaxDrawdown = maxDrawdown;
            PeriodStart = periodStart;
            PeriodEnd = periodEnd;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return TotalReturn;
            yield return TotalReturnPercentage;
            yield return AnnualizedReturn;
            yield return VolatilityStdDev;
            yield return SharpeRatio;
            yield return MaxDrawdown;
            yield return PeriodStart;
            yield return PeriodEnd;
        }
    }
}
