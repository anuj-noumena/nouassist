using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Core.Aggregates;
using NouAssist.Core.Data;
using NouAssist.Core.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Core.Logs
{
    [Authorize]
    public class LogActivityAppService : ApplicationService, ILogActivityAppService
    {
        private readonly ILogActivityRepository _logRepository;

        public LogActivityAppService(ILogActivityRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<PagedResultDto<LogActivityDto>> GetMyActivitiesAsync(GetLogActivityListDto input)
        {
            var userId = CurrentUser.GetId();
            var queryable = await _logRepository.GetQueryableAsync();
            
            var query = queryable.Where(l => l.UserId == userId);

            if (input.FromDate.HasValue)
                query = query.Where(l => l.OccurredAt >= input.FromDate.Value);

            if (input.ToDate.HasValue)
                query = query.Where(l => l.OccurredAt <= input.ToDate.Value);

            if (!string.IsNullOrWhiteSpace(input.EntityType))
                query = query.Where(l => l.EntityType == input.EntityType);

            var totalCount = await AsyncExecuter.CountAsync(query);
            
            query = query
                .OrderBy(input.Sorting ?? "OccurredAt DESC")
                .PageBy(input.SkipCount, input.MaxResultCount);

            var logs = await AsyncExecuter.ToListAsync(query);
            var dtos = ObjectMapper.Map<List<LogActivity>, List<LogActivityDto>>(logs);

            return new PagedResultDto<LogActivityDto>(totalCount, dtos);
        }

        public async Task<ListResultDto<LogActivityDto>> GetByEntityAsync(string entityType, Guid entityId)
        {
            var logs = await _logRepository.GetByEntityAsync(entityType, entityId);
            var dtos = ObjectMapper.Map<List<LogActivity>, List<LogActivityDto>>(logs);
            return new ListResultDto<LogActivityDto>(dtos);
        }

        public async Task<LogActivityDto> CreateAsync(CreateLogActivityDto input)
        {
            var userId = CurrentUser.GetId();
            
            var log = new LogActivity(
                GuidGenerator.Create(),
                userId,
                input.Action
            );

            if (!string.IsNullOrWhiteSpace(input.EntityType))
                log.EntityType = input.EntityType;

            if (input.EntityId.HasValue)
                log.EntityId = input.EntityId;

            if (!string.IsNullOrWhiteSpace(input.Description))
                log.Description = input.Description;

            await _logRepository.InsertAsync(log);
            return ObjectMapper.Map<LogActivity, LogActivityDto>(log);
        }
    }
}
