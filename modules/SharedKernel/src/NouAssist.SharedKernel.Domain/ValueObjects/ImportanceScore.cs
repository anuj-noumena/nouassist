using System.Collections.Generic;
using NouAssist.SharedKernel.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.SharedKernel.ValueObjects
{
    public class ImportanceScore : ValueObject
    {
        public double Value { get; private set; }

        public const double MinValue = 0.0;
        public const double MaxValue = 1.0;
        public const double HighThreshold = 0.75;
        public const double LowThreshold = 0.25;

        private ImportanceScore() { }

        public ImportanceScore(double value)
        {
            if (value < MinValue || value > MaxValue)
            {
                throw new BusinessException("Core:InvalidImportanceScore", 
                    $"Importance score must be between {MinValue} and {MaxValue}.");
            }
            Value = value;
        }

        public static ImportanceScore Default() => new(0.5);
        public static ImportanceScore High() => new(0.8);
        public static ImportanceScore Medium() => new(0.5);
        public static ImportanceScore Low() => new(0.2);

        public bool IsHigh => Value >= HighThreshold;
        public bool IsLow => Value <= LowThreshold;
        public bool IsMedium => !IsHigh && !IsLow;

        public EisenhowerQuadrant DetermineQuadrant(UrgencyScore urgency)
        {
            if (IsHigh && urgency.IsHigh) return EisenhowerQuadrant.Q1DoFirst;
            if (IsHigh && !urgency.IsHigh) return EisenhowerQuadrant.Q2Schedule;
            if (!IsHigh && urgency.IsHigh) return EisenhowerQuadrant.Q3Delegate;
            return EisenhowerQuadrant.Q4Eliminate;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }

    public class UrgencyScore : ValueObject
    {
        public double Value { get; private set; }

        public const double MinValue = 0.0;
        public const double MaxValue = 1.0;
        public const double HighThreshold = 0.75;
        public const double LowThreshold = 0.25;

        private UrgencyScore() { }

        public UrgencyScore(double value)
        {
            if (value < MinValue || value > MaxValue)
            {
                throw new BusinessException("Core:InvalidUrgencyScore", 
                    $"Urgency score must be between {MinValue} and {MaxValue}.");
            }
            Value = value;
        }

        public static UrgencyScore Default() => new(0.5);
        public static UrgencyScore High() => new(0.8);
        public static UrgencyScore Low() => new(0.2);

        public bool IsHigh => Value >= HighThreshold;
        public bool IsLow => Value <= LowThreshold;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
