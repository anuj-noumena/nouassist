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
    public class EfCoreTagRepository 
        : EfCoreRepository<ISharedKernelDbContext, Tag, Guid>, ITagRepository
    {
        public EfCoreTagRepository(
            IDbContextProvider<ISharedKernelDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<Tag>> GetByUserAsync(Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(t => t.OwnerId == userId)
                .OrderByDescending(t => t.UsageCount)
                .ThenBy(t => t.Title)
                .ToListAsync();
        }

        public async Task<List<Tag>> GetPopularTagsAsync(Guid userId, int minUsageCount = 10)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(t => t.OwnerId == userId && t.UsageCount >= minUsageCount)
                .OrderByDescending(t => t.UsageCount)
                .ToListAsync();
        }

        public async Task<Tag?> FindByTitleAsync(Guid userId, string title)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .FirstOrDefaultAsync(t => t.OwnerId == userId && t.Title == title);
        }
    }
}
