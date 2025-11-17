using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using NouAssist.LearningDevelopment.Learning.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.Learning.Repositories
{
    public class EfCoreLearningSessionRepository 
        : EfCoreRepository<ILearningDbContext, LearningSession, Guid>, ILearningSessionRepository
    {
        public EfCoreLearningSessionRepository(
            IDbContextProvider<ILearningDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<LearningSession> GetByTopicIdAsync(
            Guid topicId,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.LearningSessions
                .Include(s => s.PhaseMilestones)
                .FirstOrDefaultAsync(s => s.TopicId == topicId, cancellationToken);
        }

        public async Task<List<LearningSession>> GetActiveSessionsAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(s => s.UserId == userId && !s.Completed && !s.Paused)
                .OrderByDescending(s => s.LastActivity)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<LearningSession>> GetPausedSessionsAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(s => s.UserId == userId && s.Paused && !s.Completed)
                .OrderByDescending(s => s.PausedAt)
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<LearningSession>> WithDetailsAsync()
        {
            var dbContext = await GetDbContextAsync();
            
            return dbContext.LearningSessions
                .Include(s => s.PhaseMilestones);
        }

        public override async Task<LearningSession> GetAsync(
            Guid id,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            var query = dbContext.LearningSessions.AsQueryable();
            
            if (includeDetails)
            {
                query = query.Include(s => s.PhaseMilestones);
            }

            return await query.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }
    }
}
