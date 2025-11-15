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

namespace NouAssist.SkillDevelopment.EntityFrameworkCore.Repositories
{
    public class EfCoreResourceHistoryRepository 
        : EfCoreRepository<ISkillDevelopmentDbContext, ResourceHistory, Guid>, 
          IResourceHistoryRepository
    {
        public EfCoreResourceHistoryRepository(
            IDbContextProvider<ISkillDevelopmentDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<ResourceHistory>> GetByUserIdAsync(
            Guid userId, 
            bool? completedOnly = null, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            query = query.Where(r => r.UserId == userId);
            
            if (completedOnly.HasValue)
            {
                if (completedOnly.Value)
                {
                    query = query.Where(r => r.Progress >= 100);
                }
                else
                {
                    query = query.Where(r => r.Progress < 100);
                }
            }
            
            return await query
                .OrderByDescending(r => r.AccessedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ResourceHistory>> GetBySkillIdAsync(
            Guid skillId, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            // Note: RelatedSkillIds is a JSON array, so we need to use EF.Functions
            // For PostgreSQL with jsonb
            return await query
                .Where(r => EF.Functions.JsonContains(r.RelatedSkillIds, skillId.ToString()))
                .OrderByDescending(r => r.AccessedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
