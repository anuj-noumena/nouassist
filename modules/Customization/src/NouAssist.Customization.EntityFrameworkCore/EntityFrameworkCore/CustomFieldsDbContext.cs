using Microsoft.EntityFrameworkCore;
using NouAssist.Customization.CustomFields.Aggregates;
using NouAssist.Customization.CustomFields.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Customization.CustomFields.EntityFrameworkCore
{
    [ConnectionStringName(CustomFieldsDbProperties.ConnectionStringName)]
    public class CustomFieldsDbContext : AbpDbContext<CustomFieldsDbContext>, ICustomFieldsDbContext
    {
        public DbSet<CustomFieldDefinition> CustomFieldDefinitions { get; set; }
        public DbSet<CustomFieldValue> CustomFieldValues { get; set; }

        public CustomFieldsDbContext(DbContextOptions<CustomFieldsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureCustomFields(this);
        }
    }
}
