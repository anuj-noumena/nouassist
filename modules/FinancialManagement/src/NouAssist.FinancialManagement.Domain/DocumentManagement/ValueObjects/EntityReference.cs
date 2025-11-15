using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.DocumentManagement.ValueObjects
{
    public class EntityReference : ValueObject
    {
        public string EntityType { get; init; }
        public string EntityId { get; init; }
        public string ReferenceDescription { get; init; }

        protected EntityReference() { }

        public EntityReference(
            string entityType,
            string entityId,
            string referenceDescription = null)
        {
            EntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            EntityId = Check.NotNullOrWhiteSpace(entityId, nameof(entityId));
            ReferenceDescription = referenceDescription;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return EntityType;
            yield return EntityId;
        }
    }
}
