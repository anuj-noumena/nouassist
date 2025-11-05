using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Activity.Aggregates;
using NouAssist.Activity.Data;
using NouAssist.Activity.EntityHistory.Dtos;
using NouAssist.Activity.Permissions;
using Volo.Abp.Application.Services;

namespace NouAssist.Activity.EntityHistory
{
    [Authorize(ActivityPermissions.EntityHistory.View)]
    public class EntityHistoryAppService : ApplicationService, IEntityHistoryAppService
    {
        private readonly IEntityHistoryRepository _entityHistoryRepository;

        public EntityHistoryAppService(IEntityHistoryRepository entityHistoryRepository)
        {
            _entityHistoryRepository = entityHistoryRepository;
        }

        public async Task<EntityHistoryDto> GetAsync(Guid id)
        {
            var history = await _entityHistoryRepository.GetAsync(id);
            return ObjectMapper.Map<Aggregates.EntityHistory, EntityHistoryDto>(history);
        }

        public async Task<List<EntityHistoryDto>> GetEntityHistoryAsync(GetEntityHistoryListDto input)
        {
            var history = await _entityHistoryRepository.GetEntityHistoryAsync(
                input.EntityType,
                input.EntityId,
                input.MaxResultCount
            );

            return ObjectMapper.Map<List<Aggregates.EntityHistory>, List<EntityHistoryDto>>(history);
        }

        public async Task<EntityHistoryDto> GetLatestVersionAsync(string entityType, Guid entityId)
        {
            var latestVersion = await _entityHistoryRepository.GetLatestVersionAsync(entityType, entityId);
            
            if (latestVersion == null)
            {
                return null;
            }

            return ObjectMapper.Map<Aggregates.EntityHistory, EntityHistoryDto>(latestVersion);
        }
    }
}
