using System;
using System.Threading.Tasks;
using NouAssist.LearningDevelopment.SkillDevelopment.Resources.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.LearningDevelopment.SkillDevelopment.Resources
{
    public interface IResourceHistoryAppService : IApplicationService
    {
        Task<ResourceHistoryDto> GetAsync(Guid id);
        Task<PagedResultDto<ResourceHistoryDto>> GetListAsync(GetResourceHistoryListDto input);
        Task<ResourceHistoryDto> CreateAsync(CreateResourceHistoryDto input);
        Task<ResourceHistoryDto> UpdateProgressAsync(Guid id, UpdateResourceProgressDto input);
        Task DeleteAsync(Guid id);
        Task LinkToSkillAsync(Guid resourceId, Guid skillId);
        Task UnlinkFromSkillAsync(Guid resourceId, Guid skillId);
    }
}
