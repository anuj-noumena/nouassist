using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Customization.EntityFrameworkCore;

[ConnectionStringName(CustomizationDbProperties.ConnectionStringName)]
public class CustomizationDbContext : AbpDbContext<CustomizationDbContext>, ICustomizationDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public CustomizationDbContext(DbContextOptions<CustomizationDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureCustomization();
    }
}
