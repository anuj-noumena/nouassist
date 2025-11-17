using System.Collections.Generic;
using System.Text.Json;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.AuditTrail.ValueObjects
{
    public class AuditMetadata : ValueObject
    {
        public Dictionary<string, object> Properties { get; init; }
        
        protected AuditMetadata() { }

        public AuditMetadata(Dictionary<string, object> properties)
        {
            Properties = properties ?? new Dictionary<string, object>();
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(Properties);
        }

        public static AuditMetadata FromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return new AuditMetadata(new Dictionary<string, object>());
            }

            var properties = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            return new AuditMetadata(properties);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            foreach (var kvp in Properties)
            {
                yield return kvp.Key;
                yield return kvp.Value;
            }
        }
    }
}
