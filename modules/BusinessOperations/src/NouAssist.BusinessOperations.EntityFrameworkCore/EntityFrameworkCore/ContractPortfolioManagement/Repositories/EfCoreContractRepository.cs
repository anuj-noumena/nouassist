using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Contract;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Repositories;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Specifications;
using NouAssist.ContractPortfolioManagement.Aggregates.Contract;
using NouAssist.ContractPortfolioManagement.Entities.Contract;
using NouAssist.ContractPortfolioManagement.EntityFrameworkCore;
using NouAssist.ContractPortfolioManagement.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.ContractPortfolioManagement.Infrastructure.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of IContractRepository
    /// </summary>
    public class EfCoreContractRepository : 
        EfCoreRepository<ContractPortfolioManagementDbContext, ContractAggregate, Guid>,
        IContractRepository
    {
        public EfCoreContractRepository(
            IDbContextProvider<ContractPortfolioManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<ContractAggregate>> GetListAsync(
            ContractSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<ContractAggregate> GetWithVersionsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Contracts
                .Include(c => c.Versions)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<ContractAggregate> GetWithAmendmentsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Contracts
                .Include(c => c.Amendments)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<ContractAggregate> GetWithAllDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Contracts
                .Include(c => c.Versions)
                .Include(c => c.Clauses)
                .Include(c => c.Parties)
                .Include(c => c.Amendments)
                .Include(c => c.Alerts)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<ContractAggregate> GetByReferenceNumberAsync(
            string referenceNumber,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Contracts
                .Where(c => c.ReferenceNumber == referenceNumber && c.TenantId == tenantId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<ContractAggregate>> GetWithDueAlertsAsync(
            DateTime currentDate,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            // This query is simplified and might need optimization in real world scenarios
            // Ideally, we would check alert conditions directly in SQL
            var contracts = await dbContext.Contracts
                .Include(c => c.Alerts)
                .Where(c => c.TenantId == tenantId)
                .ToListAsync(cancellationToken);

            // Filter contracts with due alerts in memory
            // In a real implementation, we might want to create a more efficient query
            return contracts
                .Where(c => c.Alerts.Any(a => a.ShouldTrigger(currentDate)))
                .ToList();
        }

        public override async Task<IQueryable<ContractAggregate>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(c => c.Versions)
                .Include(c => c.Clauses)
                .Include(c => c.Parties)
                .Include(c => c.Amendments)
                .Include(c => c.Alerts);
        }
    }
}
