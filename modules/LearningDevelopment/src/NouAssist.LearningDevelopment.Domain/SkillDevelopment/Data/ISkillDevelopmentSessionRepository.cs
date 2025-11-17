using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.LearningDevelopment.SkillDevelopment.Data
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
