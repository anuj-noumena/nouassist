using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.LearningDevelopment.SkillDevelopment.Data
{
    public interface ISkillLogRepository : IRepository<SkillLog, Guid>
    {
        Task<List<SkillLog>> GetBySkillIdAsync(
            Guid skillId,
            SkillLogType? type = null,
            CancellationToken cancellationToken = default);
        
        Task<List<SkillLog>> GetRecentByUserIdAsync(
            Guid userId,
            int count = 10,
            CancellationToken cancellationToken = default);
    }
}
