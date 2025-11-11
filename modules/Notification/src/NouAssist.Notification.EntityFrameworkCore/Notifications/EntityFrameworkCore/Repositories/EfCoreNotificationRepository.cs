using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Notification.Notifications.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.Notification.Notifications.EntityFrameworkCore.Repositories
{
    public class EfCoreNotificationRepository 
        : EfCoreRepository<INotificationsDbContext, Notification, Guid>, INotificationRepository
    {
        public EfCoreNotificationRepository(
            IDbContextProvider<INotificationsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Notification> GetWithDeliveryLogsAsync(Guid id)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Include(n => n.DeliveryLogs)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<List<Notification>> GetListAsync(
            ISpecification<Notification> spec, 
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
                .Where(n => n.UserId == userId && !n.IsRead && !n.IsDeleted)
                .CountAsync();
        }

        public async Task<List<Notification>> GetExpiredNotificationsAsync()
        {
            var now = DateTime.UtcNow;
            var query = await GetQueryableAsync();
            
            return await query
                .Where(n => n.ExpiresAt.HasValue && n.ExpiresAt.Value < now && !n.IsDeleted)
                .ToListAsync();
        }
    }
}
