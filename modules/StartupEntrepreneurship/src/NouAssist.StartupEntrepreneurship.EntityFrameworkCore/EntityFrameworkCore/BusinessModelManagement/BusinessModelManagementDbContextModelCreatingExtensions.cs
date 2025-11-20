using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.EntityFrameworkCore.Configurations;
using Volo.Abp;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.EntityFrameworkCore
{
    public static class BusinessModelManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureBusinessModelManagement(
            this ModelBuilder builder,
            BusinessModelManagementDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes
            builder.ApplyConfigurationsFromAssembly(typeof(BusinessModelManagementDbContext).Assembly);
        }
    }
}
