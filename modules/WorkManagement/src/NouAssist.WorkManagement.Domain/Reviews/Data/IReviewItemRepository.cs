using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Reviews.Entities;
using NouAssist.WorkManagement.Reviews.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.Reviews.Data
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
