using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.EntityFrameworkCore;

[ConnectionStringName(BusinessOperationsDbProperties.ConnectionStringName)]
public class BusinessOperationsDbContext : AbpDbContext<BusinessOperationsDbContext>, IBusinessOperationsDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public BusinessOperationsDbContext(DbContextOptions<BusinessOperationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureBusinessOperations();
    }
}
