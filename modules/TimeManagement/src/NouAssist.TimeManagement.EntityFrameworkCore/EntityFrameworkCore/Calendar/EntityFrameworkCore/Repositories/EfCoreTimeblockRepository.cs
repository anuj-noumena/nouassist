using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.TimeManagement.Calendar.Aggregates;
using NouAssist.TimeManagement.Calendar.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.TimeManagement.EntityFrameworkCore.Calendar.EntityFrameworkCore.Repositories
{
    public class EfCoreTimeblockRepository 
        : EfCoreRepository<ITimeManagementDbContext, Timeblock, Guid>, ITimeblockRepository
    {
        public EfCoreTimeblockRepository(
            IDbContextProvider<ITimeManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Timeblock> GetWithTasksAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Include(tb => tb.Tasks)
                .FirstOrDefaultAsync(tb => tb.Id == id, cancellationToken);
        }

        public async Task<List<Timeblock>> GetTimeblocksInRangeAsync(
            Guid ownerId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(tb => tb.OwnerId == ownerId &&
                            tb.StartAt < endDate &&
                            tb.EndAt > startDate)
                .OrderBy(tb => tb.StartAt)
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<Timeblock>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(tb => tb.Tasks);
        }
    }
}
