using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Repositories;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Specifications.Base;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core repository implementation for InvestorProfile aggregate
    /// </summary>
    public class EfCoreInvestorProfileRepository 
        : EfCoreRepository<IIRMDbContext, InvestorProfile, Guid>, IInvestorProfileRepository
    {
        public EfCoreInvestorProfileRepository(
            IDbContextProvider<IIRMDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<InvestorProfile> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync();
            return await queryable
                .Where(ip => ip.ContactDetails.Email == email)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<InvestorProfile>> GetListAsync(
            Specification<InvestorProfile> specification,
            CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync();
            return await queryable
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            Specification<InvestorProfile> specification,
            CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync();
            return await queryable
                .Where(specification.ToExpression())
                .CountAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(
            Specification<InvestorProfile> specification,
            CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync();
            return await queryable
                .Where(specification.ToExpression())
                .AnyAsync(cancellationToken);
        }

        public async Task<List<InvestorProfile>> GetInvestorsByGroupAsync(
            Guid groupId,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            // First get the group to access investor IDs
            var group = await dbContext.InvestorGroups
                .Where(g => g.Id == groupId)
                .FirstOrDefaultAsync(cancellationToken);

            if (group == null || !group.InvestorIds.Any())
                return new List<InvestorProfile>();

            // Then get investors by IDs
            var queryable = await GetQueryableAsync();
            return await queryable
                .Where(ip => group.InvestorIds.Contains(ip.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<InvestorProfile>> GetByIdsAsync(
            List<Guid> ids,
            CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync();
            return await queryable
                .Where(ip => ids.Contains(ip.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsEmailUniqueAsync(
            string email,
            Guid? excludeId = null,
            CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync();
            
            var query = queryable.Where(ip => ip.ContactDetails.Email == email);
            
            if (excludeId.HasValue)
            {
                query = query.Where(ip => ip.Id != excludeId.Value);
            }

            return !await query.AnyAsync(cancellationToken);
        }

        // Override to include related entities by default
        public override async Task<IQueryable<InvestorProfile>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(ip => ip.Interactions)
                .Include(ip => ip.Investments);
        }

        protected override IQueryable<InvestorProfile> ApplyDataFilters(IQueryable<InvestorProfile> query)
        {
            // Apply any additional data filters if needed
            return base.ApplyDataFilters(query);
        }
    }
}
