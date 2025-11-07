using System;
using System.Collections.Generic;
using NouAssist.Infrastructure.SearchEngine.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.Infrastructure.SearchEngine.ValueObjects
{
    public class SearchFilter : ValueObject
    {
        public string Field { get; init; }
        public FilterType Type { get; init; }
        public object Value { get; init; }
        public List<object> Values { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public double? MinValue { get; init; }
        public double? MaxValue { get; init; }
        public bool? BoolValue { get; init; }

        private SearchFilter() { }

        public SearchFilter(
            string field,
            FilterType type,
            object value = null,
            List<object> values = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            double? minValue = null,
            double? maxValue = null,
            bool? boolValue = null)
        {
            Field = field;
            Type = type;
            Value = value;
            Values = values;
            StartDate = startDate;
            EndDate = endDate;
            MinValue = minValue;
            MaxValue = maxValue;
            BoolValue = boolValue;
        }

        public static SearchFilter Exact(string field, object value)
        {
            return new SearchFilter(field, FilterType.Exact, value: value);
        }

        public static SearchFilter Contains(string field, string value)
        {
            return new SearchFilter(field, FilterType.Contains, value: value);
        }

        public static SearchFilter DateRange(string field, DateTime? startDate, DateTime? endDate)
        {
            return new SearchFilter(field, FilterType.DateRange, startDate: startDate, endDate: endDate);
        }

        public static SearchFilter NumericRange(string field, double? minValue, double? maxValue)
        {
            return new SearchFilter(field, FilterType.NumericRange, minValue: minValue, maxValue: maxValue);
        }

        public static SearchFilter InList(string field, List<object> values)
        {
            return new SearchFilter(field, FilterType.InList, values: values);
        }

        public static SearchFilter Boolean(string field, bool value)
        {
            return new SearchFilter(field, FilterType.Boolean, boolValue: value);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Field;
            yield return Type;
            yield return Value;
        }
    }
}
