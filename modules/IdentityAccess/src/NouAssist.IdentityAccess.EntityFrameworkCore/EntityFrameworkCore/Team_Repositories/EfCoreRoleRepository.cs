using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.IdentityAccess.Team.Aggregates;
using NouAssist.IdentityAccess.Team.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.IdentityAccess.Team.EntityFrameworkCore.Repositories
{
    public class EfCoreRoleRepository 
        : EfCoreRepository<ITeamDbContext, Role, Guid>, 
          IRoleRepository
    {
        public EfCoreRoleRepository(
            IDbContextProvider<ITeamDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<Role>> GetActiveRolesAsync()
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Where(r => r.IsActive)
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        public async Task<List<Role>> GetSystemRolesAsync()
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Where(r => r.IsSystem && r.IsActive)
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        public async Task<List<Role>> GetUserRolesAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            
            var roleIds = await dbContext.UserRoles
                .Where(ur => ur.UserId == userId && 
                            ur.IsActive &&
                            (!ur.ExpiresAt.HasValue || ur.ExpiresAt.Value > DateTime.UtcNow))
                .Select(ur => ur.RoleId)
                .ToListAsync();

            return await dbContext.Roles
                .Where(r => roleIds.Contains(r.Id) && r.IsActive)
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        public async Task<Role> GetByNameAsync(string name)
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task<List<Role>> GetListAsync(
            ISpecification<Role> spec)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(spec.ToExpression())
                .ToListAsync();
        }
    }
}
