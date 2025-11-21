using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects
{
    /// <summary>
    /// Represents a benchmark value for metric comparison
    /// </summary>
    public class BenchmarkValue : ValueObject
    {
        public MetricValue Value { get; private set; }
        public string Source { get; private set; }
        public DateTime Date { get; private set; }
        public string Category { get; private set; }
        public string Notes { get; private set; }

        private BenchmarkValue() { }

        public BenchmarkValue(MetricValue value, string source, DateTime date, 
                            string category = null, string notes = null)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
            Source = Check.NotNullOrWhiteSpace(source, nameof(source));
            Date = date;
            Category = category?.Trim();
            Notes = notes?.Trim();
        }

        public decimal GetVariancePercentage(MetricValue actualValue)
        {
            if (Value.Value == 0) return 0;
            return ((actualValue.Value - Value.Value) / Value.Value) * 100;
        }

        public bool IsActualBetter(MetricValue actualValue, bool higherIsBetter = true)
        {
            return higherIsBetter ? actualValue.Value > Value.Value : actualValue.Value < Value.Value;
        }

        public BenchmarkValue WithNotes(string notes)
        {
            return new BenchmarkValue(Value, Source, Date, Category, notes);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return Source;
            yield return Date;
            yield return Category;
            yield return Notes;
        }
    }
}
