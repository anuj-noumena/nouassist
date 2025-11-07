using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.ReflectionAndSupport.Reviews.Aggregates;
using NouAssist.ReflectionAndSupport.Reviews.Data;
using NouAssist.ReflectionAndSupport.Reviews.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.ReflectionAndSupport.Reviews.EntityFrameworkCore.Repositories
{
    public class EfCoreReviewRepository 
        : EfCoreRepository<IReviewsDbContext, Review, Guid>, IReviewRepository
    {
        public EfCoreReviewRepository(
            IDbContextProvider<IReviewsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Review> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Include(r => r.Items)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<List<Review>> GetByOwnerAndTypeAsync(
            Guid ownerId,
            ReviewType reviewType,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(r => r.OwnerId == ownerId && r.ReviewType == reviewType)
                .OrderByDescending(r => r.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Review>> GetIncompleteReviewsAsync(
            Guid ownerId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(r => r.OwnerId == ownerId && !r.IsComplete)
                .OrderBy(r => r.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<Review> GetLatestReviewAsync(
            Guid ownerId,
            ReviewType reviewType,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(r => r.OwnerId == ownerId && r.ReviewType == reviewType)
                .OrderByDescending(r => r.CreationTime)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Review>> GetListAsync(
            ISpecification<Review> spec,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(spec.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<Review>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(r => r.Items);
        }
    }
}
