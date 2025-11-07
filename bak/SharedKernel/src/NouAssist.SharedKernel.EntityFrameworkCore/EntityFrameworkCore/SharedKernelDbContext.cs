using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.SharedKernel.EntityFrameworkCore;

[ConnectionStringName(SharedKernelDbProperties.ConnectionStringName)]
public class SharedKernelDbContext : AbpDbContext<SharedKernelDbContext>, ISharedKernelDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public SharedKernelDbContext(DbContextOptions<SharedKernelDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureSharedKernel();
    }
}
