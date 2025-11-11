using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.AssetManagement.Resources.EntityFrameworkCore
{
    public static class ResourcesDbContextModelCreatingExtensions
    {
        public static void ConfigureResources(
            this ModelBuilder builder,
            ResourcesDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            builder.ApplyConfigurationsFromAssembly(typeof(ResourcesDbContext).Assembly);
        }
    }
}
