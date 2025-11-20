using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.Infrastructure.Activity.Aggregates;
using NouAssist.Infrastructure.Activity.Data;
using NouAssist.Infrastructure.Activity.EntityHistory.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;

namespace NouAssist.Infrastructure.Activity.EntityHistory
{
    public class EntityHistoryAppService : ApplicationService, IEntityHistoryAppService
    {
        private readonly IEntityHistoryRepository _entityHistoryRepository;
        private readonly IAsyncQueryableExecuter _asyncExecuter;

        public EntityHistoryAppService(
            IEntityHistoryRepository entityHistoryRepository,
            IAsyncQueryableExecuter asyncExecuter)
        {
            _entityHistoryRepository = entityHistoryRepository;
            _asyncExecuter = asyncExecuter;
        }

        public async Task<EntityHistoryDto> GetAsync(Guid id)
        {
            var entityHistory = await _entityHistoryRepository.GetAsync(id);
            return ObjectMapper.Map<Aggregates.EntityHistory, EntityHistoryDto>(entityHistory);
        }

        public async Task<List<EntityHistoryDto>> GetEntityHistoryAsync(GetEntityHistoryListDto input)
        {
            var queryable = await _entityHistoryRepository.GetQueryableAsync();
            var items = await _entityHistoryRepository.GetEntityHistoryAsync(input.EntityType, input.EntityId);

            return ObjectMapper.Map<List<Aggregates.EntityHistory>, List<EntityHistoryDto>>(items);
        }

        public async Task<EntityHistoryDto> GetLatestVersionAsync(string entityType, Guid entityId)
        {
            var latestVersion = await _entityHistoryRepository.GetLatestVersionAsync(entityType, entityId);
            
            return ObjectMapper.Map<Aggregates.EntityHistory, EntityHistoryDto>(latestVersion);
        }
    }
}