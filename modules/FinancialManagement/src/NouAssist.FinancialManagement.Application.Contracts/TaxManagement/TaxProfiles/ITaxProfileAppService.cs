using System;
using System.Threading.Tasks;
using NouAssist.TaxManagement.TaxProfiles.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.TaxManagement.TaxProfiles
{
    public interface ITaxProfileAppService : IApplicationService
    {
        Task<TaxProfileDto> GetMyProfileAsync();
        Task<TaxProfileDto> CreateOrUpdateAsync(CreateOrUpdateTaxProfileDto input);
    }
}
