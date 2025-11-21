using System;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Exceptions
{
    /// <summary>
    /// Exception thrown when an attempted operation references a resource that doesn't exist.
    /// </summary>
    public class ResourceNotFoundException : CollaborationDomainException
    {
        public TargetResourceType ResourceType { get; }
        public Guid ResourceId { get; }

        public ResourceNotFoundException(TargetResourceType resourceType, Guid resourceId)
            : base("Collaboration:ErrorMessage:ResourceNotFound")
        {
            ResourceType = resourceType;
            ResourceId = resourceId;

            WithData("resourceType", resourceType);
            WithData("resourceId", resourceId);
        }
    }
}
