using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.ReflectionAndSupport.ActivityTracking.EntityHistory.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.ReflectionAndSupport.ActivityTracking.EntityHistory
{
    public interface IEntityHistoryAppService : IApplicationService
    {
        Task<EntityHistoryDto> GetAsync(Guid id);
        Task<List<EntityHistoryDto>> GetEntityHistoryAsync(GetEntityHistoryListDto input);
        Task<EntityHistoryDto> GetLatestVersionAsync(string entityType, Guid entityId);
    }
}
