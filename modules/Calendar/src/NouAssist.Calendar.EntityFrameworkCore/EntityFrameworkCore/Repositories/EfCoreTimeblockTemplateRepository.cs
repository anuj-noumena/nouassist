using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Calendar.Aggregates;
using NouAssist.Calendar.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Calendar.EntityFrameworkCore.Repositories
{
    public class EfCoreTimeblockTemplateRepository 
        : EfCoreRepository<ICalendarDbContext, TimeblockTemplate, Guid>, ITimeblockTemplateRepository
    {
        public EfCoreTimeblockTemplateRepository(
            IDbContextProvider<ICalendarDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<TimeblockTemplate>> GetActiveTemplatesAsync(
            Guid ownerId, 
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(t => t.OwnerId == ownerId && t.IsActive)
                .OrderBy(t => t.Title)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TimeblockTemplate>> GetTemplatesForDayAsync(
            Guid ownerId,
            int dayOfWeek,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(t => t.OwnerId == ownerId && 
                           t.IsActive && 
                           t.DaysOfWeek.Contains(dayOfWeek))
                .ToListAsync(cancellationToken);
        }
    }
}
