using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.TimeManagement.Calendar.Aggregates;
using NouAssist.TimeManagement.Calendar.Data;
using NouAssist.TimeManagement.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.TimeManagement.Calendar.EntityFrameworkCore.Repositories
{
    public class EfCoreUserAvailabilityRepository 
        : EfCoreRepository<ITimeManagementDbContext, UserAvailability, Guid>, IUserAvailabilityRepository
    {
        public EfCoreUserAvailabilityRepository(
            IDbContextProvider<ITimeManagementDbContext> dbContextProvider)
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
