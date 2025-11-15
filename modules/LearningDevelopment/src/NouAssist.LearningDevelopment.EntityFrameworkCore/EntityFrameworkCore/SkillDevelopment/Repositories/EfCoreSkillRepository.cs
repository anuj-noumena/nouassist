using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.SkillDevelopment.EntityFrameworkCore.Repositories
{
    public class EfCoreSkillRepository 
        : EfCoreRepository<ISkillDevelopmentDbContext, Skill, Guid>, ISkillRepository
    {
        public EfCoreSkillRepository(
            IDbContextProvider<ISkillDevelopmentDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Skill> GetWithSubSkillsAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            // Note: SubSkillIds is a collection property, not a navigation property
            // So we just load the skill itself
            return await query.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<List<Skill>> GetByUserIdAsync(
            Guid userId, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(s => s.UserId == userId)
                .OrderBy(s => s.Title)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Skill>> GetByParentIdAsync(
            Guid parentId, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(s => s.ParentId == parentId)
                .OrderBy(s => s.Title)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Skill>> GetListAsync(
            ISpecification<Skill> spec, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(spec.ToExpression())
                .ToListAsync(cancellationToken);
        }
    }
}
