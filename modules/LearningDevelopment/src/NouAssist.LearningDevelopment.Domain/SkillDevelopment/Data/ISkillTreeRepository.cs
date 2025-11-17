using System;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.LearningDevelopment.SkillDevelopment.Data
{
    public interface ISkillTreeRepository : IRepository<SkillTree, Guid>
    {
        Task<SkillTree> GetBySkillIdAsync(
            Guid skillId,
            CancellationToken cancellationToken = default);
    }
}
