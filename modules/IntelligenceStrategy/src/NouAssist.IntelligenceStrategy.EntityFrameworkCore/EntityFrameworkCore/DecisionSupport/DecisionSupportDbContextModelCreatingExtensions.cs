using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.EntityFrameworkCore
{
    /// <summary>
    /// Extension methods for configuring the Decision Support module's database model
    /// </summary>
    public static class DecisionSupportDbContextModelCreatingExtensions
    {
        /// <summary>
        /// Configures the Decision Support module database model
        /// </summary>
        public static void ConfigureDecisionSupport(
            this ModelBuilder builder,
            DecisionSupportDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));
            
            // Apply all IEntityTypeConfiguration classes defined in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(DecisionSupportDbContext).Assembly);
        }
    }
}
