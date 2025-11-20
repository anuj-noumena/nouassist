using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Infrastructure.Notifications.Aggregates;
using NouAssist.Infrastructure.Notifications.Data;
using NouAssist.Notification.Notifications.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Infrastructure.EntityFrameworkCore.Notifications.Repositories
{
    public class EfCoreNotificationPreferenceRepository 
        : EfCoreRepository<IInfrastructureDbContext, NotificationPreference, Guid>, INotificationPreferenceRepository
    {
        public EfCoreNotificationPreferenceRepository(
            IDbContextProvider<IInfrastructureDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<NotificationPreference> FindByUserAndTypeAsync(
            Guid userId, 
            NotificationType notificationType)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .FirstOrDefaultAsync(p => p.UserId == userId && p.NotificationType == notificationType);
        }

        public async Task<List<NotificationPreference>> GetUserPreferencesAsync(Guid userId)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }
    }
}
