using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using NouAssist.LearningDevelopment.SkillDevelopment.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.SkillDevelopment.Repositories
{
    public class EfCoreResourceHistoryRepository 
        : EfCoreRepository<ILearningDevelopmentDbContext, ResourceHistory, Guid>, 
          IResourceHistoryRepository
    {
        public EfCoreResourceHistoryRepository(
            IDbContextProvider<ILearningDevelopmentDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<ResourceHistory>> GetByUserIdAsync(
            Guid userId,
            bool? completedOnly = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<ResourceHistory> query = await GetQueryableAsync();

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
            IQueryable<ResourceHistory> query = await GetQueryableAsync();

            // Load all records and filter in memory since RelatedSkillIds is a collection
            // This is necessary because querying JSON arrays in EF Core requires provider-specific functions
            var allRecords = await query
                .OrderByDescending(r => r.AccessedAt)
                .ToListAsync(cancellationToken);

            // Filter in memory for records that contain the skillId
            return allRecords
                .Where(r => r.RelatedSkillIds.Contains(skillId))
                .ToList();
        }
    }
}
