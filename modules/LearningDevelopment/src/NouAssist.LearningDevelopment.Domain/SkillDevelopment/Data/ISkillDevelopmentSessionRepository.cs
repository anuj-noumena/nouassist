using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.SkillDevelopment.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.SkillDevelopment.Data
{
    public interface ISkillDevelopmentSessionRepository : IRepository<SkillDevelopmentSession, Guid>
    {
        Task<SkillDevelopmentSession> GetBySkillIdAsync(
            Guid skillId,
            CancellationToken cancellationToken = default);
        
        Task<List<SkillDevelopmentSession>> GetActiveSessionsByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
