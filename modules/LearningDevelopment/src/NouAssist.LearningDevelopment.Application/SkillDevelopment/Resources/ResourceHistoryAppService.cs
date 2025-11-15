using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment.Data;
using NouAssist.SkillDevelopment.Permissions;
using NouAssist.SkillDevelopment.Resources.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.SkillDevelopment.Resources
{
    [Authorize(SkillDevelopmentPermissions.Resources.Default)]
    public class ResourceHistoryAppService : ApplicationService, IResourceHistoryAppService
    {
        private readonly IResourceHistoryRepository _resourceHistoryRepository;
        private readonly ISkillRepository _skillRepository;

        public ResourceHistoryAppService(
            IResourceHistoryRepository resourceHistoryRepository,
            ISkillRepository skillRepository)
        {
            _resourceHistoryRepository = resourceHistoryRepository;
            _skillRepository = skillRepository;
        }

        [Authorize(SkillDevelopmentPermissions.Resources.View)]
        public async Task<ResourceHistoryDto> GetAsync(Guid id)
        {
            var resource = await _resourceHistoryRepository.GetAsync(id);

            if (resource.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            return await MapToDto(resource);
        }

        [Authorize(SkillDevelopmentPermissions.Resources.View)]
        public async Task<PagedResultDto<ResourceHistoryDto>> GetListAsync(GetResourceHistoryListDto input)
        {
            var queryable = await _resourceHistoryRepository.GetQueryableAsync();

            queryable = queryable.Where(r => r.UserId == CurrentUser.Id.Value);

            if (input.Type.HasValue)
            {
                queryable = queryable.Where(r => r.Type == input.Type.Value);
            }

            if (input.CompletedOnly.HasValue)
            {
                if (input.CompletedOnly.Value)
                {
                    queryable = queryable.Where(r => r.Progress >= 100);
                }
                else
                {
                    queryable = queryable.Where(r => r.Progress < 100);
                }
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderByDescending(r => r.AccessedAt)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var resources = await AsyncExecuter.ToListAsync(queryable);

            var dtos = new List<ResourceHistoryDto>();
            foreach (var resource in resources)
            {
                dtos.Add(await MapToDto(resource));
            }

            return new PagedResultDto<ResourceHistoryDto>(totalCount, dtos);
        }

        [Authorize(SkillDevelopmentPermissions.Resources.Track)]
        public async Task<ResourceHistoryDto> CreateAsync(CreateResourceHistoryDto input)
        {
            // Verify all related skills belong to user
            if (input.RelatedSkillIds?.Any() == true)
            {
                foreach (var skillId in input.RelatedSkillIds)
                {
                    var skill = await _skillRepository.GetAsync(skillId);
                    if (skill.UserId != CurrentUser.Id)
                    {
                        throw new BusinessException("SkillDevelopment:UnauthorizedAccess")
                            .WithData("SkillId", skillId);
                    }
                }
            }

            var resource = new ResourceHistory(
                GuidGenerator.Create(),
                CurrentUser.Id.Value,
                input.ResourceId,
                input.Title,
                input.Type,
                CurrentTenant.Id);

            if (!string.IsNullOrWhiteSpace(input.SourceUrl))
            {
                resource.SetSourceUrl(input.SourceUrl);
            }

            if (input.RelatedSkillIds?.Any() == true)
            {
                foreach (var skillId in input.RelatedSkillIds)
                {
                    resource.LinkToSkill(skillId);
                }
            }

            await _resourceHistoryRepository.InsertAsync(resource);
            return await MapToDto(resource);
        }

        [Authorize(SkillDevelopmentPermissions.Resources.Track)]
        public async Task<ResourceHistoryDto> UpdateProgressAsync(Guid id, UpdateResourceProgressDto input)
        {
            var resource = await _resourceHistoryRepository.GetAsync(id);

            if (resource.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            resource.UpdateProgress(input.Progress);

            await _resourceHistoryRepository.UpdateAsync(resource);
            return await MapToDto(resource);
        }

        [Authorize(SkillDevelopmentPermissions.Resources.Track)]
        public async Task DeleteAsync(Guid id)
        {
            var resource = await _resourceHistoryRepository.GetAsync(id);

            if (resource.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            await _resourceHistoryRepository.DeleteAsync(id);
        }

        [Authorize(SkillDevelopmentPermissions.Resources.Track)]
        public async Task LinkToSkillAsync(Guid resourceId, Guid skillId)
        {
            var resource = await _resourceHistoryRepository.GetAsync(resourceId);

            if (resource.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            // Verify skill ownership
            var skill = await _skillRepository.GetAsync(skillId);
            if (skill.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            resource.LinkToSkill(skillId);
            await _resourceHistoryRepository.UpdateAsync(resource);
        }

        [Authorize(SkillDevelopmentPermissions.Resources.Track)]
        public async Task UnlinkFromSkillAsync(Guid resourceId, Guid skillId)
        {
            var resource = await _resourceHistoryRepository.GetAsync(resourceId);

            if (resource.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            resource.UnlinkFromSkill(skillId);
            await _resourceHistoryRepository.UpdateAsync(resource);
        }

        private async Task<ResourceHistoryDto> MapToDto(ResourceHistory resource)
        {
            var dto = ObjectMapper.Map<ResourceHistory, ResourceHistoryDto>(resource);
            
            // Load skill titles
            var skillTitles = new List<string>();
            foreach (var skillId in resource.RelatedSkillIds)
            {
                var skill = await _skillRepository.FindAsync(skillId);
                if (skill != null)
                {
                    skillTitles.Add(skill.Title);
                }
            }
            dto.RelatedSkillTitles = skillTitles;

            return dto;
        }
    }
}
