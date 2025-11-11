using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.Reviews.Data;
using NouAssist.WorkManagement.Reviews.Entities;
using NouAssist.WorkManagement.Reviews.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.Reviews.EntityFrameworkCore.Repositories
{
    public class EfCoreReviewItemRepository 
        : EfCoreRepository<IReviewsDbContext, ReviewItem, Guid>, IReviewItemRepository
    {
        public EfCoreReviewItemRepository(
            IDbContextProvider<IReviewsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<ReviewItem>> GetByReviewIdAsync(
            Guid reviewId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(i => i.ReviewId == reviewId)
                .OrderBy(i => i.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ReviewItem>> GetUnreviewedItemsAsync(
            Guid reviewId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(i => i.ReviewId == reviewId && !i.IsReviewed)
                .OrderBy(i => i.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ReviewItem>> GetByTypeAsync(
            Guid reviewId,
            ReviewItemType itemType,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(i => i.ReviewId == reviewId && i.ItemType == itemType)
                .OrderBy(i => i.CreationTime)
                .ToListAsync(cancellationToken);
        }
    }
}
