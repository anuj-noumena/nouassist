using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects
{
    /// <summary>
    /// Value object representing a performance metric for a portfolio
    /// </summary>
    public class PerformanceMetric : ValueObject
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Target { get; private set; }
        public decimal Actual { get; private set; }
        public string Unit { get; private set; }
        public DateTime LastUpdated { get; private set; }

        protected PerformanceMetric() { }

        public PerformanceMetric(string name, string description, decimal target, decimal actual, string unit, DateTime lastUpdated)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Description = description;
            Target = target;
            Actual = actual;
            Unit = Check.NotNullOrWhiteSpace(unit, nameof(unit));
            LastUpdated = lastUpdated;
        }

        public void UpdateActualValue(decimal actual, DateTime updateTime)
        {
            Actual = actual;
            LastUpdated = updateTime;
        }

        public decimal CalculateProgress()
        {
            if (Target == 0)
            {
                return 0;
            }
            
            return Math.Min(100, (Actual / Target) * 100);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Description ?? string.Empty;
            yield return Target;
            yield return Actual;
            yield return Unit;
            yield return LastUpdated;
        }
    }
}
