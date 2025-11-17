using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Data;
using NouAssist.WorkManagement.GTD.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore.GTD.Repositories
{
    public class EfCoreTicklerRepository
        : EfCoreRepository<IWorkManagementDbContext, TicklerItem, Guid>, ITicklerRepository
    {
        public EfCoreTicklerRepository(
            IDbContextProvider<IWorkManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<TicklerItem>> GetDueTicklersAsync(Guid userId, DateTime currentDate)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(t => t.OwnerId == userId &&
                            t.TickleDate <= currentDate &&
                            t.Status == TicklerStatus.Scheduled)
                .OrderBy(t => t.TickleDate)
                .ToListAsync();
        }

        public async Task<List<TicklerItem>> GetSurfacedTicklersAsync(Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(t => t.OwnerId == userId && t.Status == TicklerStatus.Surfaced)
                .OrderBy(t => t.SurfacedAt)
                .ToListAsync();
        }

        public async Task<List<TicklerItem>> GetByDateRangeAsync(
            Guid userId,
            DateTime startDate,
            DateTime endDate)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(t => t.OwnerId == userId &&
                            t.TickleDate >= startDate &&
                            t.TickleDate <= endDate)
                .OrderBy(t => t.TickleDate)
                .ToListAsync();
        }
    }
}