using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkManagement.Shared.ValueObjects
{
    /// <summary>
    /// Represents a collection of custom field values attached to an entity or workflow.
    /// Enforces domain consistency and provides controlled access.
    /// </summary>
    public class CustomFieldValues : ValueObject
    {
        private readonly Dictionary<string, object> _values;

        public IReadOnlyDictionary<string, object> Values => _values;

        protected CustomFieldValues() 
        {
            _values = new Dictionary<string, object>();
        }

        public static CustomFieldValues Create()
        {
            return new CustomFieldValues();
        }

        public CustomFieldValues(Dictionary<string, object> values)
        {
            _values = values ?? new Dictionary<string, object>();
        }

        /// <summary>
        /// Sets or updates a custom field value.
        /// </summary>
        public CustomFieldValues SetValue(string key, object value)
        {
            Check.NotNullOrWhiteSpace(key, nameof(key));

            // Optional domain rule: prevent null or empty object values
            if (value == null)
            {
                throw new BusinessException("Approval:InvalidCustomFieldValue")
                    .WithData("Key", key);
            }

            var newValues = new Dictionary<string, object>(_values)
            {
                [key] = value
            };

            return new CustomFieldValues(newValues);
        }

        /// <summary>
        /// Gets a custom field value by key, or null if not found.
        /// </summary>
        public object GetValue(string key)
        {
            return _values.TryGetValue(key, out var value) ? value : null;
        }
        
        public bool ContainsKey(string key)
        {
            return _values.TryGetValue(key, out var value);
        }

        /// <summary>
        /// Removes a custom field value by key.
        /// </summary>
        public CustomFieldValues RemoveValue(string key)
        {
            if (!_values.ContainsKey(key))
                return this;

            var newValues = new Dictionary<string, object>(_values);
            newValues.Remove(key);

            return new CustomFieldValues(newValues);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            // Value object equality based on the dictionary content
            foreach (var kvp in _values.OrderBy(x => x.Key))
            {
                yield return kvp.Key;
                yield return kvp.Value;
            }
        }
    }
}
