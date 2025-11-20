using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.EntityFrameworkCore;

[ConnectionStringName(RequirementsManagementDbProperties.ConnectionStringName)]
public class RequirementsManagementDbContext : AbpDbContext<RequirementsManagementDbContext>, IRequirementsManagementDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public RequirementsManagementDbContext(DbContextOptions<RequirementsManagementDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureRequirementsManagement();
    }
}
