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
    public class EfCoreProposalRepository :
        EfCoreRepository<FreelanceOperationsDbContext, Proposal, Guid>,
        IProposalRepository
    {
        public EfCoreProposalRepository(
            IDbContextProvider<FreelanceOperationsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<Proposal>> GetProposalsByClientAsync(
            Guid clientId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(p => p.ClientId == clientId)
                .OrderByDescending(p => p.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Proposal>> GetProposalsByProjectAsync(
            Guid projectId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(p => p.ProjectId == projectId)
                .OrderByDescending(p => p.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Proposal>> GetListAsync(
            Specification<Proposal> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            Specification<Proposal> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .CountAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(
            Specification<Proposal> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .AnyAsync(cancellationToken);
        }

        public override async Task<IQueryable<Proposal>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(p => p.Versions);
        }
    }
}
