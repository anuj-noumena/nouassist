using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IdentityAccess.EntityFrameworkCore;

[ConnectionStringName(IdentityAccessDbProperties.ConnectionStringName)]
public class IdentityAccessDbContext : AbpDbContext<IdentityAccessDbContext>, IIdentityAccessDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public IdentityAccessDbContext(DbContextOptions<IdentityAccessDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureIdentityAccess();
    }
}
