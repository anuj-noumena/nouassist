using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using NouAssist.LearningDevelopment.SkillDevelopment.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.SkillDevelopment.Repositories
{
    public class EfCoreSkillTreeRepository 
        : EfCoreRepository<ILearningDevelopmentDbContext, SkillTree, Guid>, ISkillTreeRepository
    {
        public EfCoreSkillTreeRepository(
            IDbContextProvider<ILearningDevelopmentDbContext> dbContextProvider)
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
