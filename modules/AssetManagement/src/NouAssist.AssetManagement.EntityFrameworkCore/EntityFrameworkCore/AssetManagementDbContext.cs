using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.AssetManagement.EntityFrameworkCore;

[ConnectionStringName(AssetManagementDbProperties.ConnectionStringName)]
public class AssetManagementDbContext : AbpDbContext<AssetManagementDbContext>, IAssetManagementDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public AssetManagementDbContext(DbContextOptions<AssetManagementDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureAssetManagement();
    }
}
