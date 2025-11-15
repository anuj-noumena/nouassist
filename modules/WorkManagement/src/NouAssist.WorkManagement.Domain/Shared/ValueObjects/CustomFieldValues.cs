using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkManagement.Shared.ValueObjects
{
    public class CustomFieldValues : ValueObject
    {
        private Dictionary<string, object> _values;

        private CustomFieldValues()
        {
            _values = new Dictionary<string, object>();
        }

        public static CustomFieldValues Create()
        {
            return new CustomFieldValues();
        }

        public CustomFieldValues SetValue(string key, object value)
        {
            var newValues = new Dictionary<string, object>(_values)
            {
                [key] = value
            };

            return new CustomFieldValues { _values = newValues };
        }

        public object GetValue(string key)
        {
            return _values.ContainsKey(key) ? _values[key] : null;
        }

        public bool HasValue(string key)
        {
            return _values.ContainsKey(key);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            foreach (var kvp in _values)
            {
                yield return kvp.Key;
                yield return kvp.Value;
            }
        }
    }
}