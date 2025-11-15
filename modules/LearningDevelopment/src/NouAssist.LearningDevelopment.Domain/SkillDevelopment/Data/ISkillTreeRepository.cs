using System;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.SkillDevelopment.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.SkillDevelopment.Data
{
    public interface ISkillTreeRepository : IRepository<SkillTree, Guid>
    {
        Task<SkillTree> GetBySkillIdAsync(
            Guid skillId,
            CancellationToken cancellationToken = default);
    }
}
