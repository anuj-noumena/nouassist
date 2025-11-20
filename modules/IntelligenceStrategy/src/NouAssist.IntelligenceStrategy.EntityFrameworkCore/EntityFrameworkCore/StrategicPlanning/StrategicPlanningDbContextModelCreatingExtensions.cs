using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore.Configurations;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore
{
    /// <summary>
    /// Model creating extensions for Strategic Planning DbContext
    /// </summary>
    public static class StrategicPlanningDbContextModelCreatingExtensions
    {
        public static void ConfigureStrategicPlanning(
            this ModelBuilder builder, 
            StrategicPlanningDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes defined in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(StrategicPlanningDbContext).Assembly);

            // Configure global query filters for soft delete and multi-tenancy
            // This is handled by ConfigureByConvention() in each entity configuration

            // Any other module-specific model configurations can go here
        }
    }
}
