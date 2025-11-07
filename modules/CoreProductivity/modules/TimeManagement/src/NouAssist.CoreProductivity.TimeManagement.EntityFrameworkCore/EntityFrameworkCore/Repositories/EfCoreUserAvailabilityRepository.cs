using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.CoreProductivity.TimeManagement.Aggregates;
using NouAssist.CoreProductivity.TimeManagement.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CoreProductivity.TimeManagement.EntityFrameworkCore.Repositories
{
    public class EfCoreUserAvailabilityRepository 
        : EfCoreRepository<ICalendarDbContext, UserAvailability, Guid>, IUserAvailabilityRepository
    {
        public EfCoreUserAvailabilityRepository(
            IDbContextProvider<ICalendarDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<UserAvailability> GetByOwnerIdAsync(
            Guid ownerId, 
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Include(ua => ua.WeeklySlots)
                .Include(ua => ua.DateOverrides)
                .FirstOrDefaultAsync(ua => ua.OwnerId == ownerId, cancellationToken);
        }

        public override async Task<IQueryable<UserAvailability>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(ua => ua.WeeklySlots)
                .Include(ua => ua.DateOverrides);
        }
    }
}
