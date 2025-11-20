using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore
{
    public static class PerformanceFinancialModelingDbContextModelCreatingExtensions
    {
        public static void ConfigurePerformanceFinancialModeling(
            this ModelBuilder builder, 
            PerformanceFinancialModelingDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes defined in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(PerformanceFinancialModelingDbContext).Assembly);
        }
    }
}
