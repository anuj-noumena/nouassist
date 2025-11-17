using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.LearningDevelopment.SkillDevelopment.Skills.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.LearningDevelopment.SkillDevelopment.Skills
{
    public interface ISkillAppService : IApplicationService
    {
        Task<SkillDto> GetAsync(Guid id);
        Task<PagedResultDto<SkillDto>> GetListAsync(GetSkillListDto input);
        Task<List<SkillSummaryDto>> GetMySkillsAsync();
        Task<List<SkillDto>> GetSubSkillsAsync(Guid parentId);
        Task<SkillDto> CreateAsync(CreateSkillDto input);
        Task<SkillDto> UpdateAsync(Guid id, UpdateSkillDto input);
        Task DeleteAsync(Guid id);
        Task<SkillDto> UpdateProficiencyAsync(Guid id, UpdateSkillProficiencyDto input);
        Task<SkillDto> MarkAsMasteredAsync(Guid id);
        Task<SkillDto> ArchiveAsync(Guid id);
        Task AddPrerequisiteAsync(Guid skillId, Guid prerequisiteId);
        Task RemovePrerequisiteAsync(Guid skillId, Guid prerequisiteId);
        Task<SkillProgressDto> GetProgressAsync(Guid id);
        Task<List<SkillProgressDto>> GetProgressSummaryAsync();
    }
}
