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
    public class EfCoreFocusSessionRepository 
        : EfCoreRepository<IHabitsDbContext, FocusSession, Guid>, IFocusSessionRepository
    {
        public EfCoreFocusSessionRepository(
            IDbContextProvider<IHabitsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<FocusSession> GetByOwnerAndDateAsync(
            Guid ownerId,
            DateTime date,
            CancellationToken cancellationToken = default)
        {
            var normalizedDate = new DateTime(date.Year, date.Month, date.Day);
            var query = await GetQueryableAsync();
            
            return await query
                .FirstOrDefaultAsync(
                    s => s.OwnerId == ownerId && s.Date == normalizedDate,
                    cancellationToken);
        }

        public async Task<List<FocusSession>> GetByOwnerInPeriodAsync(
            Guid ownerId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            var normalizedStart = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            var normalizedEnd = new DateTime(endDate.Year, endDate.Month, endDate.Day);
            
            var query = await GetQueryableAsync();
            
            return await query
                .Where(s => s.OwnerId == ownerId && 
                           s.Date >= normalizedStart && 
                           s.Date <= normalizedEnd)
                .OrderBy(s => s.Date)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetTotalPomodorosAsync(
            Guid ownerId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            var normalizedStart = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            var normalizedEnd = new DateTime(endDate.Year, endDate.Month, endDate.Day);
            
            var query = await GetQueryableAsync();
            
            return await query
                .Where(s => s.OwnerId == ownerId && 
                           s.Date >= normalizedStart && 
                           s.Date <= normalizedEnd)
                .SumAsync(s => s.CompletedPomodoros, cancellationToken);
        }

        public async Task<int> GetTotalFocusTimeAsync(
            Guid ownerId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            var normalizedStart = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            var normalizedEnd = new DateTime(endDate.Year, endDate.Month, endDate.Day);
            
            var query = await GetQueryableAsync();
            
            return await query
                .Where(s => s.OwnerId == ownerId && 
                           s.Date >= normalizedStart && 
                           s.Date <= normalizedEnd)
                .SumAsync(s => s.TotalFocusTime, cancellationToken);
        }

        public async Task<List<FocusSession>> GetByTaskIdAsync(
            Guid taskId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(s => s.TaskId == taskId)
                .OrderByDescending(s => s.Date)
                .ToListAsync(cancellationToken);
        }
    }
}
