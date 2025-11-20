using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.SharedKernel.Aggregates;
using NouAssist.SharedKernel.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.SharedKernel.EntityFrameworkCore.Repositories
{
    public class EfCoreLogActivityRepository 
        : EfCoreRepository<ISharedKernelDbContext, LogActivity, Guid>, ILogActivityRepository
    {
        public EfCoreLogActivityRepository(
            IDbContextProvider<ISharedKernelDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<LogActivity>> GetByUserAsync(Guid userId, int maxCount = 100)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.OccurredAt)
                .Take(maxCount)
                .ToListAsync();
        }

        public async Task<List<LogActivity>> GetByEntityAsync(string entityType, Guid entityId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(l => l.EntityType == entityType && l.EntityId == entityId)
                .OrderByDescending(l => l.OccurredAt)
                .ToListAsync();
        }

        public async Task<List<LogActivity>> GetByDateRangeAsync(
            Guid userId,
            DateTime startDate,
            DateTime endDate)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(l => l.UserId == userId && 
                           l.OccurredAt >= startDate && 
                           l.OccurredAt <= endDate)
                .OrderByDescending(l => l.OccurredAt)
                .ToListAsync();
        }
    }
}
