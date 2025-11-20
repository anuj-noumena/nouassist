using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.ContractPortfolioManagement.EntityFrameworkCore
{
    /// <summary>
    /// Extension methods to configure EF Core model for ContractPortfolioManagement module
    /// </summary>
    public static class ContractPortfolioManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureContractPortfolioManagement(
            this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes defined in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(ContractPortfolioManagementDbContext).Assembly);
        }
    }
}
