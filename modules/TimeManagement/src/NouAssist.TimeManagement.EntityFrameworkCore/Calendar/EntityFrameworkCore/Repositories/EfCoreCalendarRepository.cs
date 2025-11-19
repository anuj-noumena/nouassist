using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.TimeManagement.Calendar.Data;
using NouAssist.TimeManagement.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.TimeManagement.Calendar.EntityFrameworkCore.Repositories
{
    public class EfCoreCalendarRepository 
        : EfCoreRepository<ITimeManagementDbContext, Aggregates.Calendar, Guid>, ICalendarRepository
    {
        public EfCoreCalendarRepository(
            IDbContextProvider<ITimeManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Aggregates.Calendar> GetDefaultCalendarAsync(
            Guid ownerId, 
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(c => c.OwnerId == ownerId && c.IsDefault && c.IsActive)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Aggregates.Calendar>> GetActiveCalendarsAsync(
            Guid ownerId, 
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(c => c.OwnerId == ownerId && c.IsActive)
                .OrderBy(c => c.Title)
                .ToListAsync(cancellationToken);
        }

        public async Task<Aggregates.Calendar> GetWithSharesAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Include(c => c.Shares)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public override async Task<IQueryable<Aggregates.Calendar>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(c => c.Shares);
        }
    }
}
