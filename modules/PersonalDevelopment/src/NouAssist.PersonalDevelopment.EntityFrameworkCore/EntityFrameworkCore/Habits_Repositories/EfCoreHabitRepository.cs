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
using Volo.Abp.Specifications;
using DayOfWeek = NouAssist.PersonalDevelopment.Habits.Enums.DayOfWeek;

namespace NouAssist.PersonalDevelopment.Habits.EntityFrameworkCore.Repositories
{
    public class EfCoreHabitRepository 
        : EfCoreRepository<IHabitsDbContext, Habit, Guid>, IHabitRepository
    {
        public EfCoreHabitRepository(
            IDbContextProvider<IHabitsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Habit> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Include(h => h.Cues)
                .Include(h => h.Responses)
                .Include(h => h.Rewards)
                .Include(h => h.Strategies)
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
        }

        public async Task<List<Habit>> GetActiveHabitsAsync(
            Guid ownerId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(h => h.OwnerId == ownerId && h.IsActive)
                .OrderBy(h => h.Title)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Habit>> GetScheduledForDateAsync(
            Guid ownerId,
            DateTime date,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            // For habits without schedule or with matching day
            var dayOfWeek = (DayOfWeek)((int)date.DayOfWeek == 0 ? 7 : (int)date.DayOfWeek);
            
            return await query
                .Where(h => h.OwnerId == ownerId && h.IsActive)
                .Where(h => !h.ScheduledDays.Any() || h.ScheduledDays.Contains(dayOfWeek))
                .OrderBy(h => h.Title)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Habit>> GetListAsync(
            ISpecification<Habit> spec,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(spec.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<Habit>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(h => h.Cues)
                .Include(h => h.Responses)
                .Include(h => h.Rewards)
                .Include(h => h.Strategies);
        }
    }
}
