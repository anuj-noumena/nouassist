using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.SharedKernel.ValueObjects
{
    /// <summary>
    /// Represents additional contextual or system-level data associated with an approval entity.
    /// Common use cases include integration identifiers, workflow version info,
    /// system source tags, or audit metadata.
    /// </summary>
    public class Metadata : ValueObject
    {
        private readonly Dictionary<string, object> _entries;

        /// <summary>
        /// Provides read-only access to all metadata entries.
        /// </summary>
        public IReadOnlyDictionary<string, object> Entries => _entries;

        /// <summary>
        /// Default constructor for ORM or JSON deserialization.
        /// </summary>
        protected Metadata()
        {
            _entries = new Dictionary<string, object>();
        }

        public static Metadata Create()
        {
            return new Metadata();
        }

        /// <summary>
        /// Initializes metadata with a predefined set of key–value pairs.
        /// </summary>
        public Metadata(Dictionary<string, object> entries)
        {
            _entries = entries ?? new Dictionary<string, object>();
        }

        /// <summary>
        /// Adds or updates a metadata entry and returns a new immutable instance.
        /// </summary>
        public Metadata AddOrUpdate(string key, object value)
        {
            Check.NotNullOrWhiteSpace(key, nameof(key));

            var updated = new Dictionary<string, object>(_entries)
            {
                [key] = value
            };
            return new Metadata(updated);
        }

        /// <summary>
        /// Removes a metadata entry by key and returns a new immutable instance.
        /// </summary>
        public Metadata Remove(string key)
        {
            if (!_entries.ContainsKey(key))
                return this;

            var updated = new Dictionary<string, object>(_entries);
            updated.Remove(key);
            return new Metadata(updated);
        }

        /// <summary>
        /// Retrieves a metadata value by key, or null if not found.
        /// </summary>
        public object Get(string key)
        {
            return _entries.TryGetValue(key, out var value) ? value : null;
        }

        /// <summary>
        /// Retrieves a metadata value by key and attempts to cast it to a specific type.
        /// Returns default(T) if not found or invalid.
        /// </summary>
        public T Get<T>(string key)
        {
            if (_entries.TryGetValue(key, out var value))
            {
                if (value is T typedValue)
                    return typedValue;

                try
                {
                    return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(value));
                }
                catch
                {
                    return default;
                }
            }

            return default;
        }

        /// <summary>
        /// Returns true if the metadata contains a specific key.
        /// </summary>
        public bool Contains(string key)
        {
            return _entries.ContainsKey(key);
        }

        /// <summary>
        /// Returns a JSON string representation of the metadata entries.
        /// Useful for debugging or logging.
        /// </summary>
        public override string ToString()
        {
            return JsonSerializer.Serialize(_entries);
        }

        /// <summary>
        /// Defines value-based equality using dictionary contents.
        /// </summary>
        protected override IEnumerable<object> GetAtomicValues()
        {
            foreach (var kvp in _entries.OrderBy(x => x.Key))
            {
                yield return kvp.Key;
                yield return kvp.Value;
            }
        }
    }
}
