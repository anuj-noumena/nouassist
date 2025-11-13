using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.Notification.Reminders.EntityFrameworkCore
{
    public static class RemindersDbContextModelCreatingExtensions
    {
        public static void ConfigureReminders(
            this ModelBuilder builder,
            RemindersDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            builder.ApplyConfigurationsFromAssembly(typeof(RemindersDbContext).Assembly);
        }
    }
}
