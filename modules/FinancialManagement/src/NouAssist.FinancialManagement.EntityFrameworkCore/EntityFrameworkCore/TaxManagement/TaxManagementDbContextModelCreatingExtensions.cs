using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.TaxManagement.EntityFrameworkCore
{
    public static class TaxManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureTaxManagement(
            this ModelBuilder builder,
            TaxManagementDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            builder.ApplyConfigurationsFromAssembly(typeof(TaxManagementDbContext).Assembly);
        }
    }
}
