using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.EntityFrameworkCore
{
    public static class RequirementsAnalysisToolsDbContextModelCreatingExtensions
    {
        public static void ConfigureRequirementsAnalysisTools(
            this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes defined in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(RequirementsAnalysisToolsDbContext).Assembly);
        }
    }
}
