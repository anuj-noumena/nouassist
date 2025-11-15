using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment.Data;
using NouAssist.SkillDevelopment.Permissions;
using NouAssist.SkillDevelopment.SkillLogs.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.SkillDevelopment.SkillLogs
{
    [Authorize(SkillDevelopmentPermissions.SkillLogs.Default)]
    public class SkillLogAppService : ApplicationService, ISkillLogAppService
    {
        private readonly ISkillLogRepository _skillLogRepository;
        private readonly ISkillRepository _skillRepository;

        public SkillLogAppService(
            ISkillLogRepository skillLogRepository,
            ISkillRepository skillRepository)
        {
            _skillLogRepository = skillLogRepository;
            _skillRepository = skillRepository;
        }

        public async Task<SkillLogDto> GetAsync(Guid id)
        {
            var log = await _skillLogRepository.GetAsync(id);
            
            if (log.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            return await MapToDto(log);
        }

        public async Task<PagedResultDto<SkillLogDto>> GetListAsync(GetSkillLogListDto input)
        {
            var queryable = await _skillLogRepository.GetQueryableAsync();

            queryable = queryable.Where(l => l.UserId == CurrentUser.Id.Value);

            if (input.SkillId.HasValue)
            {
                queryable = queryable.Where(l => l.SkillId == input.SkillId.Value);
            }

            if (input.Type.HasValue)
            {
                queryable = queryable.Where(l => l.Type == input.Type.Value);
            }

            if (input.FromDate.HasValue)
            {
                queryable = queryable.Where(l => l.CreationTime >= input.FromDate.Value);
            }

            if (input.ToDate.HasValue)
            {
                queryable = queryable.Where(l => l.CreationTime <= input.ToDate.Value);
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderByDescending(l => l.CreationTime)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var logs = await AsyncExecuter.ToListAsync(queryable);

            var dtos = new System.Collections.Generic.List<SkillLogDto>();
            foreach (var log in logs)
            {
                dtos.Add(await MapToDto(log));
            }

            return new PagedResultDto<SkillLogDto>(totalCount, dtos);
        }

        [Authorize(SkillDevelopmentPermissions.SkillLogs.Create)]
        public async Task<SkillLogDto> CreateAsync(CreateSkillLogDto input)
        {
            // Verify skill belongs to user
            var skill = await _skillRepository.GetAsync(input.SkillId);
            if (skill.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            var log = new SkillLog(
                GuidGenerator.Create(),
                CurrentUser.Id.Value,
                input.SkillId,
                input.Type,
                input.Content,
                CurrentTenant.Id);

            if (input.ProficiencyScore.HasValue)
            {
                log.SetProficiencyScore(input.ProficiencyScore.Value);
            }

            if (input.Metadata != null)
            {
                foreach (var meta in input.Metadata)
                {
                    log.AddMetadata(meta.Key, meta.Value);
                }
            }

            if (input.Tags?.Any() == true)
            {
                foreach (var tag in input.Tags)
                {
                    log.AddTag(tag);
                }
            }

            await _skillLogRepository.InsertAsync(log);
            return await MapToDto(log);
        }

        [Authorize(SkillDevelopmentPermissions.SkillLogs.Edit)]
        public async Task<SkillLogDto> UpdateAsync(Guid id, UpdateSkillLogDto input)
        {
            var log = await _skillLogRepository.GetAsync(id);

            if (log.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            if (!string.IsNullOrWhiteSpace(input.Content))
            {
                log.UpdateContent(input.Content);
            }

            if (input.ProficiencyScore.HasValue)
            {
                log.SetProficiencyScore(input.ProficiencyScore.Value);
            }

            await _skillLogRepository.UpdateAsync(log);
            return await MapToDto(log);
        }

        [Authorize(SkillDevelopmentPermissions.SkillLogs.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var log = await _skillLogRepository.GetAsync(id);

            if (log.UserId != CurrentUser.Id)
            {
                throw new BusinessException("SkillDevelopment:UnauthorizedAccess");
            }

            await _skillLogRepository.DeleteAsync(id);
        }

        private async Task<SkillLogDto> MapToDto(SkillLog log)
        {
            var dto = ObjectMapper.Map<SkillLog, SkillLogDto>(log);
            
            var skill = await _skillRepository.GetAsync(log.SkillId);
            dto.SkillTitle = skill.Title;

            return dto;
        }
    }
}
