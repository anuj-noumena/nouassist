using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkManagement.Shared.ValueObjects
{
    public class CustomFieldValues : ValueObject
    {
        private Dictionary<string, object> _values;

        private CustomFieldValues(Dictionary<string, object>? values = null)
        {
            _values = values ?? new Dictionary<string, object>();
        }

        public static CustomFieldValues Create(Dictionary<string, object>? values = null)
        {
            return new CustomFieldValues(values);
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
        
        public Dictionary<string, object> GetValues()
        {
            // Return a copy to prevent external mutation of the internal state
            return new Dictionary<string, object>(_values);
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