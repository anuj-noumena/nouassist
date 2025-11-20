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
    public class EfCoreInboxItemRepository 
        : EfCoreRepository<ISharedKernelDbContext, InboxItem, Guid>, IInboxItemRepository
    {
        public EfCoreInboxItemRepository(
            IDbContextProvider<ISharedKernelDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<InboxItem>> GetUnprocessedByUserAsync(Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(i => i.OwnerId == userId && !i.Processed)
                .OrderBy(i => i.CreationTime)
                .ToListAsync();
        }

        public async Task<List<InboxItem>> GetStaleItemsAsync(Guid userId, int daysToConsiderStale = 7)
        {
            var staleDate = DateTime.UtcNow.AddDays(-daysToConsiderStale);
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(i => i.OwnerId == userId && !i.Processed && i.CreationTime < staleDate)
                .OrderBy(i => i.CreationTime)
                .ToListAsync();
        }

        public async Task<int> CountUnprocessedByUserAsync(Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .CountAsync(i => i.OwnerId == userId && !i.Processed);
        }
    }
}
