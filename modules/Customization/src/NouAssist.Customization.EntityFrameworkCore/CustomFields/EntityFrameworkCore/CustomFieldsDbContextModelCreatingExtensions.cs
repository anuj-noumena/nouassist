using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.Customization.CustomFields.EntityFrameworkCore
{
    public static class CustomFieldsDbContextModelCreatingExtensions
    {
        public static void ConfigureCustomFields(
            this ModelBuilder builder,
            CustomFieldsDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            builder.ApplyConfigurationsFromAssembly(typeof(CustomFieldsDbContext).Assembly);
        }
    }
}
