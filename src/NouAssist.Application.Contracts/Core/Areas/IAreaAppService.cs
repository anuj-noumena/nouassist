using System;
using System.Threading.Tasks;
using NouAssist.Core.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Core.Areas
{
    public interface IAreaAppService : IApplicationService
    {
        Task<AreaDto> GetAsync(Guid id);
        Task<AreaDto> GetWithChildrenAsync(Guid id);
        Task<ListResultDto<AreaDto>> GetMyAreasAsync();
        Task<ListResultDto<AreaDto>> GetRootAreasAsync();
        Task<ListResultDto<AreaDto>> GetChildAreasAsync(Guid parentId);
        Task<AreaDto> CreateAsync(CreateAreaDto input);
        Task<AreaDto> UpdateAsync(Guid id, UpdateAreaDto input);
        Task DeleteAsync(Guid id);
        Task<AreaDto> ArchiveAsync(Guid id);
        Task<AreaDto> ReactivateAsync(Guid id);
        Task<AreaDto> MarkAsReviewedAsync(Guid id);
        Task<AreaDto> AddStandardAsync(Guid id, string standard);
        Task<AreaDto> AddKeyResultAreaAsync(Guid id, string kra);
    }
}
