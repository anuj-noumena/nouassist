using System;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.Reviews.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Reviews.Data
{
    public interface IReviewProgressRepository : IRepository<ReviewProgress, Guid>
    {
        Task<ReviewProgress> GetByReviewIdAsync(
            Guid reviewId,
            CancellationToken cancellationToken = default
        );

        Task<ReviewProgress> GetLatestForReviewAsync(
            Guid reviewId,
            CancellationToken cancellationToken = default
        );
    }
}
