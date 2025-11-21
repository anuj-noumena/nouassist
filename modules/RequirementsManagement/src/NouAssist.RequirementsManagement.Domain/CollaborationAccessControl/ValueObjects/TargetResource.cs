using System;
using System.Collections.Generic;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.ValueObjects
{
    /// <summary>
    /// Represents a target resource that can be the subject of permissions or notifications.
    /// </summary>
    public class TargetResource : ValueObject
    {
        public TargetResourceType Type { get; private set; }
        public Guid? Id { get; private set; }

        private TargetResource() { }

        /// <summary>
        /// Creates a target resource with a specific ID.
        /// </summary>
        public TargetResource(TargetResourceType type, Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Resource ID cannot be empty", nameof(id));
            }

            Type = type;
            Id = id;
        }

        /// <summary>
        /// Creates a target resource representing all resources of a specific type.
        /// </summary>
        public TargetResource(TargetResourceType type)
        {
            Type = type;
            Id = null;
        }

        /// <summary>
        /// Determines if this target resource applies to the specified resource.
        /// </summary>
        public bool AppliesTo(TargetResourceType resourceType, Guid resourceId)
        {
            if (Type != resourceType)
            {
                return false;
            }

            return Id == null || Id == resourceId;
        }

        /// <summary>
        /// Determines if this target resource applies to a resource of the specified type.
        /// </summary>
        public bool AppliesTo(TargetResourceType resourceType)
        {
            return Type == resourceType;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Type;
            if (Id.HasValue)
            {
                yield return Id.Value;
            }
        }
    }
}
