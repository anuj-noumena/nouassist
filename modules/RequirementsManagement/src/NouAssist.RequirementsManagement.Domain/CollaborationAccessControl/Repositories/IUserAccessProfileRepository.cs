using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Aggregates;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Repositories
{
    /// <summary>
    /// Repository for UserAccessProfile aggregate.
    /// </summary>
    public interface IUserAccessProfileRepository : IRepository<UserAccessProfile, Guid>
    {
        /// <summary>
        /// Gets a user access profile by user ID.
        /// </summary>
        Task<UserAccessProfile> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Checks if a user has the specified permission for a resource.
        /// </summary>
        Task<bool> HasPermissionAsync(
            Guid userId,
            PermissionType permissionType,
            TargetResourceType resourceType,
            Guid resourceId,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets all user access profiles with a specific role assigned.
        /// </summary>
        Task<List<UserAccessProfile>> GetByRoleAsync(
            Guid roleId,
            CancellationToken cancellationToken = default);
    }
}
