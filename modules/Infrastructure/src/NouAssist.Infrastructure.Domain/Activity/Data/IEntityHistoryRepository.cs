using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Infrastructure.Activity.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Infrastructure.Activity.Data
{
    public interface IEntityHistoryRepository : IRepository<EntityHistory, Guid>
    {
        Task<List<EntityHistory>> GetEntityHistoryAsync(string entityType, Guid entityId, int maxResultCount = 50);
        Task<EntityHistory> GetLatestVersionAsync(string entityType, Guid entityId);
        Task<int> GetNextVersionNumberAsync(string entityType, Guid entityId);
    }
}
