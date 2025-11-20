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
    public class EfCoreClientRepository :
        EfCoreRepository<FreelanceOperationsDbContext, FreelanceClient, Guid>,
        IClientRepository
    {
        public EfCoreClientRepository(
            IDbContextProvider<FreelanceOperationsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<FreelanceClient>> GetListAsync(
            Specification<FreelanceClient> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            Specification<FreelanceClient> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .CountAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(
            Specification<FreelanceClient> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .AnyAsync(cancellationToken);
        }

        public override async Task<IQueryable<FreelanceClient>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(c => c.CommunicationRecords);
        }
    }
}
