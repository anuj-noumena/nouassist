using System;
using NouAssist.SharedKernel.Utils;
using Volo.Abp.Domain.Entities;

namespace NouAssist.SharedKernel.Entities
{
    public class EntityTag : Entity
    {
        public Guid EntityType { get; private set; }
        public Guid EntityId { get; private set; }
        public Guid TagId { get; private set; }
        public Guid? TaggedBy { get; private set; }
        public DateTime TaggedAt { get; private set; }

        protected EntityTag() { }

        public EntityTag(string entityType, Guid entityId, Guid tagId, Guid? taggedBy = null)
        {
            EntityType = DomainUtils.ConvertEntityTypeToGuid(entityType);
            EntityId = entityId;
            TagId = tagId;
            TaggedBy = taggedBy;
            TaggedAt = DateTime.UtcNow;
        }

        public override object[] GetKeys()
        {
            return new object[] { EntityType, EntityId, TagId };
        }
    }
}
