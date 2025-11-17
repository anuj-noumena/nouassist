using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.Reviews.Aggregates;
using NouAssist.WorkManagement.Reviews.Data;
using NouAssist.WorkManagement.Reviews.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.EntityFrameworkCore.Reviews.Repositories
{
    public class EfCoreReviewRepository 
        : EfCoreRepository<IWorkManagementDbContext, Review, Guid>, IReviewRepository
    {
        public EfCoreReviewRepository(
            IDbContextProvider<IWorkManagementDbContext> dbContextProvider)
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
