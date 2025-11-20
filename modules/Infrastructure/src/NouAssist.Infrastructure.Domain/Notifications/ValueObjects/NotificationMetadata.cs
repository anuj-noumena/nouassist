using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Values;

namespace NouAssist.Infrastructure.Notifications.ValueObjects
{
    public class NotificationMetadata : ValueObject
    {
        private readonly Dictionary<string, object> _data;
        public IReadOnlyDictionary<string, object> Data => _data;

        private NotificationMetadata()
        {
            _data = new Dictionary<string, object>();
        }

        public NotificationMetadata(Dictionary<string, object> data)
        {
            _data = data ?? new Dictionary<string, object>();
        }

        public static NotificationMetadata Empty() => new NotificationMetadata();

        public static NotificationMetadata Create() => new NotificationMetadata();

        public NotificationMetadata Add(string key, object value)
        {
            var newData = new Dictionary<string, object>(_data)
            {
                [key] = value
            };
            return new NotificationMetadata(newData);
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            if (_data.TryGetValue(key, out var obj) && obj is T typedValue)
            {
                value = typedValue;
                return true;
            }

            value = default;
            return false;
        }

        public T GetValue<T>(string key, T defaultValue = default)
        {
            return TryGetValue<T>(key, out var value) ? value : defaultValue;
        }

        public bool ContainsKey(string key) => _data.ContainsKey(key);

        public int Count => _data.Count;

        protected override IEnumerable<object> GetAtomicValues()
        {
            foreach (var kvp in _data.OrderBy(x => x.Key))
            {
                yield return kvp.Key;
                yield return kvp.Value;
            }
        }
    }
}
