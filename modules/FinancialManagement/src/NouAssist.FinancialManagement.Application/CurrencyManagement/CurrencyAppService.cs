using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.CurrencyManagement.Aggregates;
using NouAssist.FinancialManagement.CurrencyManagement.Data;
using NouAssist.FinancialManagement.CurrencyManagement.Dtos;
using NouAssist.FinancialManagement.CurrencyManagement.Permissions;
using NouAssist.FinancialManagement.CurrencyManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.CurrencyManagement
{
    [Authorize(CurrencyManagementPermissions.Currencies.View)]
    public class CurrencyAppService : ApplicationService, ICurrencyAppService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyAppService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<CurrencyDto> GetAsync(Guid id)
        {
            var currency = await _currencyRepository.GetAsync(id);
            return ObjectMapper.Map<Currency, CurrencyDto>(currency);
        }

        public async Task<CurrencyDto> GetByCodeAsync(string code)
        {
            var currency = await _currencyRepository.GetByCodeAsync(new CurrencyCode(code));
            
            if (currency == null)
            {
                throw new BusinessException("CurrencyManagement:CurrencyNotFound")
                    .WithData("Code", code);
            }

            return ObjectMapper.Map<Currency, CurrencyDto>(currency);
        }

        public async Task<ListResultDto<CurrencyDto>> GetActiveCurrenciesAsync()
        {
            var currencies = await _currencyRepository.GetActiveCurrenciesAsync();
            return new ListResultDto<CurrencyDto>(
                ObjectMapper.Map<List<Currency>, List<CurrencyDto>>(currencies));
        }

        [Authorize(CurrencyManagementPermissions.Currencies.Create)]
        public async Task<CurrencyDto> CreateAsync(CreateCurrencyDto input)
        {
            var currencyCode = new CurrencyCode(input.Code);

            // Check for duplicates
            if (await _currencyRepository.CurrencyExistsAsync(currencyCode))
            {
                throw new BusinessException("CurrencyManagement:DuplicateCurrency")
                    .WithData("Code", input.Code);
            }

            var currency = new Currency(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                currencyCode,
                input.Name,
                input.Symbol,
                input.DecimalPlaces,
                input.IsBaseCurrency);

            currency.DisplayOrder = input.DisplayOrder;

            await _currencyRepository.InsertAsync(currency);

            return ObjectMapper.Map<Currency, CurrencyDto>(currency);
        }

        [Authorize(CurrencyManagementPermissions.Currencies.Update)]
        public async Task<CurrencyDto> UpdateAsync(Guid id, UpdateCurrencyDto input)
        {
            var currency = await _currencyRepository.GetAsync(id);

            // Cannot change status via this method
            // Use Activate/Deactivate methods instead
            
            await _currencyRepository.UpdateAsync(currency);

            return ObjectMapper.Map<Currency, CurrencyDto>(currency);
        }

        [Authorize(CurrencyManagementPermissions.Currencies.Activate)]
        public async Task ActivateAsync(Guid id)
        {
            var currency = await _currencyRepository.GetAsync(id);
            currency.Activate();
            await _currencyRepository.UpdateAsync(currency);
        }

        [Authorize(CurrencyManagementPermissions.Currencies.Deactivate)]
        public async Task DeactivateAsync(Guid id)
        {
            var currency = await _currencyRepository.GetAsync(id);
            currency.Deactivate();
            await _currencyRepository.UpdateAsync(currency);
        }
    }
}
