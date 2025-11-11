using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Meetings.Aggregates;
using NouAssist.Meetings.Data;
using NouAssist.Meetings.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.Meetings.EntityFrameworkCore.Repositories
{
    public class EfCoreMeetingRepository 
        : EfCoreRepository<IMeetingsDbContext, Meeting, Guid>, 
          IMeetingRepository
    {
        public EfCoreMeetingRepository(
            IDbContextProvider<IMeetingsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Meeting> GetWithAttendeesAsync(Guid id)
        {
            // All data is in the same table, so just get the entity
            return await GetAsync(id);
        }

        public async Task<Meeting> GetWithFullDetailsAsync(Guid id)
        {
            // All data is in the same table (JSON columns), so just get the entity
            return await GetAsync(id);
        }

        public async Task<List<Meeting>> GetUpcomingMeetingsAsync(Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            var now = DateTime.UtcNow;
            
            // Note: Querying JSON columns for attendees is database-specific
            // For PostgreSQL with jsonb, you can use EF.Functions.JsonContains
            return await dbSet
                .Where(m => m.StartAt > now && 
                           !m.IsCancelled &&
                           m.Status == MeetingStatus.Scheduled)
                .OrderBy(m => m.StartAt)
                .ToListAsync();
        }

        public async Task<List<Meeting>> GetMeetingsForDateAsync(
            DateTime date, 
            Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);
            
            return await dbSet
                .Where(m => m.StartAt >= startOfDay && 
                           m.StartAt < endOfDay &&
                           !m.IsCancelled)
                .OrderBy(m => m.StartAt)
                .ToListAsync();
        }

        public async Task<List<Meeting>> GetMeetingsByProjectAsync(Guid projectId)
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Where(m => m.ProjectId == projectId)
                .OrderByDescending(m => m.StartAt)
                .ToListAsync();
        }

        public async Task<List<Meeting>> GetRecurringSeriesAsync(Guid seriesId)
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Where(m => m.RecurringSeriesId == seriesId)
                .OrderBy(m => m.StartAt)
                .ToListAsync();
        }

        public async Task<List<Meeting>> GetListAsync(
            ISpecification<Meeting> spec)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(spec.ToExpression())
                .ToListAsync();
        }
    }
}
