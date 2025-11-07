using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.ReflectionAndSupport.CustomFields.EntityFrameworkCore;

[ConnectionStringName(CustomFieldsDbProperties.ConnectionStringName)]
public class CustomFieldsDbContext : AbpDbContext<CustomFieldsDbContext>, ICustomFieldsDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public CustomFieldsDbContext(DbContextOptions<CustomFieldsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureCustomFields();
    }
}
