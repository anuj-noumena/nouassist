using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.BusinessOperations.FreelanceOperations.EntityFrameworkCore.Repositories
{
    public class EfCoreUpworkEngagementRepository :
        EfCoreRepository<FreelanceOperationsDbContext, UpworkEngagement, Guid>,
        IUpworkEngagementRepository
    {
        public EfCoreUpworkEngagementRepository(
            IDbContextProvider<FreelanceOperationsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<UpworkEngagement> GetByUpworkJobIdAsync(
            string upworkJobId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .FirstOrDefaultAsync(e => e.UpworkJobId == upworkJobId, cancellationToken);
        }

        public async Task<UpworkEngagement> GetByUpworkContractIdAsync(
            string upworkContractId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .FirstOrDefaultAsync(e => e.UpworkContractId == upworkContractId, cancellationToken);
        }

        public async Task<List<UpworkEngagement>> GetListAsync(
            Specification<UpworkEngagement> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            Specification<UpworkEngagement> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .CountAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(
            Specification<UpworkEngagement> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .AnyAsync(cancellationToken);
        }
    }
}
