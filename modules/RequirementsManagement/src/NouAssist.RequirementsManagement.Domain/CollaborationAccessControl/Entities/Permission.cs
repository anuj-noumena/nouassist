using System;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;
using NouAssist.RequirementsManagement.CollaborationAccessControl.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Entities
{
    /// <summary>
    /// Represents a specific permission assigned to a role for a resource.
    /// </summary>
    public class Permission : Entity
    {
        public PermissionType Type { get; private set; }
        public TargetResource Resource { get; private set; }
        
        // For EF Core
        protected Permission() { }
        
        internal Permission(PermissionType type, TargetResource resource)
        {
            Type = type;
            Resource = resource ?? throw new ArgumentNullException(nameof(resource));
        }
        
        /// <summary>
        /// Checks if this permission applies to the specified resource.
        /// </summary>
        public bool AppliesTo(TargetResourceType resourceType, Guid resourceId)
        {
            return Resource.AppliesTo(resourceType, resourceId);
        }
        
        /// <summary>
        /// Checks if this permission includes the specified permission type.
        /// </summary>
        public bool Includes(PermissionType permissionType)
        {
            // FullControl includes all permissions
            if (Type == PermissionType.FullControl)
            {
                return true;
            }
            
            // Write permission includes Read permission
            if (Type == PermissionType.Write && permissionType == PermissionType.Read)
            {
                return true;
            }
            
            // ManagePermissions includes all other permissions except ManageRoles
            if (Type == PermissionType.ManagePermissions && permissionType != PermissionType.ManageRoles)
            {
                return true;
            }
            
            return Type == permissionType;
        }
        
        public override object[] GetKeys()
        {
            return new object[] { Type, Resource };
        }
    }
}
