using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.Notification.Notifications.EntityFrameworkCore
{
    public static class NotificationsDbContextModelCreatingExtensions
    {
        public static void ConfigureNotifications(
            this ModelBuilder builder,
            NotificationsDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            builder.ApplyConfigurationsFromAssembly(typeof(NotificationsDbContext).Assembly);
        }
    }
}
