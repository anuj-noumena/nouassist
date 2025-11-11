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

namespace NouAssist.PersonalDevelopment.Habits.EntityFrameworkCore.Repositories
{
    public class EfCoreMoodTrackingRepository 
        : EfCoreRepository<IHabitsDbContext, MoodTrackingEntry, Guid>, IMoodTrackingRepository
    {
        public EfCoreMoodTrackingRepository(
            IDbContextProvider<IHabitsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<MoodTrackingEntry> GetByOwnerAndDateAsync(
            Guid ownerId,
            DateTime date,
            CancellationToken cancellationToken = default)
        {
            var normalizedDate = new DateTime(date.Year, date.Month, date.Day);
            var query = await GetQueryableAsync();
            
            return await query
                .FirstOrDefaultAsync(
                    e => e.OwnerId == ownerId && e.Date == normalizedDate,
                    cancellationToken);
        }

        public async Task<List<MoodTrackingEntry>> GetByOwnerInPeriodAsync(
            Guid ownerId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            var normalizedStart = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            var normalizedEnd = new DateTime(endDate.Year, endDate.Month, endDate.Day);
            
            var query = await GetQueryableAsync();
            
            return await query
                .Where(e => e.OwnerId == ownerId && 
                           e.Date >= normalizedStart && 
                           e.Date <= normalizedEnd)
                .OrderBy(e => e.Date)
                .ToListAsync(cancellationToken);
        }

        public async Task<double> GetAverageMoodRatingAsync(
            Guid ownerId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            var normalizedStart = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            var normalizedEnd = new DateTime(endDate.Year, endDate.Month, endDate.Day);
            
            var query = await GetQueryableAsync();
            
            var entries = await query
                .Where(e => e.OwnerId == ownerId && 
                           e.Date >= normalizedStart && 
                           e.Date <= normalizedEnd)
                .ToListAsync(cancellationToken);

            return entries.Any() ? entries.Average(e => e.MoodRating) : 0.0;
        }

        public async Task<List<string>> GetPopularTagsAsync(
            Guid ownerId,
            int topCount = 10,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            var entries = await query
                .Where(e => e.OwnerId == ownerId)
                .ToListAsync(cancellationToken);

            var allTags = entries
                .SelectMany(e => e.Tags ?? new List<string>())
                .GroupBy(t => t)
                .OrderByDescending(g => g.Count())
                .Take(topCount)
                .Select(g => g.Key)
                .ToList();

            return allTags;
        }
    }
}
