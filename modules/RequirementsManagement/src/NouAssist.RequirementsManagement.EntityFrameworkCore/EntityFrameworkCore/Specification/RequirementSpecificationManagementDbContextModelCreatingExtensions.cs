using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.RequirementsManagement.Specification.EntityFrameworkCore
{
    public static class RequirementSpecificationManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureRequirementSpecificationManagement(
            this ModelBuilder builder,
            RequirementSpecificationManagementDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes defined in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(RequirementSpecificationManagementDbContext).Assembly);
        }
    }
}
