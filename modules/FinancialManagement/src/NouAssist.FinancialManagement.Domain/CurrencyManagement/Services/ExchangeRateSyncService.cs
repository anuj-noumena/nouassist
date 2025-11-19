using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.CurrencyManagement.Aggregates;
using NouAssist.FinancialManagement.CurrencyManagement.Data;
using NouAssist.FinancialManagement.CurrencyManagement.Enums;
using NouAssist.FinancialManagement.CurrencyManagement.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.CurrencyManagement.Services
{
    public class ExchangeRateSyncService : DomainService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ICurrencyExchangeRateRepository _exchangeRateRepository;

        public ExchangeRateSyncService(
            ICurrencyRepository currencyRepository,
            ICurrencyExchangeRateRepository exchangeRateRepository)
        {
            _currencyRepository = currencyRepository;
            _exchangeRateRepository = exchangeRateRepository;
        }

        public async Task<int> SyncExchangeRatesAsync(
            Dictionary<string, decimal> rates,
            CurrencyCode baseCurrency,
            ExchangeRateProvider provider,
            DateTime effectiveDate)
        {
            var activeCurrencies = await _currencyRepository.GetActiveCurrenciesAsync();
            var updatedCount = 0;

            foreach (var currency in activeCurrencies)
            {
                if (currency.Code.Equals(baseCurrency))
                {
                    continue; // Skip base currency
                }

                if (!rates.TryGetValue(currency.Code.Code, out var rate))
                {
                    continue; // Skip if no rate provided
                }

                var existingRate = await _exchangeRateRepository.GetRateAsync(
                    baseCurrency,
                    currency.Code,
                    effectiveDate);

                if (existingRate != null && existingRate.IsActive)
                {
                    // Update existing rate
                    existingRate.UpdateRate(rate, effectiveDate);
                    await _exchangeRateRepository.UpdateAsync(existingRate);
                }
                else
                {
                    // Create new rate
                    var newRate = new CurrencyExchangeRate(
                        GuidGenerator.Create(),
                        null, // Tenant context
                        baseCurrency,
                        currency.Code,
                        rate,
                        effectiveDate,
                        provider);

                    await _exchangeRateRepository.InsertAsync(newRate);
                }

                updatedCount++;
            }

            return updatedCount;
        }

        public async Task DeactivateOldRatesAsync(DateTime cutoffDate)
        {
            var oldRates = await _exchangeRateRepository.GetActiveRatesBeforeDateAsync(cutoffDate);

            foreach (var rate in oldRates)
            {
                rate.Deactivate(cutoffDate);
            }

            await _exchangeRateRepository.UpdateManyAsync(oldRates);
        }
    }
}
