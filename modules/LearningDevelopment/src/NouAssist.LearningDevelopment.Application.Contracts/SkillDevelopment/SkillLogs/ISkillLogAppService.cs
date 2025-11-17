using System;
using System.Threading.Tasks;
using NouAssist.LearningDevelopment.SkillDevelopment.SkillLogs.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.LearningDevelopment.SkillDevelopment.SkillLogs
{
    public interface ISkillLogAppService : IApplicationService
    {
        Task<SkillLogDto> GetAsync(Guid id);
        Task<PagedResultDto<SkillLogDto>> GetListAsync(GetSkillLogListDto input);
        Task<SkillLogDto> CreateAsync(CreateSkillLogDto input);
        Task<SkillLogDto> UpdateAsync(Guid id, UpdateSkillLogDto input);
        Task DeleteAsync(Guid id);
    }
}
