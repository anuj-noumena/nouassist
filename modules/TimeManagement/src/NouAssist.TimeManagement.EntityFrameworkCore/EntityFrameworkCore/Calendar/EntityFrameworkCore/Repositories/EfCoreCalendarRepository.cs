using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.TimeManagement.Calendar.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.TimeManagement.EntityFrameworkCore.Calendar.EntityFrameworkCore.Repositories
{
    public class EfCoreCalendarRepository 
        : EfCoreRepository<ITimeManagementDbContext, TimeManagement.Calendar.Aggregates.Calendar, Guid>, ICalendarRepository
    {
        public EfCoreCalendarRepository(
            IDbContextProvider<ITimeManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<TimeManagement.Calendar.Aggregates.Calendar> GetDefaultCalendarAsync(
            Guid ownerId, 
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(c => c.OwnerId == ownerId && c.IsDefault && c.IsActive)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<TimeManagement.Calendar.Aggregates.Calendar>> GetActiveCalendarsAsync(
            Guid ownerId, 
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(c => c.OwnerId == ownerId && c.IsActive)
                .OrderBy(c => c.Title)
                .ToListAsync(cancellationToken);
        }

        public async Task<TimeManagement.Calendar.Aggregates.Calendar> GetWithSharesAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Include(c => c.Shares)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public override async Task<IQueryable<TimeManagement.Calendar.Aggregates.Calendar>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(c => c.Shares);
        }
    }
}
