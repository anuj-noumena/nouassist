using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.SkillDevelopment.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.SkillDevelopment.Data
{
    public interface IResourceHistoryRepository : IRepository<ResourceHistory, Guid>
    {
        Task<List<ResourceHistory>> GetByUserIdAsync(
            Guid userId,
            bool? completedOnly = null,
            CancellationToken cancellationToken = default);
        
        Task<List<ResourceHistory>> GetBySkillIdAsync(
            Guid skillId,
            CancellationToken cancellationToken = default);
    }
}
