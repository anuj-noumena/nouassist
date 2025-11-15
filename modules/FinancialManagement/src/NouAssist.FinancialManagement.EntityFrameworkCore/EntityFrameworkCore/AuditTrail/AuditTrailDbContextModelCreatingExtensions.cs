using Microsoft.EntityFrameworkCore;
using NouAssist.AuditTrail.EntityFrameworkCore.Configurations;
using Volo.Abp;

namespace NouAssist.AuditTrail.EntityFrameworkCore
{
    public static class AuditTrailDbContextModelCreatingExtensions
    {
        public static void ConfigureAuditTrail(
            this ModelBuilder builder,
            AuditTrailDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes from assembly
            builder.ApplyConfigurationsFromAssembly(typeof(AuditTrailDbContext).Assembly);
        }
    }
}
