using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using NouAssist.LearningDevelopment.SkillDevelopment.Data;
using NouAssist.LearningDevelopment.SkillDevelopment.Permissions;
using NouAssist.LearningDevelopment.SkillDevelopment.Services;
using NouAssist.LearningDevelopment.SkillDevelopment.Skills.Dtos;
using NouAssist.LearningDevelopment.SkillDevelopment.Specifications;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.LearningDevelopment.SkillDevelopment.Skills
{
    [Authorize(SkillDevelopmentPermissions.Skills.Default)]
    public class SkillAppService : ApplicationService, ISkillAppService
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IRepository<Skill, Guid> _skillBasicRepository;
        private readonly SkillProgressionService _skillProgressionService;
        private readonly SkillValidationService _skillValidationService;

        public SkillAppService(
            ISkillRepository skillRepository,
            IRepository<Skill, Guid> skillBasicRepository,
            SkillProgressionService skillProgressionService,
            SkillValidationService skillValidationService)
        {
            _skillRepository = skillRepository;
            _skillBasicRepository = skillBasicRepository;
            _skillProgressionService = skillProgressionService;
            _skillValidationService = skillValidationService;
        }

        public async Task<SkillDto> GetAsync(Guid id)
        {
            var skill = await _skillRepository.GetAsync(id);
            
            // Check if user owns this skill or has permission
            if (skill.UserId != CurrentUser.Id)
            {
                await CheckPolicyAsync(SkillDevelopmentPermissions.Skills.Edit);
            }

            return ObjectMapper.Map<Skill, SkillDto>(skill);
        }

        public async Task<PagedResultDto<SkillDto>> GetListAsync(GetSkillListDto input)
        {
            var queryable = await _skillRepository.GetQueryableAsync();

            // Filter by current user
            queryable = queryable.Where(s => s.UserId == CurrentUser.Id.Value);

            // Apply filters
            if (input.Status.HasValue)
            {
                queryable = queryable.Where(s => s.Status == input.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(input.Category))
            {
                queryable = queryable.Where(s => s.Category == input.Category);
            }

            if (input.IsCategory.HasValue)
            {
                queryable = queryable.Where(s => s.IsCategory == input.IsCategory.Value);
            }

            if (input.ParentId.HasValue)
            {
                queryable = queryable.Where(s => s.ParentId == input.ParentId.Value);
            }

            if (input.MinProficiency.HasValue)
            {
                queryable = queryable.Where(s => s.CurrentProficiency.Score >= input.MinProficiency.Value);
            }

            if (input.MaxProficiency.HasValue)
            {
                queryable = queryable.Where(s => s.CurrentProficiency.Score <= input.MaxProficiency.Value);
            }

            if (!string.IsNullOrWhiteSpace(input.SearchTerm))
            {
                queryable = queryable.Where(s => 
                    s.Title.Contains(input.SearchTerm) || 
                    (s.Description != null && s.Description.Contains(input.SearchTerm)));
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            // Sorting
            if (!string.IsNullOrWhiteSpace(input.Sorting))
            {
                queryable = queryable.OrderBy(input.Sorting);
            }
            else
            {
                queryable = queryable.OrderByDescending(s => s.CreationTime);
            }

            // Paging
            queryable = queryable
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var skills = await AsyncExecuter.ToListAsync(queryable);

            return new PagedResultDto<SkillDto>(
                totalCount,
                ObjectMapper.Map<List<Skill>, List<SkillDto>>(skills)
            );
        }

        public async Task<List<SkillSummaryDto>> GetMySkillsAsync()
        {
            var skills = await _skillRepository.GetByUserIdAsync(CurrentUser.Id.Value);
            return ObjectMapper.Map<List<Skill>, List<SkillSummaryDto>>(skills);
        }

        public async Task<List<SkillDto>> GetSubSkillsAsync(Guid parentId)
        {
            var subSkills = await _skillRepository.GetByParentIdAsync(parentId);
            
            // Verify user has access to parent skill
            var parentSkill = await _skillRepository.GetAsync(parentId);
            if (parentSkill.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            return ObjectMapper.Map<List<Skill>, List<SkillDto>>(subSkills);
        }

        [Authorize(SkillDevelopmentPermissions.Skills.Create)]
        public async Task<SkillDto> CreateAsync(CreateSkillDto input)
        {
            var skill = new Skill(
                GuidGenerator.Create(),
                CurrentUser.Id.Value,
                input.Title,
                input.TargetProficiency,
                input.EstimatedPracticeHours,
                CurrentTenant.Id,
                input.ParentId,
                input.IsCategory);

            if (!string.IsNullOrWhiteSpace(input.Description))
            {
                skill.UpdateDescription(input.Description);
            }

            if (!string.IsNullOrWhiteSpace(input.Category))
            {
                skill.SetCategory(input.Category);
            }

            if (!string.IsNullOrWhiteSpace(input.DevelopmentGoals))
            {
                skill.UpdateDevelopmentGoals(input.DevelopmentGoals);
            }

            // Add prerequisites
            if (input.Prerequisites?.Any() == true)
            {
                foreach (var prereqId in input.Prerequisites)
                {
                    skill.AddPrerequisite(prereqId);
                }
            }

            // Add resources
            if (input.Resources?.Any() == true)
            {
                foreach (var resource in input.Resources)
                {
                    skill.AddResource(resource);
                }
            }

            await _skillRepository.InsertAsync(skill);
            return ObjectMapper.Map<Skill, SkillDto>(skill);
        }

        [Authorize(SkillDevelopmentPermissions.Skills.Edit)]
        public async Task<SkillDto> UpdateAsync(Guid id, UpdateSkillDto input)
        {
            var skill = await _skillRepository.GetAsync(id);

            // Check ownership
            if (skill.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            if (!string.IsNullOrWhiteSpace(input.Description))
            {
                skill.UpdateDescription(input.Description);
            }

            if (!string.IsNullOrWhiteSpace(input.Category))
            {
                skill.SetCategory(input.Category);
            }

            if (!string.IsNullOrWhiteSpace(input.DevelopmentGoals))
            {
                skill.UpdateDevelopmentGoals(input.DevelopmentGoals);
            }

            await _skillRepository.UpdateAsync(skill);
            return ObjectMapper.Map<Skill, SkillDto>(skill);
        }

        [Authorize(SkillDevelopmentPermissions.Skills.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var skill = await _skillRepository.GetAsync(id);

            // Check ownership
            if (skill.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            await _skillRepository.DeleteAsync(id);
        }

        [Authorize(SkillDevelopmentPermissions.Skills.Edit)]
        public async Task<SkillDto> UpdateProficiencyAsync(Guid id, UpdateSkillProficiencyDto input)
        {
            var skill = await _skillRepository.GetAsync(id);

            // Check ownership
            if (skill.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            skill.UpdateProficiency(input.Score, input.Notes, CurrentUser.Id.Value);

            await _skillRepository.UpdateAsync(skill);
            return ObjectMapper.Map<Skill, SkillDto>(skill);
        }

        [Authorize(SkillDevelopmentPermissions.Skills.Edit)]
        public async Task<SkillDto> MarkAsMasteredAsync(Guid id)
        {
            var skill = await _skillRepository.GetAsync(id);

            // Check ownership
            if (skill.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            skill.MarkAsMastered(CurrentUser.Id.Value);

            await _skillRepository.UpdateAsync(skill);
            return ObjectMapper.Map<Skill, SkillDto>(skill);
        }

        [Authorize(SkillDevelopmentPermissions.Skills.Edit)]
        public async Task<SkillDto> ArchiveAsync(Guid id)
        {
            var skill = await _skillRepository.GetAsync(id);

            // Check ownership
            if (skill.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            skill.Archive();

            await _skillRepository.UpdateAsync(skill);
            return ObjectMapper.Map<Skill, SkillDto>(skill);
        }

        [Authorize(SkillDevelopmentPermissions.Skills.ManageSubSkills)]
        public async Task AddPrerequisiteAsync(Guid skillId, Guid prerequisiteId)
        {
            var skill = await _skillRepository.GetAsync(skillId);

            // Check ownership
            if (skill.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            // Validate prerequisite exists and is owned by user
            var prerequisite = await _skillRepository.GetAsync(prerequisiteId);
            if (prerequisite.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:PrerequisiteNotFound");
            }

            skill.AddPrerequisite(prerequisiteId);
            await _skillRepository.UpdateAsync(skill);
        }

        [Authorize(SkillDevelopmentPermissions.Skills.ManageSubSkills)]
        public async Task RemovePrerequisiteAsync(Guid skillId, Guid prerequisiteId)
        {
            var skill = await _skillRepository.GetAsync(skillId);

            // Check ownership
            if (skill.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            skill.RemovePrerequisite(prerequisiteId);
            await _skillRepository.UpdateAsync(skill);
        }

        public async Task<SkillProgressDto> GetProgressAsync(Guid id)
        {
            var skill = await _skillRepository.GetAsync(id);

            // Check ownership
            if (skill.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            return new SkillProgressDto
            {
                SkillId = skill.Id,
                Title = skill.Title,
                CurrentProficiency = skill.CurrentProficiency.Score,
                ProficiencyGainThisWeek = 0, // TODO: Calculate from logs
                TotalPracticeHours = skill.EstimatedPracticeHours,
                LastActivity = skill.LastModificationTime ?? skill.CreationTime,
                NextMilestone = skill.Status.ToString()
            };
        }

        public async Task<List<SkillProgressDto>> GetProgressSummaryAsync()
        {
            var activeSkills = await _skillRepository.GetListAsync(
                new ActiveSkillsSpecification());

            var progressList = new List<SkillProgressDto>();
            foreach (var skill in activeSkills.Where(s => s.UserId == CurrentUser.Id))
            {
                progressList.Add(new SkillProgressDto
                {
                    SkillId = skill.Id,
                    Title = skill.Title,
                    CurrentProficiency = skill.CurrentProficiency.Score,
                    ProficiencyGainThisWeek = 0,
                    TotalPracticeHours = skill.EstimatedPracticeHours,
                    LastActivity = skill.LastModificationTime ?? skill.CreationTime,
                    NextMilestone = skill.Status.ToString()
                });
            }

            return progressList;
        }
    }
}
