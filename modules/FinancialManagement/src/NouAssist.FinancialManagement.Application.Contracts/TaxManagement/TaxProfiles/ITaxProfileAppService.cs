using System.Threading.Tasks;
using NouAssist.FinancialManagement.TaxManagement.TaxProfiles.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.TaxManagement.TaxProfiles
{
    public interface ITaxProfileAppService : IApplicationService
    {
        Task<TaxProfileDto> GetMyProfileAsync();
        Task<TaxProfileDto> CreateOrUpdateAsync(CreateOrUpdateTaxProfileDto input);
    }
}
