using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.CurrencyManagement.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.CurrencyManagement
{
    public interface IExchangeRateAppService : IApplicationService
    {
        Task<ExchangeRateDto> GetAsync(Guid id);
        
        Task<PagedResultDto<ExchangeRateDto>> GetListAsync(GetExchangeRateListDto input);
        
        Task<ExchangeRateDto> GetCurrentRateAsync(string baseCurrency, string targetCurrency);
        
        Task<ExchangeRateDto> CreateAsync(CreateExchangeRateDto input);
        
        Task<ExchangeRateDto> UpdateAsync(Guid id, CreateExchangeRateDto input);
        
        Task<CurrencyConversionResultDto> ConvertAsync(ConvertCurrencyDto input);
        
        Task<int> SyncExchangeRatesAsync();
    }
}
