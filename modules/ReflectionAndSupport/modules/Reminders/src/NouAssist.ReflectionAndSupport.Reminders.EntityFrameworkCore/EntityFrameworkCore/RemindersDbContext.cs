using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.ReflectionAndSupport.Reminders.EntityFrameworkCore;

[ConnectionStringName(RemindersDbProperties.ConnectionStringName)]
public class RemindersDbContext : AbpDbContext<RemindersDbContext>, IRemindersDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public RemindersDbContext(DbContextOptions<RemindersDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureReminders();
    }
}
