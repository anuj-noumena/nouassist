using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore;

[ConnectionStringName(FinancialManagementDbProperties.ConnectionStringName)]
public class FinancialManagementDbContext : AbpDbContext<FinancialManagementDbContext>, IFinancialManagementDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public FinancialManagementDbContext(DbContextOptions<FinancialManagementDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureFinancialManagement();
    }
}
