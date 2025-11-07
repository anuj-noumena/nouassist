using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Infrastructure.Notifications.Aggregates;
using NouAssist.Infrastructure.Notifications.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Infrastructure.Notifications.Data
{
    public interface INotificationPreferenceRepository : IRepository<NotificationPreference, Guid>
    {
        Task<NotificationPreference> FindByUserAndTypeAsync(Guid userId, NotificationType notificationType);
        Task<List<NotificationPreference>> GetUserPreferencesAsync(Guid userId);
    }
}
