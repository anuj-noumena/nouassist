using Microsoft.EntityFrameworkCore;
using NouAssist.Infrastructure.Notifications.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Infrastructure.Notifications.EntityFrameworkCore
{
    [ConnectionStringName(NotificationsDbProperties.ConnectionStringName)]
    public interface INotificationsDbContext : IEfCoreDbContext
    {
        DbSet<Notification> Notifications { get; }
        DbSet<NotificationPreference> NotificationPreferences { get; }
    }
}
