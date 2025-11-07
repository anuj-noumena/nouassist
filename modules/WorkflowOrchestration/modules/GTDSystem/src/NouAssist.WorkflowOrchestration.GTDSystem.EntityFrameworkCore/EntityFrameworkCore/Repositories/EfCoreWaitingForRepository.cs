using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkflowOrchestration.GTDSystem.Aggregates;
using NouAssist.WorkflowOrchestration.GTDSystem.Data;
using NouAssist.WorkflowOrchestration.GTDSystem.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkflowOrchestration.GTDSystem.EntityFrameworkCore.Repositories
{
    public class EfCoreWaitingForRepository 
        : EfCoreRepository<IGtdDbContext, WaitingFor, Guid>, IWaitingForRepository
    {
        public EfCoreWaitingForRepository(
            IDbContextProvider<IGtdDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<WaitingFor>> GetActiveByUserAsync(Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(w => w.OwnerId == userId && w.Status == WaitingForStatus.Active)
                .OrderBy(w => w.CreationTime)
                .ToListAsync();
        }

        public async Task<List<WaitingFor>> GetFollowUpDueAsync(Guid userId, DateTime currentDate)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(w => w.OwnerId == userId && 
                           w.Status == WaitingForStatus.Active &&
                           w.FollowUpDate.HasValue &&
                           w.FollowUpDate.Value <= currentDate)
                .OrderBy(w => w.FollowUpDate)
                .ToListAsync();
        }
    }
}
