using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.TimeManagement.Calendar.Aggregates;
using NouAssist.TimeManagement.Calendar.Data;
using NouAssist.TimeManagement.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.TimeManagement.Calendar.EntityFrameworkCore.Repositories
{
    public class EfCoreTimeblockTemplateRepository 
        : EfCoreRepository<ITimeManagementDbContext, TimeblockTemplate, Guid>, ITimeblockTemplateRepository
    {
        public EfCoreTimeblockTemplateRepository(
            IDbContextProvider<ITimeManagementDbContext> dbContextProvider)
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
