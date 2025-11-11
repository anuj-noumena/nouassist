using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Reviews.Aggregates;
using NouAssist.WorkManagement.Reviews.Enums;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.Reviews.Data
{
    public interface IReviewRepository : IRepository<Review, Guid>
    {
        Task<Review> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default
        );

        Task<List<Review>> GetByOwnerAndTypeAsync(
            Guid ownerId,
            ReviewType reviewType,
            CancellationToken cancellationToken = default
        );

        Task<List<Review>> GetIncompleteReviewsAsync(
            Guid ownerId,
            CancellationToken cancellationToken = default
        );

        Task<Review> GetLatestReviewAsync(
            Guid ownerId,
            ReviewType reviewType,
            CancellationToken cancellationToken = default
        );

        Task<List<Review>> GetListAsync(
            ISpecification<Review> spec,
            CancellationToken cancellationToken = default
        );
    }
}
