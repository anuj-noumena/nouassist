using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Entities;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Services
{
    /// <summary>
    /// Interface for authorization service.
    /// </summary>
    public interface IAuthorizationService
    {
        Task<bool> HasPermissionAsync(
            Guid userId,
            PermissionType permissionType,
            TargetResourceType resourceType,
            Guid resourceId);
            
        Task AuthorizeAsync(
            Guid userId,
            PermissionType permissionType,
            TargetResourceType resourceType,
            Guid resourceId);
            
        Task<List<Permission>> GetUserPermissionsAsync(Guid userId);
    }
}
