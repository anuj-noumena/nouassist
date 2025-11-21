using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Entities;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Exceptions;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Repositories;
using Volo.Abp.Domain.Services;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Services
{
    /// <summary>
    /// Domain service for authorization and permission checking.
    /// </summary>
    public class AuthorizationService : DomainService, IAuthorizationService
    {
        private readonly IUserAccessProfileRepository _userAccessProfileRepository;
        private readonly ILogger<AuthorizationService> _logger;

        public AuthorizationService(
            IUserAccessProfileRepository userAccessProfileRepository,
            ILogger<AuthorizationService> logger)
        {
            _userAccessProfileRepository = userAccessProfileRepository;
            _logger = logger;
        }

        /// <summary>
        /// Checks if a user has the specified permission for a resource.
        /// </summary>
        public async Task<bool> HasPermissionAsync(
            Guid userId,
            PermissionType permissionType,
            TargetResourceType resourceType,
            Guid resourceId)
        {
            try
            {
                return await _userAccessProfileRepository.HasPermissionAsync(
                    userId,
                    permissionType,
                    resourceType,
                    resourceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Error checking permission {Permission} for user {UserId} on {ResourceType} with ID {ResourceId}",
                    permissionType, userId, resourceType, resourceId);
                return false;
            }
        }

        /// <summary>
        /// Authorizes a user for an operation, throwing an exception if not authorized.
        /// </summary>
        public async Task AuthorizeAsync(
            Guid userId,
            PermissionType permissionType,
            TargetResourceType resourceType,
            Guid resourceId)
        {
            if (!await HasPermissionAsync(userId, permissionType, resourceType, resourceId))
            {
                _logger.LogWarning(
                    "Permission denied: User {UserId} does not have {Permission} for {ResourceType} with ID {ResourceId}",
                    userId, permissionType, resourceType, resourceId);
                
                throw new PermissionDeniedException(userId, permissionType, resourceType, resourceId);
            }
        }

        /// <summary>
        /// Gets all permissions for a user across all resources.
        /// </summary>
        public async Task<List<Permission>> GetUserPermissionsAsync(Guid userId)
        {
            var profile = await _userAccessProfileRepository.GetByUserIdAsync(userId);
            if (profile == null)
            {
                return new List<Permission>();
            }

            return profile.Roles
                .SelectMany(r => r.Permissions)
                .ToList();
        }
    }
}
