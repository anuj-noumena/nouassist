using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using NouAssist.LearningDevelopment.SkillDevelopment.Data;
using NouAssist.LearningDevelopment.SkillDevelopment.Permissions;
using NouAssist.LearningDevelopment.SkillDevelopment.SkillTrees.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.LearningDevelopment.SkillDevelopment.SkillTrees
{
    [Authorize(SkillDevelopmentPermissions.SkillTrees.Default)]
    public class SkillTreeAppService : ApplicationService, ISkillTreeAppService
    {
        private readonly ISkillTreeRepository _skillTreeRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IRepository<SkillTree, Guid> _skillTreeBasicRepository;

        public SkillTreeAppService(
            ISkillTreeRepository skillTreeRepository,
            ISkillRepository skillRepository,
            IRepository<SkillTree, Guid> skillTreeBasicRepository)
        {
            _skillTreeRepository = skillTreeRepository;
            _skillRepository = skillRepository;
            _skillTreeBasicRepository = skillTreeBasicRepository;
        }

        [Authorize(SkillDevelopmentPermissions.SkillTrees.View)]
        public async Task<SkillTreeDto> GetAsync(Guid id)
        {
            var skillTree = await _skillTreeRepository.GetAsync(id);

            if (skillTree.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            return await MapToDto(skillTree);
        }

        [Authorize(SkillDevelopmentPermissions.SkillTrees.View)]
        public async Task<SkillTreeDto> GetBySkillIdAsync(Guid skillId)
        {
            // Verify skill ownership
            var skill = await _skillRepository.GetAsync(skillId);
            if (skill.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            var skillTree = await _skillTreeRepository.GetBySkillIdAsync(skillId);
            
            // If no tree exists, create one
            if (skillTree == null)
            {
                skillTree = new SkillTree(
                    GuidGenerator.Create(),
                    CurrentUser.Id.Value,
                    skillId,
                    CurrentTenant.Id);

                // Initialize with the skill as root node
                skillTree.AddNode(skillId, skill.Title, new Dictionary<string, object>
                {
                    { "status", skill.Status.ToString() },
                    { "proficiency", skill.CurrentProficiency.Score },
                    { "isCategory", skill.IsCategory }
                });

                // Add prerequisite edges
                foreach (var prereqId in skill.Prerequisites)
                {
                    skillTree.AddEdge(prereqId, skillId, "prerequisite");
                }

                // Add sub-skill edges
                foreach (var subSkillId in skill.SubSkillIds)
                {
                    skillTree.AddEdge(skillId, subSkillId, "contains");
                }

                await _skillTreeRepository.InsertAsync(skillTree);
            }

            return await MapToDto(skillTree);
        }

        [Authorize(SkillDevelopmentPermissions.SkillTrees.Edit)]
        public async Task<SkillTreeDto> UpdateAsync(Guid id, UpdateSkillTreeDto input)
        {
            var skillTree = await _skillTreeRepository.GetAsync(id);

            if (skillTree.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            if (input.Structure != null)
            {
                skillTree.UpdateStructure(input.Structure);
            }

            if (!string.IsNullOrWhiteSpace(input.VisualizationUrl))
            {
                skillTree.SetVisualizationUrl(input.VisualizationUrl);
            }

            await _skillTreeRepository.UpdateAsync(skillTree);
            return await MapToDto(skillTree);
        }

        private async Task<SkillTreeDto> MapToDto(SkillTree skillTree)
        {
            var dto = ObjectMapper.Map<SkillTree, SkillTreeDto>(skillTree);
            
            var skill = await _skillRepository.GetAsync(skillTree.SkillId);
            dto.SkillTitle = skill.Title;

            return dto;
        }
    }
}
