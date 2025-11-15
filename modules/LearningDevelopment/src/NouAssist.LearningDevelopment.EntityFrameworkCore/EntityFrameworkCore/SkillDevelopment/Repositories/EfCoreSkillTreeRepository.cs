using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.SkillDevelopment.EntityFrameworkCore.Repositories
{
    public class EfCoreSkillTreeRepository 
        : EfCoreRepository<ISkillDevelopmentDbContext, SkillTree, Guid>, ISkillTreeRepository
    {
        public EfCoreSkillTreeRepository(
            IDbContextProvider<ISkillDevelopmentDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<SkillTree> GetBySkillIdAsync(
            Guid skillId, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .FirstOrDefaultAsync(t => t.SkillId == skillId, cancellationToken);
        }
    }
}
