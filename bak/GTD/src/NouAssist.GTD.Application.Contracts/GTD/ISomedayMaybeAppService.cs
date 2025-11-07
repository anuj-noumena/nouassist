using System;
using System.Threading.Tasks;
using NouAssist.GTD.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.GTD.GTD
{
    public interface ISomedayMaybeAppService : IApplicationService
    {
        Task<SomedayMaybeItemDto> GetAsync(Guid id);
        Task<PagedResultDto<SomedayMaybeItemDto>> GetListAsync(GetSomedayMaybeListDto input);
        Task<ListResultDto<SomedayMaybeItemDto>> GetItemsNeedingReviewAsync();
        Task<SomedayMaybeItemDto> CreateAsync(CreateSomedayMaybeItemDto input);
        Task<SomedayMaybeItemDto> UpdateAsync(Guid id, UpdateSomedayMaybeItemDto input);
        Task DeleteAsync(Guid id);
        Task<SomedayMaybeItemDto> PromoteAsync(Guid id, PromoteSomedayMaybeDto input);
        Task<SomedayMaybeItemDto> ArchiveAsync(Guid id);
        Task<SomedayMaybeItemDto> ActivateAsync(Guid id);
        Task<SomedayMaybeItemDto> PutOnHoldAsync(Guid id);
        Task<SomedayMaybeItemDto> ReviewAsync(Guid id, ReviewSomedayMaybeDto input);
    }
}
