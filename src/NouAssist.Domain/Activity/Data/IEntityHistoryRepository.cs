using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Activity.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Activity.Data
{
    public interface IEntityHistoryRepository : IRepository<EntityHistory, Guid>
    {
        Task<List<EntityHistory>> GetEntityHistoryAsync(string entityType, Guid entityId, int maxResultCount = 50);
        Task<EntityHistory> GetLatestVersionAsync(string entityType, Guid entityId);
        Task<int> GetNextVersionNumberAsync(string entityType, Guid entityId);
    }
}
