using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.IdentityAccess.Team.Aggregates;
using NouAssist.IdentityAccess.Team.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.IdentityAccess.Team.Services
{
    public class RolePermissionService : DomainService
    {
        public bool UserHasPermission(
            Guid userId,
            string permission,
            List<UserRole> userRoles,
            List<Role> roles
        )
        {
            var activeUserRoles = userRoles
                .Where(ur => ur.UserId == userId && 
                            ur.IsActive && 
                            !ur.IsExpired)
                .ToList();

            foreach (var userRole in activeUserRoles)
            {
                var role = roles.FirstOrDefault(r => r.Id == userRole.RoleId);
                if (role != null && role.IsActive && role.HasPermission(permission))
                {
                    return true;
                }
            }

            return false;
        }

        public List<string> GetUserPermissions(
            Guid userId,
            List<UserRole> userRoles,
            List<Role> roles
        )
        {
            var permissions = new HashSet<string>();

            var activeUserRoles = userRoles
                .Where(ur => ur.UserId == userId && 
                            ur.IsActive && 
                            !ur.IsExpired)
                .ToList();

            foreach (var userRole in activeUserRoles)
            {
                var role = roles.FirstOrDefault(r => r.Id == userRole.RoleId);
                if (role != null && role.IsActive)
                {
                    foreach (var permission in role.Permissions)
                    {
                        permissions.Add(permission);
                    }
                }
            }

            return permissions.ToList();
        }

        public UserRole AssignRoleToUser(
            Guid userId,
            Role role,
            Guid assignedBy,
            Guid? tenantId,
            DateTime? expiresAt = null,
            string scope = null,
            Guid? scopeId = null
        )
        {
            if (!role.IsActive)
            {
                throw new BusinessException("Team:RoleNotActive");
            }

            if (role.MaxUsers.HasValue)
            {
                // This check would require repository access, 
                // so it should be done in application service
                // Just placeholder here to show the concept
            }

            var userRole = new UserRole(
                Guid.NewGuid(),
                userId,
                role.Id,
                tenantId,
                assignedBy,
                expiresAt,
                scope,
                scopeId
            );

            return userRole;
        }

        public bool CanAssignRole(
            Guid assignerId,
            Role roleToAssign,
            List<UserRole> assignerRoles,
            List<Role> allRoles
        )
        {
            // Check if assigner has role management permission
            return UserHasPermission(
                assignerId,
                "roleManage",
                assignerRoles,
                allRoles
            );
        }

        public List<Role> GetUserRoles(
            Guid userId,
            List<UserRole> userRoles,
            List<Role> allRoles
        )
        {
            var userRoleIds = userRoles
                .Where(ur => ur.UserId == userId && 
                            ur.IsActive && 
                            !ur.IsExpired)
                .Select(ur => ur.RoleId)
                .ToList();

            return allRoles
                .Where(r => userRoleIds.Contains(r.Id) && r.IsActive)
                .ToList();
        }
    }
}
