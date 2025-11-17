using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.CurrencyManagement.Aggregates;
using NouAssist.FinancialManagement.CurrencyManagement.Data;
using NouAssist.FinancialManagement.CurrencyManagement.ValueObjects;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CurrencyManagement.EntityFrameworkCore.Repositories
{
    public class EfCoreCurrencyExchangeRateRepository
        : EfCoreRepository<ICurrencyManagementDbContext, CurrencyExchangeRate, Guid>,
          ICurrencyExchangeRateRepository
    {
        public EfCoreCurrencyExchangeRateRepository(
            IDbContextProvider<ICurrencyManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<CurrencyExchangeRate> GetRateAsync(
            CurrencyCode baseCurrency,
            CurrencyCode targetCurrency,
            DateTime effectiveDate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            var date = effectiveDate.Date;

            return await query
                .Where(r => r.BaseCurrency.Code == baseCurrency.Code &&
                           r.TargetCurrency.Code == targetCurrency.Code &&
                           r.IsActive &&
                           r.ValidFrom <= date &&
                           (!r.ValidTo.HasValue || r.ValidTo >= date))
                .OrderByDescending(r => r.ValidFrom)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<CurrencyExchangeRate>> GetRatesForCurrencyAsync(
            CurrencyCode currency,
            DateTime effectiveDate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            var date = effectiveDate.Date;

            return await query
                .Where(r => (r.BaseCurrency.Code == currency.Code ||
                            r.TargetCurrency.Code == currency.Code) &&
                           r.IsActive &&
                           r.ValidFrom <= date &&
                           (!r.ValidTo.HasValue || r.ValidTo >= date))
                .OrderBy(r => r.BaseCurrency.Code)
                .ThenBy(r => r.TargetCurrency.Code)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<CurrencyExchangeRate>> GetActiveRatesBeforeDateAsync(
            DateTime cutoffDate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(r => r.IsActive && r.ValidFrom < cutoffDate)
                .OrderBy(r => r.ValidFrom)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<CurrencyExchangeRate>> GetHistoricalRatesAsync(
            CurrencyCode baseCurrency,
            CurrencyCode targetCurrency,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(r => r.BaseCurrency.Code == baseCurrency.Code &&
                           r.TargetCurrency.Code == targetCurrency.Code &&
                           r.ValidFrom >= startDate &&
                           r.ValidFrom <= endDate)
                .OrderBy(r => r.ValidFrom)
                .ToListAsync(cancellationToken);
        }
    }
}
