using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects
{
    /// <summary>
    /// Represents a projected financial amount with confidence intervals
    /// </summary>
    public class ProjectedAmount : ValueObject
    {
        public decimal Amount { get; private set; }
        public decimal? LowerBound { get; private set; }
        public decimal? UpperBound { get; private set; }
        public decimal ConfidenceLevel { get; private set; }
        public int PeriodIndex { get; private set; }
        public DateTime ProjectionDate { get; private set; }

        private ProjectedAmount() { }

        public ProjectedAmount(decimal amount, int periodIndex, DateTime projectionDate, 
                             decimal confidenceLevel = 0.95m, decimal? lowerBound = null, decimal? upperBound = null)
        {
            Amount = amount;
            PeriodIndex = periodIndex;
            ProjectionDate = projectionDate;
            SetConfidenceLevel(confidenceLevel);
            
            if (lowerBound.HasValue && lowerBound.Value > amount)
            {
                throw new ArgumentException("Lower bound cannot be greater than the amount");
            }
            
            if (upperBound.HasValue && upperBound.Value < amount)
            {
                throw new ArgumentException("Upper bound cannot be less than the amount");
            }
            
            LowerBound = lowerBound;
            UpperBound = upperBound;
        }

        public bool IsWithinBounds(decimal actualAmount)
        {
            if (LowerBound.HasValue && actualAmount < LowerBound.Value) return false;
            if (UpperBound.HasValue && actualAmount > UpperBound.Value) return false;
            return true;
        }

        public decimal GetVariance()
        {
            if (!LowerBound.HasValue || !UpperBound.HasValue) return 0;
            return UpperBound.Value - LowerBound.Value;
        }

        public decimal GetAccuracy(decimal actualAmount)
        {
            if (Amount == 0) return actualAmount == 0 ? 100 : 0;
            return 100 - Math.Abs((actualAmount - Amount) / Amount * 100);
        }

        public ProjectedAmount AdjustForInflation(decimal inflationRate)
        {
            var adjustedAmount = Amount * (1 + inflationRate);
            var adjustedLower = LowerBound * (1 + inflationRate);
            var adjustedUpper = UpperBound * (1 + inflationRate);
            
            return new ProjectedAmount(adjustedAmount, PeriodIndex, ProjectionDate, ConfidenceLevel, adjustedLower, adjustedUpper);
        }

        private void SetConfidenceLevel(decimal confidenceLevel)
        {
            if (confidenceLevel <= 0 || confidenceLevel > 1)
            {
                throw new ArgumentException("Confidence level must be between 0 and 1", nameof(confidenceLevel));
            }
            ConfidenceLevel = confidenceLevel;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Amount;
            yield return LowerBound;
            yield return UpperBound;
            yield return ConfidenceLevel;
            yield return PeriodIndex;
            yield return ProjectionDate;
        }
    }
}
