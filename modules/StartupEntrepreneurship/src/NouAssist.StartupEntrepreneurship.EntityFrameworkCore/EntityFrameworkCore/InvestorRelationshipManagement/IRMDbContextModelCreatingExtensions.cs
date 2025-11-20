using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore.Configurations;
using Volo.Abp;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore
{
    /// <summary>
    /// Extension methods for configuring IRM module model
    /// </summary>
    public static class IRMDbContextModelCreatingExtensions
    {
        public static void ConfigureIRM(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes defined in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(IRMDbContext).Assembly);

            // Any other module-specific model configurations can go here
            // For example, global filters or conventions
        }
    }
}
