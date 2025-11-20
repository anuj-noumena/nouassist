using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.EntityFrameworkCore
{
    public static class LegalComplianceDbContextModelCreatingExtensions
    {
        public static void ConfigureLegalCompliance(
            this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes from this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(LegalComplianceDbContext).Assembly);
        }
    }
}
