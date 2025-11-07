using Microsoft.EntityFrameworkCore;
using NouAssist.Reminders.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Reminders.EntityFrameworkCore
{
    [ConnectionStringName(RemindersDbProperties.ConnectionStringName)]
    public interface IRemindersDbContext : IEfCoreDbContext
    {
        DbSet<Reminder> Reminders { get; }
    }
}
