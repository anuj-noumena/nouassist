using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.CurrencyManagement.EntityFrameworkCore
{
    public static class CurrencyManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureCurrencyManagement(
            this ModelBuilder builder,
            CurrencyManagementDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            builder.ApplyConfigurationsFromAssembly(typeof(CurrencyManagementDbContext).Assembly);
        }
    }
}
