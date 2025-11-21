using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects
{
    /// <summary>
    /// Represents the remaining runway in months
    /// </summary>
    public class RunwayLength : ValueObject
    {
        public int Months { get; private set; }
        public DateTime DepletionDate { get; private set; }
        public decimal CurrentBurnRate { get; private set; }
        public decimal CurrentCashBalance { get; private set; }

        private RunwayLength() { }

        public RunwayLength(int months, DateTime depletionDate, decimal currentBurnRate, decimal currentCashBalance)
        {
            if (months < 0) throw new ArgumentException("Months cannot be negative", nameof(months));
            
            Months = months;
            DepletionDate = depletionDate;
            CurrentBurnRate = currentBurnRate;
            CurrentCashBalance = currentCashBalance;
        }

        public bool IsCritical(int criticalThresholdMonths = 6)
        {
            return Months <= criticalThresholdMonths;
        }

        public bool IsRunningOut(int warningThresholdMonths = 12)
        {
            return Months <= warningThresholdMonths;
        }

        public decimal GetEstimatedFundingNeeded(int targetMonths = 18)
        {
            if (Months >= targetMonths) return 0;
            
            var additionalMonths = targetMonths - Months;
            return additionalMonths * CurrentBurnRate;
        }

        public RunwayLength ExtendWith(decimal additionalFunding)
        {
            if (CurrentBurnRate <= 0) 
                return new RunwayLength(int.MaxValue, DateTime.MaxValue, CurrentBurnRate, CurrentCashBalance + additionalFunding);
            
            var newBalance = CurrentCashBalance + additionalFunding;
            var newMonths = (int)(newBalance / CurrentBurnRate);
            var newDepletionDate = DateTime.UtcNow.AddMonths(newMonths);
            
            return new RunwayLength(newMonths, newDepletionDate, CurrentBurnRate, newBalance);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Months;
            yield return DepletionDate;
            yield return CurrentBurnRate;
            yield return CurrentCashBalance;
        }
    }
}
