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
    public class EfCoreDailyReflectionRepository 
        : EfCoreRepository<IHabitsDbContext, DailyReflection, Guid>, IDailyReflectionRepository
    {
        public EfCoreDailyReflectionRepository(
            IDbContextProvider<IHabitsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<DailyReflection> GetByOwnerAndDateAsync(
            Guid ownerId,
            DateTime date,
            CancellationToken cancellationToken = default)
        {
            var normalizedDate = new DateTime(date.Year, date.Month, date.Day);
            var query = await GetQueryableAsync();
            
            return await query
                .FirstOrDefaultAsync(
                    r => r.OwnerId == ownerId && r.Date == normalizedDate,
                    cancellationToken);
        }

        public async Task<List<DailyReflection>> GetByOwnerInPeriodAsync(
            Guid ownerId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            var normalizedStart = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            var normalizedEnd = new DateTime(endDate.Year, endDate.Month, endDate.Day);
            
            var query = await GetQueryableAsync();
            
            return await query
                .Where(r => r.OwnerId == ownerId && 
                           r.Date >= normalizedStart && 
                           r.Date <= normalizedEnd)
                .OrderBy(r => r.Date)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<DailyReflection>> GetCompletedReflectionsAsync(
            Guid ownerId,
            int take = 10,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(r => r.OwnerId == ownerId && r.Rating.HasValue)
                .OrderByDescending(r => r.Date)
                .Take(take)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasReflectionForDateAsync(
            Guid ownerId,
            DateTime date,
            CancellationToken cancellationToken = default)
        {
            var normalizedDate = new DateTime(date.Year, date.Month, date.Day);
            var query = await GetQueryableAsync();
            
            return await query
                .AnyAsync(
                    r => r.OwnerId == ownerId && r.Date == normalizedDate,
                    cancellationToken);
        }
    }
}
