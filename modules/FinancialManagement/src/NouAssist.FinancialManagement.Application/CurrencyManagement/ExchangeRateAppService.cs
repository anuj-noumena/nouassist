using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.CurrencyManagement.Aggregates;
using NouAssist.CurrencyManagement.Data;
using NouAssist.CurrencyManagement.Dtos;
using NouAssist.CurrencyManagement.Enums;
using NouAssist.CurrencyManagement.Permissions;
using NouAssist.CurrencyManagement.Services;
using NouAssist.CurrencyManagement.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.CurrencyManagement
{
    [Authorize(CurrencyManagementPermissions.ExchangeRates.View)]
    public class ExchangeRateAppService : ApplicationService, IExchangeRateAppService
    {
        private readonly ICurrencyExchangeRateRepository _exchangeRateRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly CurrencyConversionService _conversionService;
        private readonly ExchangeRateSyncService _syncService;

        public ExchangeRateAppService(
            ICurrencyExchangeRateRepository exchangeRateRepository,
            ICurrencyRepository currencyRepository,
            CurrencyConversionService conversionService,
            ExchangeRateSyncService syncService)
        {
            _exchangeRateRepository = exchangeRateRepository;
            _currencyRepository = currencyRepository;
            _conversionService = conversionService;
            _syncService = syncService;
        }

        public async Task<ExchangeRateDto> GetAsync(Guid id)
        {
            var exchangeRate = await _exchangeRateRepository.GetAsync(id);
            return ObjectMapper.Map<CurrencyExchangeRate, ExchangeRateDto>(exchangeRate);
        }

        public async Task<PagedResultDto<ExchangeRateDto>> GetListAsync(GetExchangeRateListDto input)
        {
            var query = await _exchangeRateRepository.GetQueryableAsync();

            if (!string.IsNullOrEmpty(input.BaseCurrency))
            {
                query = query.Where(r => r.BaseCurrency.Code == input.BaseCurrency);
            }

            if (!string.IsNullOrEmpty(input.TargetCurrency))
            {
                query = query.Where(r => r.TargetCurrency.Code == input.TargetCurrency);
            }

            if (input.FromDate.HasValue)
            {
                query = query.Where(r => r.ValidFrom >= input.FromDate.Value);
            }

            if (input.ToDate.HasValue)
            {
                query = query.Where(r => r.ValidFrom <= input.ToDate.Value);
            }

            if (input.ActiveOnly.GetValueOrDefault(true))
            {
                query = query.Where(r => r.IsActive);
            }

            var totalCount = await AsyncExecuter.CountAsync(query);

            query = query.OrderByDescending(r => r.ValidFrom)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var exchangeRates = await AsyncExecuter.ToListAsync(query);

            return new PagedResultDto<ExchangeRateDto>(
                totalCount,
                ObjectMapper.Map<List<CurrencyExchangeRate>, List<ExchangeRateDto>>(exchangeRates));
        }

        public async Task<ExchangeRateDto> GetCurrentRateAsync(string baseCurrency, string targetCurrency)
        {
            var rate = await _exchangeRateRepository.GetRateAsync(
                new CurrencyCode(baseCurrency),
                new CurrencyCode(targetCurrency),
                Clock.Now);

            if (rate == null)
            {
                throw new BusinessException("CurrencyManagement:ExchangeRateNotFound")
                    .WithData("BaseCurrency", baseCurrency)
                    .WithData("TargetCurrency", targetCurrency);
            }

            return ObjectMapper.Map<CurrencyExchangeRate, ExchangeRateDto>(rate);
        }

        [Authorize(CurrencyManagementPermissions.ExchangeRates.Create)]
        public async Task<ExchangeRateDto> CreateAsync(CreateExchangeRateDto input)
        {
            var baseCurrency = new CurrencyCode(input.BaseCurrency);
            var targetCurrency = new CurrencyCode(input.TargetCurrency);

            // Verify currencies exist
            if (!await _currencyRepository.CurrencyExistsAsync(baseCurrency))
            {
                throw new BusinessException("CurrencyManagement:CurrencyNotFound")
                    .WithData("Code", input.BaseCurrency);
            }

            if (!await _currencyRepository.CurrencyExistsAsync(targetCurrency))
            {
                throw new BusinessException("CurrencyManagement:CurrencyNotFound")
                    .WithData("Code", input.TargetCurrency);
            }

            var exchangeRate = new CurrencyExchangeRate(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                baseCurrency,
                targetCurrency,
                input.Rate,
                input.EffectiveDate,
                input.Provider,
                input.ProviderReference);

            await _exchangeRateRepository.InsertAsync(exchangeRate);

            return ObjectMapper.Map<CurrencyExchangeRate, ExchangeRateDto>(exchangeRate);
        }

        [Authorize(CurrencyManagementPermissions.ExchangeRates.Update)]
        public async Task<ExchangeRateDto> UpdateAsync(Guid id, CreateExchangeRateDto input)
        {
            var exchangeRate = await _exchangeRateRepository.GetAsync(id);

            exchangeRate.UpdateRate(input.Rate, input.EffectiveDate);

            await _exchangeRateRepository.UpdateAsync(exchangeRate);

            return ObjectMapper.Map<CurrencyExchangeRate, ExchangeRateDto>(exchangeRate);
        }

        [Authorize(CurrencyManagementPermissions.ExchangeRates.Convert)]
        public async Task<CurrencyConversionResultDto> ConvertAsync(ConvertCurrencyDto input)
        {
            var amount = new MonetaryAmount(input.Amount, new CurrencyCode(input.FromCurrency));
            var targetCurrency = new CurrencyCode(input.ToCurrency);

            var convertedAmount = await _conversionService.ConvertAsync(
                amount,
                targetCurrency,
                input.AsOfDate);

            var rate = await _conversionService.GetExchangeRateAsync(
                amount.Currency,
                targetCurrency,
                input.AsOfDate);

            return new CurrencyConversionResultDto
            {
                OriginalAmount = input.Amount,
                FromCurrency = input.FromCurrency,
                ConvertedAmount = convertedAmount.Amount,
                ToCurrency = input.ToCurrency,
                ExchangeRate = rate,
                EffectiveDate = input.AsOfDate ?? Clock.Now
            };
        }

        [Authorize(CurrencyManagementPermissions.ExchangeRates.Sync)]
        public async Task<int> SyncExchangeRatesAsync()
        {
            // This would call an external API to get rates
            // For now, return a placeholder
            var baseCurrency = await _currencyRepository.GetBaseCurrencyAsync();

            if (baseCurrency == null)
            {
                throw new BusinessException("CurrencyManagement:NoBaseCurrency");
            }

            // In real implementation, call external API
            var rates = new Dictionary<string, decimal>
            {
                ["EUR"] = 0.85m,
                ["GBP"] = 0.73m,
                ["JPY"] = 110.50m
            };

            return await _syncService.SyncExchangeRatesAsync(
                rates,
                baseCurrency.Code,
                ExchangeRateProvider.ECB,
                Clock.Now);
        }
    }
}
