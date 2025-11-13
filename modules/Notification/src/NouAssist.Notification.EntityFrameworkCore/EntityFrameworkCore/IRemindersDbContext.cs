using Microsoft.EntityFrameworkCore;
using NouAssist.Notification.Reminders.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Notification.Reminders.EntityFrameworkCore
{
    [ConnectionStringName(RemindersDbProperties.ConnectionStringName)]
    public interface IRemindersDbContext : IEfCoreDbContext
    {
        DbSet<Reminder> Reminders { get; }
    }
}
