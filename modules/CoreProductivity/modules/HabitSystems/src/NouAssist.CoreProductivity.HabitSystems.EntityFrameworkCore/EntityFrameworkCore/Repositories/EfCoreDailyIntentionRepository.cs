using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.CoreProductivity.HabitSystems.Aggregates;
using NouAssist.CoreProductivity.HabitSystems.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CoreProductivity.HabitSystems.EntityFrameworkCore.Repositories
{
    public class EfCoreDailyIntentionRepository 
        : EfCoreRepository<IHabitsDbContext, DailyIntention, Guid>, IDailyIntentionRepository
    {
        public EfCoreDailyIntentionRepository(
            IDbContextProvider<IHabitsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<DailyIntention> GetByOwnerAndDateAsync(
            Guid ownerId,
            DateTime date,
            CancellationToken cancellationToken = default)
        {
            var normalizedDate = new DateTime(date.Year, date.Month, date.Day);
            var query = await GetQueryableAsync();
            
            return await query
                .FirstOrDefaultAsync(
                    i => i.OwnerId == ownerId && i.Date == normalizedDate,
                    cancellationToken);
        }

        public async Task<List<DailyIntention>> GetByOwnerInPeriodAsync(
            Guid ownerId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            var normalizedStart = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            var normalizedEnd = new DateTime(endDate.Year, endDate.Month, endDate.Day);
            
            var query = await GetQueryableAsync();
            
            return await query
                .Where(i => i.OwnerId == ownerId && 
                           i.Date >= normalizedStart && 
                           i.Date <= normalizedEnd)
                .OrderBy(i => i.Date)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasIntentionsForDateAsync(
            Guid ownerId,
            DateTime date,
            CancellationToken cancellationToken = default)
        {
            var normalizedDate = new DateTime(date.Year, date.Month, date.Day);
            var query = await GetQueryableAsync();
            
            var intention = await query
                .FirstOrDefaultAsync(
                    i => i.OwnerId == ownerId && i.Date == normalizedDate,
                    cancellationToken);

            return intention != null && intention.HasIntentions;
        }
    }
}
