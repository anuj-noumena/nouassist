using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.EntityFrameworkCore;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Entities;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories
{
    public class InteractionLogRepository : EfCoreRepository<CRMDbContext, InteractionLog, Guid>, IInteractionLogRepository
    {
        public InteractionLogRepository(IDbContextProvider<CRMDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<InteractionLog>> GetForEntityAsync(
            Guid entityId, 
            string entityType, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.InteractionLogs
                .Where(i => i.EntityId == entityId && i.EntityType == entityType)
                .OrderBy(sorting ?? nameof(InteractionLog.CreationTime) + " desc")
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<InteractionLog>> GetByInteractionTypeAsync(
            InteractionType type, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            // This is a bit inefficient as we have to join with CommunicationRecords
            // A better approach might be to denormalize this data in a real system
            return await dbContext.InteractionLogs
                .Where(i => i.CommunicationRecords.Any(cr => cr.Type == type))
                .OrderBy(sorting ?? nameof(InteractionLog.CreationTime) + " desc")
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<InteractionLog>> GetWithPendingFollowUpsAsync(
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var now = DateTime.UtcNow;
            
            return await dbContext.InteractionLogs
                .Where(i => i.FollowUpTasks.Any(ft => 
                    ft.Status == FollowUpStatus.Pending && 
                    ft.DueDate <= now))
                .OrderBy(sorting ?? "FollowUpTasks.DueDate")
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<InteractionLog>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(i => i.CommunicationRecords)
                .Include(i => i.FollowUpTasks);
        }
    }
}
