using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.ContractPortfolioManagement.Aggregates.Portfolio;
using NouAssist.ContractPortfolioManagement.Enums;
using NouAssist.ContractPortfolioManagement.EntityFrameworkCore;
using NouAssist.ContractPortfolioManagement.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.ContractPortfolioManagement.Infrastructure.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of IPortfolioRepository
    /// </summary>
    public class EfCorePortfolioRepository : 
        EfCoreRepository<ContractPortfolioManagementDbContext, PortfolioAggregate, Guid>,
        IPortfolioRepository
    {
        public EfCorePortfolioRepository(
            IDbContextProvider<ContractPortfolioManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<PortfolioAggregate>> GetListAsync(
            PortfolioSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<PortfolioAggregate> GetWithItemsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Portfolios
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<PortfolioAggregate> GetWithRisksAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Portfolios
                .Include(p => p.Risks)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<PortfolioAggregate> GetWithAllDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Portfolios
                .Include(p => p.Items)
                .Include(p => p.Categories)
                .Include(p => p.Risks)
                .Include(p => p.ComplianceRecords)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<PortfolioAggregate> GetByNameAsync(
            string name,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Portfolios
                .Where(p => p.Name == name && p.TenantId == tenantId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<PortfolioAggregate>> GetContainingItemAsync(
            Guid itemId,
            PortfolioItemType itemType,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Portfolios
                .Include(p => p.Items)
                .Where(p => p.TenantId == tenantId && 
                            p.Items.Any(i => i.Reference.ItemId == itemId && 
                                             i.Reference.ItemType == itemType))
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<PortfolioAggregate>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(p => p.Items)
                .Include(p => p.Categories)
                .Include(p => p.Risks)
                .Include(p => p.ComplianceRecords);
        }
    }
}
