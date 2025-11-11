using Microsoft.EntityFrameworkCore;
using NouAssist.Notification.Reminders.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Notification.Reminders.EntityFrameworkCore
{
    [ConnectionStringName(RemindersDbProperties.ConnectionStringName)]
    public class RemindersDbContext : AbpDbContext<RemindersDbContext>, IRemindersDbContext
    {
        public DbSet<Reminder> Reminders { get; set; }

        public RemindersDbContext(DbContextOptions<RemindersDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureReminders(this);
        }
    }
}
