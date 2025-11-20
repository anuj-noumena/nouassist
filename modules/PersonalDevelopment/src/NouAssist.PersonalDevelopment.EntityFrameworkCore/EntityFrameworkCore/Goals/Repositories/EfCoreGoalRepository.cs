using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.PersonalDevelopment.Goals.Aggregates;
using NouAssist.PersonalDevelopment.Goals.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.PersonalDevelopment.EntityFrameworkCore.Goals.Repositories
{
    public class EfCoreGoalRepository 
        : EfCoreRepository<IPersonalDevelopmentDbContext, Goal, Guid>, IGoalRepository
    {
        public EfCoreGoalRepository(
            IDbContextProvider<IPersonalDevelopmentDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Goal> GetWithMilestonesAsync(Guid id)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Include(g => g.Milestones)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<List<Goal>> GetListAsync(ISpecification<Goal> spec)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(spec.ToExpression())
                .OrderByDescending(g => g.CreationTime)
                .ToListAsync();
        }

        public async Task<List<Goal>> GetByUserAsync(Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(g => g.OwnerId == userId)
                .OrderByDescending(g => g.CreationTime)
                .ToListAsync();
        }

        public async Task<List<Goal>> GetByAreaAsync(Guid areaId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(g => g.AreaId == areaId)
                .OrderByDescending(g => g.CreationTime)
                .ToListAsync();
        }

        public async Task<int> CountActiveByUserAsync(Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .CountAsync(g => g.OwnerId == userId && g.IsActive);
        }
    }
}
