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
    public class EfCoreBookReadingHistoryRepository 
        : EfCoreRepository<ILearningDbContext, BookReadingHistory, Guid>, IBookReadingHistoryRepository
    {
        public EfCoreBookReadingHistoryRepository(
            IDbContextProvider<ILearningDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<BookReadingHistory>> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.Timestamp)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<BookReadingHistory>> GetByTopicIdAsync(
            Guid topicId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(h => h.TopicId == topicId)
                .OrderByDescending(h => h.Timestamp)
                .ToListAsync(cancellationToken);
        }

        public async Task<BookReadingHistory> GetByBookIdAsync(
            Guid userId,
            string bookId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .FirstOrDefaultAsync(
                    h => h.UserId == userId && h.BookId == bookId,
                    cancellationToken);
        }
    }
}
