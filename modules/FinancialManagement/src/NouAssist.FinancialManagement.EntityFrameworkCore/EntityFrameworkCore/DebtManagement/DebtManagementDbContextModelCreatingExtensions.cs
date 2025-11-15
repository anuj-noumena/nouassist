using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.DebtManagement.EntityFrameworkCore
{
    public static class DebtManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureDebtManagement(
            this ModelBuilder builder,
            DebtManagementDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes
            builder.ApplyConfigurationsFromAssembly(typeof(DebtManagementDbContext).Assembly);
        }
    }
}
