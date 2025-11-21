using System;
using System.Collections.Generic;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.ValueObjects
{
    /// <summary>
    /// Represents a value assigned to a requirement attribute
    /// </summary>
    public class AttributeValue : ValueObject
    {
        public string StringValue { get; private set; }
        public decimal? NumericValue { get; private set; }
        public bool? BooleanValue { get; private set; }
        public DateTime? DateTimeValue { get; private set; }
        public AttributeType ValueType { get; private set; }

        private AttributeValue() { }

        public static AttributeValue FromString(string value)
        {
            Check.NotNull(value, nameof(value));
            return new AttributeValue
            {
                StringValue = value,
                ValueType = AttributeType.Custom
            };
        }

        public static AttributeValue FromNumber(decimal value)
        {
            return new AttributeValue
            {
                NumericValue = value,
                StringValue = value.ToString(),
                ValueType = AttributeType.Priority
            };
        }

        public static AttributeValue FromBoolean(bool value)
        {
            return new AttributeValue
            {
                BooleanValue = value,
                StringValue = value.ToString(),
                ValueType = AttributeType.Custom
            };
        }

        public static AttributeValue FromDateTime(DateTime value)
        {
            return new AttributeValue
            {
                DateTimeValue = value,
                StringValue = value.ToString("yyyy-MM-dd HH:mm:ss"),
                ValueType = AttributeType.Custom
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return StringValue ?? string.Empty;
            yield return NumericValue ?? 0;
            yield return BooleanValue ?? false;
            yield return DateTimeValue ?? DateTime.MinValue;
            yield return ValueType;
        }
    }
}
