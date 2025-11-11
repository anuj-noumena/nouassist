using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Habits.Aggregates;
using NouAssist.Habits.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Habits.EntityFrameworkCore.Repositories
{
    public class EfCoreEnergyTrackingRepository 
        : EfCoreRepository<IHabitsDbContext, EnergyTrackingEntry, Guid>, IEnergyTrackingRepository
    {
        public EfCoreEnergyTrackingRepository(
            IDbContextProvider<IHabitsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<EnergyTrackingEntry> GetByOwnerAndDateAsync(
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

        public async Task<List<EnergyTrackingEntry>> GetByOwnerInPeriodAsync(
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

        public async Task<double> GetAverageEnergyLevelAsync(
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

            return entries.Any() ? entries.Average(e => e.EnergyLevel) : 0.0;
        }
    }
}
