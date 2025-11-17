using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.AuditTrail.ValueObjects
{
    public class EntityReference : ValueObject
    {
        public string EntityType { get; init; }
        public string EntityId { get; init; }
        public string EntityName { get; init; }

        protected EntityReference() { }

        public EntityReference(string entityType, string entityId, string entityName = null)
        {
            EntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            EntityId = Check.NotNullOrWhiteSpace(entityId, nameof(entityId));
            EntityName = entityName;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return EntityType;
            yield return EntityId;
        }
    }
}
