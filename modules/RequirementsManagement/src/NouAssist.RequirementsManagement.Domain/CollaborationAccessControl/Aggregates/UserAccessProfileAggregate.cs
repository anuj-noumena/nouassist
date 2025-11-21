using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Entities;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Events;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Exceptions;
using NouAssist.RequirementsManagement.CollaborationAccessControl.ValueObjects;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Aggregates
{
    /// <summary>
    /// Aggregate root for user access profiles, managing roles and permissions.
    /// </summary>
    public class UserAccessProfile : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public UserID UserId { get; private set; }
        
        private readonly List<Role> _roles;
        public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();
        
        protected UserAccessProfile()
        {
            _roles = new List<Role>();
        }
        
        /// <summary>
        /// Factory method to create a new UserAccessProfile.
        /// </summary>
        public static UserAccessProfile Create(
            Guid id,
            UserID userId,
            Guid? tenantId)
        {
            return new UserAccessProfile(id, userId, tenantId);
        }
        
        private UserAccessProfile(
            Guid id,
            UserID userId,
            Guid? tenantId) : base(id)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            TenantId = tenantId;
            _roles = new List<Role>();
        }
        
        /// <summary>
        /// Creates a new role with the specified permissions.
        /// </summary>
        public Role CreateRole(
            Guid roleId,
            RoleName name,
            string description,
            bool isSystemRole = false)
        {
            // Check if a role with the same name already exists
            if (_roles.Any(r => r.Name.Value == name.Value))
            {
                throw new CollaborationDomainException("Collaboration:ErrorMessage:DuplicateRole")
                    .WithData("roleName", name.Value);
            }
            
            var role = new Role(roleId, name, description, isSystemRole);
            _roles.Add(role);
            
            return role;
        }
        
        /// <summary>
        /// Updates an existing role.
        /// </summary>
        public void UpdateRole(
            Guid roleId,
            RoleName name,
            string description)
        {
            var role = _roles.FirstOrDefault(r => r.Id == roleId);
            if (role == null)
            {
                throw new EntityNotFoundException(nameof(Role), roleId);
            }
            
            // Cannot update system roles
            if (role.IsSystemRole)
            {
                throw new CollaborationDomainException(
                    "Collaboration:ErrorMessage:InvalidOperation",
                    "System roles cannot be modified.");
            }
            
            // Check for name conflicts with other roles
            if (_roles.Any(r => r.Id != roleId && r.Name.Value == name.Value))
            {
                throw new CollaborationDomainException("Collaboration:ErrorMessage:DuplicateRole")
                    .WithData("roleName", name.Value);
            }
            
            var oldRoleName = role.Name.Value;
            var newRoleName = name.Value;
            
            // Update the role
            role.SetDescription(description);
            
            // Raise domain event for role update
            AddDistributedEvent(new RoleUpdatedEto
            {
                Id = Guid.NewGuid(),
                TenantId = TenantId,
                RoleId = roleId,
                OldRoleName = oldRoleName,
                NewRoleName = newRoleName
            });
        }
        
        /// <summary>
        /// Deletes a role from this profile.
        /// </summary>
        public void DeleteRole(Guid roleId)
        {
            var role = _roles.FirstOrDefault(r => r.Id == roleId);
            if (role == null)
            {
                return; // Role already deleted or doesn't exist
            }
            
            // Cannot delete system roles
            if (role.IsSystemRole)
            {
                throw new CollaborationDomainException(
                    "Collaboration:ErrorMessage:InvalidOperation",
                    "System roles cannot be deleted.");
            }
            
            _roles.Remove(role);
            
            // Raise domain event for role deletion
            AddDistributedEvent(new RoleDeletedEto
            {
                Id = Guid.NewGuid(),
                TenantId = TenantId,
                RoleId = roleId,
                RoleName = role.Name.Value
            });
        }
        
        /// <summary>
        /// Adds a permission to a role.
        /// </summary>
        public void AddPermissionToRole(
            Guid roleId,
            PermissionType permissionType,
            TargetResource resource)
        {
            var role = _roles.FirstOrDefault(r => r.Id == roleId);
            if (role == null)
            {
                throw new EntityNotFoundException(nameof(Role), roleId);
            }
            
            // Add the permission to the role
            role.AddPermission(permissionType, resource);
            
            // Raise domain event for permission change
            AddDistributedEvent(new PermissionChangedEto
            {
                Id = Guid.NewGuid(),
                TenantId = TenantId,
                UserId = UserId.Value,
                RoleId = roleId,
                RoleName = role.Name.Value,
                ResourceType = resource.Type,
                ResourceId = resource.Id,
                AddedPermissions = new List<PermissionType> { permissionType },
                RemovedPermissions = new List<PermissionType>(),
                ChangedByUserId = CurrentUser.Id.GetValueOrDefault(),
                ChangeTime = Clock.Now
            });
        }
        
        /// <summary>
        /// Removes a permission from a role.
        /// </summary>
        public void RemovePermissionFromRole(
            Guid roleId,
            PermissionType permissionType,
            TargetResource resource)
        {
            var role = _roles.FirstOrDefault(r => r.Id == roleId);
            if (role == null)
            {
                throw new EntityNotFoundException(nameof(Role), roleId);
            }
            
            // Remove the permission from the role
            role.RemovePermission(permissionType, resource);
            
            // Raise domain event for permission change
            AddDistributedEvent(new PermissionChangedEto
            {
                Id = Guid.NewGuid(),
                TenantId = TenantId,
                UserId = UserId.Value,
                RoleId = roleId,
                RoleName = role.Name.Value,
                ResourceType = resource.Type,
                ResourceId = resource.Id,
                AddedPermissions = new List<PermissionType>(),
                RemovedPermissions = new List<PermissionType> { permissionType },
                ChangedByUserId = CurrentUser.Id.GetValueOrDefault(),
                ChangeTime = Clock.Now
            });
        }
        
        /// <summary>
        /// Checks if the user has the specified permission for the resource.
        /// </summary>
        public bool HasPermission(
            PermissionType permissionType,
            TargetResourceType resourceType,
            Guid resourceId)
        {
            return _roles.Any(r => r.HasPermission(permissionType, resourceType, resourceId));
        }
        
        /// <summary>
        /// Checks if the user has the specified permission for any resource of the specified type.
        /// </summary>
        public bool HasPermissionForResourceType(
            PermissionType permissionType,
            TargetResourceType resourceType)
        {
            return _roles.Any(r => r.HasPermissionForResourceType(permissionType, resourceType));
        }
    }
}
