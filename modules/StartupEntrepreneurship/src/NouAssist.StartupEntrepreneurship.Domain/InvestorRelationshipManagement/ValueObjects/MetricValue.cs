using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects
{
    /// <summary>
    /// Value object representing a metric value in investor updates
    /// </summary>
    public class MetricValue : ValueObject
    {
        public string Name { get; private set; }
        public decimal Value { get; private set; }
        public string Unit { get; private set; }
        public decimal? PreviousValue { get; private set; }
        public decimal? TargetValue { get; private set; }

        private MetricValue() { }

        public MetricValue(
            string name,
            decimal value,
            string unit = null,
            decimal? previousValue = null,
            decimal? targetValue = null)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Value = value;
            Unit = unit;
            PreviousValue = previousValue;
            TargetValue = targetValue;
        }

        public decimal? GetGrowthPercentage()
        {
            if (!PreviousValue.HasValue || PreviousValue.Value == 0)
                return null;

            return ((Value - PreviousValue.Value) / PreviousValue.Value) * 100;
        }

        public decimal? GetTargetAchievementPercentage()
        {
            if (!TargetValue.HasValue || TargetValue.Value == 0)
                return null;

            return (Value / TargetValue.Value) * 100;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Value;
            yield return Unit;
            yield return PreviousValue;
            yield return TargetValue;
        }
    }
}
