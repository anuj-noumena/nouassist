using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.Reviews.Entities;
using NouAssist.Reviews.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Reviews.Data
{
    public interface IReviewItemRepository : IRepository<ReviewItem, Guid>
    {
        Task<List<ReviewItem>> GetByReviewIdAsync(
            Guid reviewId,
            CancellationToken cancellationToken = default
        );

        Task<List<ReviewItem>> GetUnreviewedItemsAsync(
            Guid reviewId,
            CancellationToken cancellationToken = default
        );

        Task<List<ReviewItem>> GetByTypeAsync(
            Guid reviewId,
            ReviewItemType itemType,
            CancellationToken cancellationToken = default
        );
    }
}
