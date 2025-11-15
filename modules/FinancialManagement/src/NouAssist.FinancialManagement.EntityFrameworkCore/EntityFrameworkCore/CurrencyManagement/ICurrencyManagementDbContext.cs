using Microsoft.EntityFrameworkCore;
using NouAssist.CurrencyManagement.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CurrencyManagement.EntityFrameworkCore
{
    [ConnectionStringName(CurrencyManagementDbProperties.ConnectionStringName)]
    public interface ICurrencyManagementDbContext : IEfCoreDbContext
    {
        DbSet<Currency> Currencies { get; }
        DbSet<CurrencyExchangeRate> ExchangeRates { get; }
    }
}
