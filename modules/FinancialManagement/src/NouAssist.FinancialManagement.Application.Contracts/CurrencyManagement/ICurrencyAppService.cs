using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.CurrencyManagement.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.CurrencyManagement
{
    public interface ICurrencyAppService : IApplicationService
    {
        Task<CurrencyDto> GetAsync(Guid id);
        
        Task<CurrencyDto> GetByCodeAsync(string code);
        
        Task<ListResultDto<CurrencyDto>> GetActiveCurrenciesAsync();
        
        Task<CurrencyDto> CreateAsync(CreateCurrencyDto input);
        
        Task<CurrencyDto> UpdateAsync(Guid id, UpdateCurrencyDto input);
        
        Task ActivateAsync(Guid id);
        
        Task DeactivateAsync(Guid id);

        Task<PagedResultDto<CurrencyDto>> GetListAsync(GetCurrencyListDto input);
    }
}
