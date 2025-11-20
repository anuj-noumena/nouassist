using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core repository implementation for InvestorGroup entity
    /// </summary>
    public class EfCoreInvestorGroupRepository 
        : EfCoreRepository<IIRMDbContext, InvestorGroup, Guid>, IInvestorGroupRepository
    {
        public EfCoreInvestorGroupRepository(
            IDbContextProvider<IIRMDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<InvestorGroup>> GetActiveGroupsAsync(CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync();
            return await queryable
                .Where(g => g.IsActive)
                .OrderBy(g => g.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<InvestorGroup>> GetGroupsForInvestorAsync(
            Guid investorId,
            CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync();
            return await queryable
                .Where(g => g.IsActive && g.InvestorIds.Contains(investorId))
                .OrderBy(g => g.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsNameUniqueAsync(
            string name,
            Guid? excludeId = null,
            CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync();
            
            var query = queryable.Where(g => g.Name == name);
            
            if (excludeId.HasValue)
            {
                query = query.Where(g => g.Id != excludeId.Value);
            }

            return !await query.AnyAsync(cancellationToken);
        }

        protected override IQueryable<InvestorGroup> ApplyDataFilters(IQueryable<InvestorGroup> query)
        {
            return base.ApplyDataFilters(query);
        }
    }
}
