using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Infrastructure.Notifications.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.Infrastructure.EntityFrameworkCore.Notifications.Repositories
{
    public class EfCoreNotificationRepository 
        : EfCoreRepository<IInfrastructureDbContext, Infrastructure.Notifications.Aggregates.Notification, Guid>, INotificationRepository
    {
        public EfCoreNotificationRepository(
            IDbContextProvider<IInfrastructureDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Infrastructure.Notifications.Aggregates.Notification> GetWithDeliveryLogsAsync(Guid id)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Include(n => n.DeliveryLogs)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<List<Infrastructure.Notifications.Aggregates.Notification>> GetListAsync(
            ISpecification<Infrastructure.Notifications.Aggregates.Notification> spec, 
            int maxResultCount = 50)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(spec.ToExpression())
                .OrderByDescending(n => n.CreationTime)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(Guid userId)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(n => n.UserId == userId && !n.IsRead && !n.IsDismissed)
                .CountAsync();
        }

        public async Task<List<Infrastructure.Notifications.Aggregates.Notification>> GetExpiredNotificationsAsync()
        {
            var now = DateTime.UtcNow;
            var query = await GetQueryableAsync();
            
            return await query
                .Where(n => n.ExpiresAt.HasValue && n.ExpiresAt.Value < now && !n.IsDismissed)
                .ToListAsync();
        }
    }
}
