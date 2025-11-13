using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Infrastructure.Activity.Aggregates;
using NouAssist.Infrastructure.Activity.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.Infrastructure.Activity.EntityFrameworkCore.Repositories
{
    public class EfCoreActivityFeedRepository 
        : EfCoreRepository<IActivityDbContext, ActivityFeed, Guid>, IActivityFeedRepository
    {
        public EfCoreActivityFeedRepository(
            IDbContextProvider<IActivityDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<ActivityFeed>> GetListAsync(
            ISpecification<ActivityFeed> spec, 
            int maxResultCount = 50)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(spec.ToExpression())
                .OrderByDescending(a => a.OccurredAt)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<long> GetCountAsync(ISpecification<ActivityFeed> spec)
        {
            var query = await GetQueryableAsync();
            return await query.Where(spec.ToExpression()).LongCountAsync();
        }

        public async Task<int> GetUnreadCountAsync(Guid userId)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(a => a.ActorId == userId && !a.IsRead)
                .CountAsync();
        }
    }
}
