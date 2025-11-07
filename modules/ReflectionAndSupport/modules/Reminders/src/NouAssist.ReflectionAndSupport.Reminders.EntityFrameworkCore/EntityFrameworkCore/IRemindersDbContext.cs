using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.ReflectionAndSupport.Reminders.EntityFrameworkCore;

[ConnectionStringName(RemindersDbProperties.ConnectionStringName)]
public interface IRemindersDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
