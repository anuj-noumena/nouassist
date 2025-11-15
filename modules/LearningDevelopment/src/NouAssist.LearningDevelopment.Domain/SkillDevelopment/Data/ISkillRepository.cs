using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.SkillDevelopment.Aggregates;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.SkillDevelopment.Data
{
    public interface ISkillRepository : IRepository<Skill, Guid>
    {
        Task<Skill> GetWithSubSkillsAsync(Guid id, CancellationToken cancellationToken = default);
        
        Task<List<Skill>> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
        
        Task<List<Skill>> GetByParentIdAsync(
            Guid parentId,
            CancellationToken cancellationToken = default);
        
        Task<List<Skill>> GetListAsync(
            ISpecification<Skill> spec,
            CancellationToken cancellationToken = default);
    }
}
