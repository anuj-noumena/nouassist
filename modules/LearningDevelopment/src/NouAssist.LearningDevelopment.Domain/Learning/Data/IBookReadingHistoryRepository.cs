using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.Learning.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Learning.Data
{
    public interface IBookReadingHistoryRepository : IRepository<BookReadingHistory, Guid>
    {
        Task<List<BookReadingHistory>> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
        
        Task<List<BookReadingHistory>> GetByTopicIdAsync(
            Guid topicId,
            CancellationToken cancellationToken = default);
        
        Task<BookReadingHistory> GetByBookIdAsync(
            Guid userId,
            string bookId,
            CancellationToken cancellationToken = default);
    }
}
