using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Learning.Enums;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using NouAssist.LearningDevelopment.Learning.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.Learning.Repositories
{
    public class EfCoreLearningNoteRepository 
        : EfCoreRepository<ILearningDbContext, LearningNote, Guid>, ILearningNoteRepository
    {
        public EfCoreLearningNoteRepository(
            IDbContextProvider<ILearningDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<LearningNote>> GetByTopicIdAsync(
            Guid topicId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(n => n.TopicId == topicId)
                .OrderByDescending(n => n.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<LearningNote>> GetByTypeAsync(
            Guid userId,
            LearningNoteType type,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(n => n.UserId == userId && n.Type == type)
                .OrderByDescending(n => n.CreationTime)
                .ToListAsync(cancellationToken);
        }
    }
}
