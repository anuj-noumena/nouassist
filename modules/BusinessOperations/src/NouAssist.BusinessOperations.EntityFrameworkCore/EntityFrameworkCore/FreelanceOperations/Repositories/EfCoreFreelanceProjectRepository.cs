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
    public class EfCoreFreelanceProjectRepository :
        EfCoreRepository<FreelanceOperationsDbContext, FreelanceProject, Guid>,
        IFreelanceProjectRepository
    {
        public EfCoreFreelanceProjectRepository(
            IDbContextProvider<FreelanceOperationsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<FreelanceProject>> GetProjectsByClientAsync(
            Guid clientId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(p => p.ClientId == clientId)
                .OrderByDescending(p => p.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<FreelanceProject>> GetListAsync(
            Specification<FreelanceProject> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            Specification<FreelanceProject> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .CountAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(
            Specification<FreelanceProject> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .AnyAsync(cancellationToken);
        }

        public override async Task<IQueryable<FreelanceProject>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(p => p.TimeEntries)
                .Include(p => p.Milestones);
        }
    }
}
