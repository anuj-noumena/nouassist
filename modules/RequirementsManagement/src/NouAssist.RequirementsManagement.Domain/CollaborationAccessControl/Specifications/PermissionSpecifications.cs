using System;
using System.Linq.Expressions;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Entities;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Specifications
{
    /// <summary>
    /// Base specification for permissions.
    /// </summary>
    public abstract class PermissionSpecification : Specification<Permission>
    {
    }

    /// <summary>
    /// Specification for permissions applying to a specific resource.
    /// </summary>
    public class PermissionForResourceSpecification : PermissionSpecification
    {
        private readonly TargetResourceType _resourceType;
        private readonly Guid _resourceId;

        public PermissionForResourceSpecification(TargetResourceType resourceType, Guid resourceId)
        {
            _resourceType = resourceType;
            _resourceId = resourceId;
        }

        public override Expression<Func<Permission, bool>> ToExpression()
        {
            return permission => permission.AppliesTo(_resourceType, _resourceId);
        }
    }

    /// <summary>
    /// Specification for permissions of a specific type.
    /// </summary>
    public class PermissionTypeSpecification : PermissionSpecification
    {
        private readonly PermissionType _permissionType;

        public PermissionTypeSpecification(PermissionType permissionType)
        {
            _permissionType = permissionType;
        }

        public override Expression<Func<Permission, bool>> ToExpression()
        {
            return permission => permission.Includes(_permissionType);
        }
    }

    /// <summary>
    /// Specification for permissions of a specific type applying to a specific resource.
    /// </summary>
    public class SpecificPermissionForResourceSpecification : PermissionSpecification
    {
        private readonly PermissionType _permissionType;
        private readonly TargetResourceType _resourceType;
        private readonly Guid _resourceId;

        public SpecificPermissionForResourceSpecification(
            PermissionType permissionType,
            TargetResourceType resourceType,
            Guid resourceId)
        {
            _permissionType = permissionType;
            _resourceType = resourceType;
            _resourceId = resourceId;
        }

        public override Expression<Func<Permission, bool>> ToExpression()
        {
            return permission => 
                permission.Includes(_permissionType) && 
                permission.AppliesTo(_resourceType, _resourceId);
        }
    }
}
