using System;
using NouAssist.CollaborationHub.Resources.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.CollaborationHub.Resources.Entities
{
    public class Attachment : Entity
    {
        public Guid ResourceId { get; private set; }
        public AttachedEntityType EntityType { get; private set; }
        public Guid EntityId { get; private set; }
        public Guid? AttachedBy { get; private set; }
        public DateTime AttachedAt { get; private set; }
        public string Notes { get; private set; }

        protected Attachment() { /* ORM */ }

        public Attachment(
            Guid resourceId,
            AttachedEntityType entityType,
            Guid entityId,
            Guid? attachedBy = null)
        {
            ResourceId = Check.NotNull(resourceId, nameof(resourceId));
            EntityType = entityType;
            EntityId = Check.NotNull(entityId, nameof(entityId));
            AttachedBy = attachedBy;
            AttachedAt = DateTime.UtcNow;
        }

        public void SetNotes(string notes)
        {
            Notes = notes;
        }

        public bool HasNotes => !string.IsNullOrWhiteSpace(Notes);

        public override object[] GetKeys() => new object[] { ResourceId, EntityType, EntityId };
    }
}
