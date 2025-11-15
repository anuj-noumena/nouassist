using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.FinancialManagement.EntityFrameworkCore
{
    public static class FinancialManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureFinancialManagement(
            this ModelBuilder builder,
            FinancialManagementDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all entity type configurations from this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(FinancialManagementDbContext).Assembly);

            // Configure schema
            builder.HasDefaultSchema(Shared.FinancialManagementDbProperties.DbSchema);

            // Additional global configurations can go here
            // e.g., custom value converters, query filters, etc.
        }
    }
}
