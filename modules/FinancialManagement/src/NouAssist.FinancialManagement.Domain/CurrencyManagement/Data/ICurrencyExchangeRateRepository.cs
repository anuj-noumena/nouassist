using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.CurrencyManagement.Aggregates;
using NouAssist.CurrencyManagement.ValueObjects;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.CurrencyManagement.Data
{
    public interface ICurrencyExchangeRateRepository : IRepository<CurrencyExchangeRate, Guid>
    {
        Task<CurrencyExchangeRate> GetRateAsync(
            CurrencyCode baseCurrency,
            CurrencyCode targetCurrency,
            DateTime effectiveDate);

        Task<List<CurrencyExchangeRate>> GetRatesForCurrencyAsync(
            CurrencyCode currency,
            DateTime effectiveDate);

        Task<List<CurrencyExchangeRate>> GetActiveRatesBeforeDateAsync(DateTime cutoffDate);
        
        Task<List<CurrencyExchangeRate>> GetHistoricalRatesAsync(
            CurrencyCode baseCurrency,
            CurrencyCode targetCurrency,
            DateTime startDate,
            DateTime endDate);
    }
}
