using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.LearningDevelopment.Integration.Aggregates;
using NouAssist.LearningDevelopment.Integration.Data;
using NouAssist.LearningDevelopment.Integration.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.Integration.Repositories
{
    public class EfCoreLearningPathRepository :
        EfCoreRepository<ILearningDevelopmentDbContext, LearningPath, Guid>,
        ILearningPathRepository
    {
        public EfCoreLearningPathRepository(
            IDbContextProvider<ILearningDevelopmentDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<LearningPath>> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(lp => lp.UserId == userId)
                .OrderByDescending(lp => lp.LastActivity)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<LearningPath>> GetByStatusAsync(
            Guid userId,
            LearningPathStatus status,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(lp => lp.UserId == userId && lp.Status == status)
                .OrderByDescending(lp => lp.LastActivity)
                .ToListAsync(cancellationToken);
        }

        public async Task<LearningPath> FindByTopicIdAsync(
            Guid topicId,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .FirstOrDefaultAsync(lp => lp.TopicId == topicId, cancellationToken);
        }

        public async Task<LearningPath> FindBySkillIdAsync(
            Guid skillId,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .FirstOrDefaultAsync(lp => lp.SkillId == skillId, cancellationToken);
        }

        public async Task<List<LearningPath>> GetActivePathsAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(lp => lp.UserId == userId && lp.Status == LearningPathStatus.Active)
                .OrderByDescending(lp => lp.LastActivity)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<LearningPath>> GetInMaintenancePhaseAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(lp => lp.UserId == userId &&
                             lp.CurrentPhase == LearningPathPhase.Maintenance)
                .OrderByDescending(lp => lp.LastActivity)
                .ToListAsync(cancellationToken);
        }
    }
}
