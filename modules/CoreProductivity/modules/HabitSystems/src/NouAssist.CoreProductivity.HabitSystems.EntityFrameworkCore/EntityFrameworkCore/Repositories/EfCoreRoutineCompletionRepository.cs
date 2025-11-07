using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.CoreProductivity.HabitSystems.Aggregates;
using NouAssist.CoreProductivity.HabitSystems.Data;
using NouAssist.CoreProductivity.HabitSystems.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CoreProductivity.HabitSystems.EntityFrameworkCore.Repositories
{
    public class EfCoreRoutineCompletionRepository 
        : EfCoreRepository<IHabitsDbContext, RoutineCompletion, Guid>, IRoutineCompletionRepository
    {
        public EfCoreRoutineCompletionRepository(
            IDbContextProvider<IHabitsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<RoutineCompletion> GetByRoutineAndDateAsync(
            Guid routineId,
            DateTime date,
            CancellationToken cancellationToken = default)
        {
            var normalizedDate = new DateTime(date.Year, date.Month, date.Day);
            var query = await GetQueryableAsync();
            
            return await query
                .Include(c => c.CompletedSteps)
                .FirstOrDefaultAsync(
                    c => c.RoutineId == routineId && c.LogDate == normalizedDate,
                    cancellationToken);
        }

        public async Task<List<RoutineCompletion>> GetByRoutineInPeriodAsync(
            Guid routineId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            var normalizedStart = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            var normalizedEnd = new DateTime(endDate.Year, endDate.Month, endDate.Day);
            
            var query = await GetQueryableAsync();
            
            return await query
                .Include(c => c.CompletedSteps)
                .Where(c => c.RoutineId == routineId && 
                           c.LogDate >= normalizedStart && 
                           c.LogDate <= normalizedEnd)
                .OrderBy(c => c.LogDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<RoutineCompletion>> GetByOwnerAndDateAsync(
            Guid ownerId,
            DateTime date,
            CancellationToken cancellationToken = default)
        {
            var normalizedDate = new DateTime(date.Year, date.Month, date.Day);
            var query = await GetQueryableAsync();
            
            return await query
                .Include(c => c.CompletedSteps)
                .Where(c => c.OwnerId == ownerId && c.LogDate == normalizedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<double> GetCompletionRateAsync(
            Guid routineId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            var normalizedStart = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            var normalizedEnd = new DateTime(endDate.Year, endDate.Month, endDate.Day);
            
            var query = await GetQueryableAsync();
            
            var completions = await query
                .Where(c => c.RoutineId == routineId && 
                           c.LogDate >= normalizedStart && 
                           c.LogDate <= normalizedEnd)
                .ToListAsync(cancellationToken);

            if (!completions.Any())
            {
                return 0.0;
            }

            var completed = completions.Count(c => c.Status == RoutineCompletionStatus.Completed);
            return (double)completed / completions.Count;
        }

        public async Task<List<RoutineCompletion>> GetByStatusAsync(
            Guid ownerId,
            RoutineCompletionStatus status,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            query = query.Where(c => c.OwnerId == ownerId && c.Status == status);

            if (startDate.HasValue)
            {
                var normalizedStart = new DateTime(startDate.Value.Year, startDate.Value.Month, startDate.Value.Day);
                query = query.Where(c => c.LogDate >= normalizedStart);
            }

            if (endDate.HasValue)
            {
                var normalizedEnd = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day);
                query = query.Where(c => c.LogDate <= normalizedEnd);
            }

            return await query
                .Include(c => c.CompletedSteps)
                .OrderByDescending(c => c.LogDate)
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<RoutineCompletion>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(c => c.CompletedSteps);
        }
    }
}
