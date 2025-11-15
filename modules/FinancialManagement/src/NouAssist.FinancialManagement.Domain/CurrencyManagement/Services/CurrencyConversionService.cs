using System;
using System.Threading.Tasks;
using NouAssist.CurrencyManagement.Data;
using NouAssist.CurrencyManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.CurrencyManagement.Services
{
    public class CurrencyConversionService : DomainService
    {
        private readonly ICurrencyExchangeRateRepository _exchangeRateRepository;

        public CurrencyConversionService(ICurrencyExchangeRateRepository exchangeRateRepository)
        {
            _exchangeRateRepository = exchangeRateRepository;
        }

        public async Task<MonetaryAmount> ConvertAsync(
            MonetaryAmount amount,
            CurrencyCode targetCurrency,
            DateTime? asOfDate = null)
        {
            if (amount.Currency.Equals(targetCurrency))
            {
                return amount;
            }

            var effectiveDate = asOfDate ?? Clock.Now;
            
            var exchangeRate = await _exchangeRateRepository.GetRateAsync(
                amount.Currency,
                targetCurrency,
                effectiveDate);

            if (exchangeRate == null)
            {
                throw new BusinessException("CurrencyManagement:ExchangeRateNotFound")
                    .WithData("BaseCurrency", amount.Currency.Code)
                    .WithData("TargetCurrency", targetCurrency.Code)
                    .WithData("Date", effectiveDate);
            }

            var convertedAmount = exchangeRate.Rate.Convert(amount.Amount);
            return new MonetaryAmount(convertedAmount, targetCurrency);
        }

        public async Task<decimal> GetExchangeRateAsync(
            CurrencyCode baseCurrency,
            CurrencyCode targetCurrency,
            DateTime? asOfDate = null)
        {
            if (baseCurrency.Equals(targetCurrency))
            {
                return 1.0m;
            }

            var effectiveDate = asOfDate ?? Clock.Now;
            
            var exchangeRate = await _exchangeRateRepository.GetRateAsync(
                baseCurrency,
                targetCurrency,
                effectiveDate);

            if (exchangeRate == null)
            {
                throw new BusinessException("CurrencyManagement:ExchangeRateNotFound")
                    .WithData("BaseCurrency", baseCurrency.Code)
                    .WithData("TargetCurrency", targetCurrency.Code)
                    .WithData("Date", effectiveDate);
            }

            return exchangeRate.Rate.Rate;
        }

        public async Task<MonetaryAmount> ConvertToBaseCurrencyAsync(
            MonetaryAmount amount,
            CurrencyCode baseCurrency,
            DateTime? asOfDate = null)
        {
            return await ConvertAsync(amount, baseCurrency, asOfDate);
        }
    }
}
