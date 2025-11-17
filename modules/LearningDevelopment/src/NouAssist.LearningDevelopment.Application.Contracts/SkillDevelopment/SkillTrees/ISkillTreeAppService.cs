using System;
using System.Threading.Tasks;
using NouAssist.LearningDevelopment.SkillDevelopment.SkillTrees.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.LearningDevelopment.SkillDevelopment.SkillTrees
{
    public interface ISkillTreeAppService : IApplicationService
    {
        Task<SkillTreeDto> GetAsync(Guid id);
        Task<SkillTreeDto> GetBySkillIdAsync(Guid skillId);
        Task<SkillTreeDto> UpdateAsync(Guid id, UpdateSkillTreeDto input);
    }
}
