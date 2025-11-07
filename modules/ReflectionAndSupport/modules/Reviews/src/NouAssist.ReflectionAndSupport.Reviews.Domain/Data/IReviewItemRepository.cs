using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.ReflectionAndSupport.Reviews.Entities;
using NouAssist.ReflectionAndSupport.Reviews.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.ReflectionAndSupport.Reviews.Data
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
