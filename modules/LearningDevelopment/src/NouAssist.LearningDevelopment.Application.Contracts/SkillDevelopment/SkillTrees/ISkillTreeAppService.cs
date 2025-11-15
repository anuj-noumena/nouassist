using System;
using System.Threading.Tasks;
using NouAssist.SkillDevelopment.SkillTrees.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.SkillDevelopment.SkillTrees
{
    public interface ISkillTreeAppService : IApplicationService
    {
        Task<SkillTreeDto> GetAsync(Guid id);
        Task<SkillTreeDto> GetBySkillIdAsync(Guid skillId);
        Task<SkillTreeDto> UpdateAsync(Guid id, UpdateSkillTreeDto input);
    }
}
