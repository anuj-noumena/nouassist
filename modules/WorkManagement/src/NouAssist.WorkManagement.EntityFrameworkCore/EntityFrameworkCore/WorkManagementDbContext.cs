using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore;

[ConnectionStringName(WorkManagementDbProperties.ConnectionStringName)]
public class WorkManagementDbContext : AbpDbContext<WorkManagementDbContext>, IWorkManagementDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public WorkManagementDbContext(DbContextOptions<WorkManagementDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureWorkManagement();
    }
}
