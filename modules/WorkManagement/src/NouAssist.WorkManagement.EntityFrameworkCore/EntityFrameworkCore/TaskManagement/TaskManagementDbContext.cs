using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CoreProductivity.TaskManagement.EntityFrameworkCore;

[ConnectionStringName(TaskManagementDbProperties.ConnectionStringName)]
public class TaskManagementDbContext : AbpDbContext<TaskManagementDbContext>, ITaskManagementDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureTaskManagement();
    }
}
