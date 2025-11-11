using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement;

[ConnectionStringName(TasksDbProperties.ConnectionStringName)]
public class TasksDbContext : AbpDbContext<TasksDbContext>, ITasksDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public TasksDbContext(DbContextOptions<TasksDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureTasks();
    }
}
