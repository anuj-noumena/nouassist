using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Calendar.Aggregates;
using NouAssist.Calendar.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Calendar.EntityFrameworkCore.Repositories
{
    public class EfCoreSchedulingRequestRepository 
        : EfCoreRepository<ICalendarDbContext, SchedulingRequest, Guid>, ISchedulingRequestRepository
    {
        public EfCoreSchedulingRequestRepository(
            IDbContextProvider<ICalendarDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<SchedulingRequest> GetWithSlotsAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Include(sr => sr.Slots)
                .FirstOrDefaultAsync(sr => sr.Id == id, cancellationToken);
        }

        public override async Task<IQueryable<SchedulingRequest>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(sr => sr.Slots);
        }
    }
}
