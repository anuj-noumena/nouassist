using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Customization.CustomFields.Entities
{
    public class CustomFieldValue : FullAuditedEntity<Guid>, IMultiTenant
    {
        public Guid FieldDefinitionId { get; private set; }
        public string EntityType { get; private set; }
        public Guid EntityId { get; private set; }
        public string ValueJson { get; private set; } // Store as JSON for flexibility
        public Guid? TenantId { get; private set; }

        protected CustomFieldValue() { /* ORM */ }

        public CustomFieldValue(
            Guid id,
            Guid fieldDefinitionId,
            string entityType,
            Guid entityId,
            string valueJson,
            Guid? tenantId
        ) : base(id)
        {
            FieldDefinitionId = fieldDefinitionId;
            EntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            EntityId = entityId;
            ValueJson = valueJson;
            TenantId = tenantId;
        }

        public void UpdateValue(string newValueJson)
        {
            ValueJson = newValueJson;
        }

        // Type-safe getters using System.Text.Json
        public T GetValue<T>()
        {
            if (string.IsNullOrEmpty(ValueJson))
            {
                return default;
            }

            try
            {
                return System.Text.Json.JsonSerializer.Deserialize<T>(ValueJson);
            }
            catch
            {
                return default;
            }
        }

        public void SetValue<T>(T value)
        {
            ValueJson = System.Text.Json.JsonSerializer.Serialize(value);
        }
    }
}

// Add to repository interface
