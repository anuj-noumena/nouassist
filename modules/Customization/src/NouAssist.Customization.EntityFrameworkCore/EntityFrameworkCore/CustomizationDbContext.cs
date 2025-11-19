using Microsoft.EntityFrameworkCore;
using NouAssist.Customization.CustomFields.Aggregates;
using NouAssist.Customization.CustomFields.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Customization.EntityFrameworkCore;

[ConnectionStringName(CustomizationDbProperties.ConnectionStringName)]
public class CustomizationDbContext : AbpDbContext<CustomizationDbContext>, ICustomizationDbContext
{
    public DbSet<CustomFieldDefinition> CustomFieldDefinitions { get; set; }
    public DbSet<CustomFieldValue> CustomFieldValues { get; set; }
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
