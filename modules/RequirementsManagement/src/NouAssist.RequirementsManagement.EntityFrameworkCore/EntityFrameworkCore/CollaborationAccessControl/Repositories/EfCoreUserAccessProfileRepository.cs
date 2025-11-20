using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Aggregates;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Entities;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.EntityFrameworkCore.Repositories
{
    public class EfCoreUserAccessProfileRepository : 
        CollaborationRepositoryBase<UserAccessProfile>, 
        IUserAccessProfileRepository
    {
        public EfCoreUserAccessProfileRepository(
            IDbContextProvider<ICollaborationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public override async Task<IQueryable<UserAccessProfile>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(x => x.Roles)
                .ThenInclude(r => r.Permissions);
        }

        public async Task<UserAccessProfile> GetByUserIdAsync(
            Guid userId, 
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            
            return await query
                .Where(p => EF.Property<Guid>(p, "UserId") == userId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> HasPermissionAsync(
            Guid userId, 
            PermissionType permissionType, 
            TargetResourceType resourceType, 
            Guid resourceId, 
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            
            return await query
                .Where(p => EF.Property<Guid>(p, "UserId") == userId)
                .AnyAsync(p => p.HasPermission(permissionType, resourceType, resourceId), 
                    cancellationToken);
        }

        public async Task<List<UserAccessProfile>> GetByRoleAsync(
            Guid roleId, 
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            
            return await query
                .Where(p => p.Roles.Any(r => r.Id == roleId))
                .ToListAsync(cancellationToken);
        }
    }
}
