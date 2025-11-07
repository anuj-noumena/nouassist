using Microsoft.EntityFrameworkCore;
using NouAssist.ReflectionAndSupport.Reminders.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.ReflectionAndSupport.Reminders.EntityFrameworkCore
{
    [ConnectionStringName(RemindersDbProperties.ConnectionStringName)]
    public interface IRemindersDbContext : IEfCoreDbContext
    {
        DbSet<Reminder> Reminders { get; }
    }
}
