using Microsoft.EntityFrameworkCore;
using NouAssist.CurrencyManagement.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CurrencyManagement.EntityFrameworkCore
{
    [ConnectionStringName(CurrencyManagementDbProperties.ConnectionStringName)]
    public class CurrencyManagementDbContext
        : AbpDbContext<CurrencyManagementDbContext>, ICurrencyManagementDbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyExchangeRate> ExchangeRates { get; set; }

        public CurrencyManagementDbContext(DbContextOptions<CurrencyManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureCurrencyManagement(this);
        }
    }
}
