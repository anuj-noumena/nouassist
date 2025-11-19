using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.Customization.CustomFields.ValueObjects
{
    public class CustomFieldValueData : ValueObject
    {
        public Guid FieldDefinitionId { get; private set; }
        public object Value { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private CustomFieldValueData() { /* ORM */ }

        public CustomFieldValueData(Guid fieldDefinitionId, object value)
        {
            FieldDefinitionId = fieldDefinitionId;
            Value = value;
            UpdatedAt = DateTime.UtcNow;
        }

        public string AsString() => Value?.ToString();
        public int? AsInt() => Value is int i ? i : null;
        public decimal? AsDecimal() => Value is decimal d ? d : null;
        public bool? AsBool() => Value is bool b ? b : null;
        
        public DateTime? AsDateTime()
        {
            if (Value is DateTime dt)
            {
                return dt;
            }

            if (Value is string s && DateTime.TryParse(s, out var parsed))
            {
                return parsed;
            }

            return null;
        }

        public List<string> AsStringList()
        {
            return Value is List<string> list ? list : new List<string>();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return FieldDefinitionId;
            yield return Value;
            yield return UpdatedAt;
        }
    }
}
