using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects
{
    /// <summary>
    /// Represents the burn rate (cash consumption rate)
    /// </summary>
    public class BurnRateValue : ValueObject
    {
        public decimal MonthlyAmount { get; private set; }
        public decimal DailyAmount { get; private set; }
        public DateTime CalculationDate { get; private set; }
        public int PeriodMonths { get; private set; }

        private BurnRateValue() { }

        public BurnRateValue(decimal monthlyAmount, DateTime calculationDate, int periodMonths = 3)
        {
            if (monthlyAmount < 0) throw new ArgumentException("Burn rate cannot be negative", nameof(monthlyAmount));
            if (periodMonths <= 0) throw new ArgumentException("Period months must be positive", nameof(periodMonths));
            
            MonthlyAmount = monthlyAmount;
            DailyAmount = monthlyAmount / 30; // Approximate daily rate
            CalculationDate = calculationDate;
            PeriodMonths = periodMonths;
        }

        public decimal GetAnnualBurnRate()
        {
            return MonthlyAmount * 12;
        }

        public decimal GetWeeklyBurnRate()
        {
            return MonthlyAmount / 4.33m; // Average weeks in a month
        }

        public decimal GetProjectedCashPosition(decimal currentCash, int monthsAhead)
        {
            return currentCash - (MonthlyAmount * monthsAhead);
        }

        public bool IsIncreasing(BurnRateValue previousBurnRate)
        {
            return MonthlyAmount > previousBurnRate.MonthlyAmount;
        }

        public decimal GetChangePercentage(BurnRateValue previousBurnRate)
        {
            if (previousBurnRate.MonthlyAmount == 0) return 0;
            return ((MonthlyAmount - previousBurnRate.MonthlyAmount) / previousBurnRate.MonthlyAmount) * 100;
        }

        public BurnRateValue ProjectFuture(int monthsAhead, decimal growthRate = 0)
        {
            var futureAmount = MonthlyAmount * (decimal)Math.Pow(1 + (double)growthRate, monthsAhead);
            var futureDate = CalculationDate.AddMonths(monthsAhead);
            return new BurnRateValue(futureAmount, futureDate, PeriodMonths);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return MonthlyAmount;
            yield return DailyAmount;
            yield return CalculationDate;
            yield return PeriodMonths;
        }
    }
}
