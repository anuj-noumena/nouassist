using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using NouAssist.PersonalDevelopment.Habits.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.PersonalDevelopment.EntityFrameworkCore.Habits.Repositories
{
    public class EfCoreHabitTrackingRepository 
        : EfCoreRepository<IPersonalDevelopmentDbContext, HabitTrackingEntry, Guid>, IHabitTrackingRepository
    {
        public EfCoreHabitTrackingRepository(
            IDbContextProvider<IPersonalDevelopmentDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<HabitTrackingEntry> GetByHabitAndDateAsync(
            Guid habitId,
            DateTime date,
            CancellationToken cancellationToken = default)
        {
            var normalizedDate = new DateTime(date.Year, date.Month, date.Day);
            var query = await GetQueryableAsync();
            
            return await query
                .FirstOrDefaultAsync(
                    e => e.HabitId == habitId && e.LogDate == normalizedDate, 
                    cancellationToken);
        }

        public async Task<List<HabitTrackingEntry>> GetHistoryAsync(
            Guid habitId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            query = query.Where(e => e.HabitId == habitId);

            if (startDate.HasValue)
            {
                var normalizedStart = new DateTime(startDate.Value.Year, startDate.Value.Month, startDate.Value.Day);
                query = query.Where(e => e.LogDate >= normalizedStart);
            }

            if (endDate.HasValue)
            {
                var normalizedEnd = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day);
                query = query.Where(e => e.LogDate <= normalizedEnd);
            }

            return await query
                .OrderByDescending(e => e.LogDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<HabitTrackingEntry>> GetCompletedInPeriodAsync(
            Guid habitId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            var normalizedStart = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            var normalizedEnd = new DateTime(endDate.Year, endDate.Month, endDate.Day);
            
            var query = await GetQueryableAsync();
            
            return await query
                .Where(e => e.HabitId == habitId && 
                           e.Completed && 
                           e.LogDate >= normalizedStart && 
                           e.LogDate <= normalizedEnd)
                .OrderBy(e => e.LogDate)
                .ToListAsync(cancellationToken);
        }
    }
}
