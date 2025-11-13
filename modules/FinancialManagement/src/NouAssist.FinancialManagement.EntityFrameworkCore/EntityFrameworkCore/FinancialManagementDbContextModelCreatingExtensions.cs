using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore
{
    public static class FinancialManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureFinancialManagement(
            this ModelBuilder builder,
            FinancialManagementDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes from this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(FinancialManagementDbContext).Assembly);

            // Configure schema for all tables
            builder.HasDefaultSchema(FinancialManagementDbProperties.DbSchema);
        }
    }
}
