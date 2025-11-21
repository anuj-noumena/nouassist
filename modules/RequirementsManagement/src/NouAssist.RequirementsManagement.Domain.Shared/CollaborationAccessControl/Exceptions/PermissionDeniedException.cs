using System;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Exceptions
{
    /// <summary>
    /// Exception thrown when a user attempts to perform an operation without sufficient permissions.
    /// </summary>
    public class PermissionDeniedException : CollaborationDomainException
    {
        public Guid UserId { get; }
        public PermissionType RequiredPermission { get; }
        public TargetResourceType ResourceType { get; }
        public Guid ResourceId { get; }

        public PermissionDeniedException(
            Guid userId,
            PermissionType requiredPermission,
            TargetResourceType resourceType,
            Guid resourceId)
            : base("Collaboration:ErrorMessage:PermissionDenied")
        {
            UserId = userId;
            RequiredPermission = requiredPermission;
            ResourceType = resourceType;
            ResourceId = resourceId;

            WithData("userId", userId);
            WithData("requiredPermission", requiredPermission);
            WithData("resourceType", resourceType);
            WithData("resourceId", resourceId);
        }
    }
}
