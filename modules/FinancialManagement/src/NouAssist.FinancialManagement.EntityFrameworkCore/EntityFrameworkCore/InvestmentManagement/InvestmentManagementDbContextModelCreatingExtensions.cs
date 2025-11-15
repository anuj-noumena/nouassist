using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.InvestmentManagement.EntityFrameworkCore
{
    public static class InvestmentManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureInvestmentManagement(
            this ModelBuilder builder,
            InvestmentManagementDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            builder.ApplyConfigurationsFromAssembly(typeof(InvestmentManagementDbContext).Assembly);
        }
    }
}
