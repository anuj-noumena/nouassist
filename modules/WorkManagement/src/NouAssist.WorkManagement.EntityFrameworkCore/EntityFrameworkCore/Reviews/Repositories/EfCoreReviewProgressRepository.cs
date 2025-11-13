using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.Reviews.Aggregates;
using NouAssist.WorkManagement.Reviews.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore.Reviews.Repositories
{
    public class EfCoreReviewProgressRepository 
        : EfCoreRepository<IReviewsDbContext, ReviewProgress, Guid>, IReviewProgressRepository
    {
        public EfCoreReviewProgressRepository(
            IDbContextProvider<IReviewsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<ReviewProgress> GetByReviewIdAsync(
            Guid reviewId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .FirstOrDefaultAsync(p => p.ReviewId == reviewId, cancellationToken);
        }

        public async Task<ReviewProgress> GetLatestForReviewAsync(
            Guid reviewId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(p => p.ReviewId == reviewId)
                .OrderByDescending(p => p.Timestamp)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
