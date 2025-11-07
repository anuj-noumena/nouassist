using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Team.Data;
using NouAssist.Team.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Team.EntityFrameworkCore.Repositories
{
    public class EfCoreUserRoleRepository 
        : EfCoreRepository<ITeamDbContext, UserRole, Guid>, 
          IUserRoleRepository
    {
        public EfCoreUserRoleRepository(
            IDbContextProvider<ITeamDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<UserRole>> GetUserRolesAsync(Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Where(ur => ur.UserId == userId)
                .OrderBy(ur => ur.AssignedAt)
                .ToListAsync();
        }

        public async Task<List<UserRole>> GetRoleAssignmentsAsync(Guid roleId)
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Where(ur => ur.RoleId == roleId)
                .OrderBy(ur => ur.AssignedAt)
                .ToListAsync();
        }

        public async Task<List<UserRole>> GetActiveUserRolesAsync(Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            var now = DateTime.UtcNow;
            
            return await dbSet
                .Where(ur => ur.UserId == userId && 
                            ur.IsActive &&
                            (!ur.ExpiresAt.HasValue || ur.ExpiresAt.Value > now))
                .OrderBy(ur => ur.AssignedAt)
                .ToListAsync();
        }

        public async Task<bool> UserHasRoleAsync(Guid userId, Guid roleId)
        {
            var dbSet = await GetDbSetAsync();
            var now = DateTime.UtcNow;
            
            return await dbSet
                .AnyAsync(ur => ur.UserId == userId && 
                               ur.RoleId == roleId &&
                               ur.IsActive &&
                               (!ur.ExpiresAt.HasValue || ur.ExpiresAt.Value > now));
        }
    }
}
