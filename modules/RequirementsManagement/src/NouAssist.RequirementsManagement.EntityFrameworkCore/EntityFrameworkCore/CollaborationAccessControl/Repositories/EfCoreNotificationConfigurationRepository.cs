using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Aggregates;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.EntityFrameworkCore.Repositories
{
    public class EfCoreNotificationConfigurationRepository : 
        CollaborationRepositoryBase<NotificationConfiguration>, 
        INotificationConfigurationRepository
    {
        public EfCoreNotificationConfigurationRepository(
            IDbContextProvider<ICollaborationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public override async Task<IQueryable<NotificationConfiguration>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(x => x.SubscriptionRules);
        }

        public async Task<NotificationConfiguration> GetByUserIdAsync(
            Guid userId, 
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            
            return await query
                .Where(c => EF.Property<Guid>(c, "UserId") == userId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<NotificationConfiguration>> GetByMatchingEventAsync(
            NotificationEventType eventType, 
            TargetResourceType resourceType, 
            Guid resourceId, 
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            
            return await query
                .Where(c => c.SubscriptionRules.Any(r => 
                    r.IsEnabled && r.Matches(eventType, resourceType, resourceId)))
                .ToListAsync(cancellationToken);
        }
    }
}
