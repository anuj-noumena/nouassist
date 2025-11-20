using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.LearningDevelopment.Integration.Aggregates;
using NouAssist.LearningDevelopment.Integration.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.Integration.Repositories
{
    public class EfCoreLearningActivityRepository :
        EfCoreRepository<ILearningDevelopmentDbContext, LearningActivity, Guid>,
        ILearningActivityRepository
    {
        public EfCoreLearningActivityRepository(
            IDbContextProvider<ILearningDevelopmentDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<LearningActivity> FindByLearningPathIdAsync(
            Guid learningPathId,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .FirstOrDefaultAsync(la => la.LearningPathId == learningPathId, cancellationToken);
        }

        public async Task<List<LearningActivity>> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(la => la.UserId == userId)
                .OrderByDescending(la => la.LastSyncAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<LearningActivity>> GetActiveActivitiesAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(la => la.UserId == userId && la.OverallProgress < 100)
                .OrderByDescending(la => la.LastSyncAt)
                .ToListAsync(cancellationToken);
        }
    }
}
