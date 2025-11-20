using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Repositories;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Specifications.Base;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of ICapTableRepository
    /// </summary>
    public class EfCoreCapTableRepository 
        : EfCoreRepository<ICapTableDbContext, Aggregates.CapTable, Guid>, ICapTableRepository
    {
        public EfCoreCapTableRepository(
            IDbContextProvider<ICapTableDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<Aggregates.CapTable>> GetListAsync(
            CapTableSpecification<Aggregates.CapTable> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<Aggregates.CapTable> FindByCompanyNameAsync(
            string companyName,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .FirstOrDefaultAsync(ct => ct.CompanyName == companyName, cancellationToken);
        }

        public async Task<Aggregates.CapTable> GetWithTransactionsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Include("_transactions") // Include private field collection
                .FirstOrDefaultAsync(ct => ct.Id == id, cancellationToken);
        }

        public async Task<Aggregates.CapTable> GetWithValuationHistoryAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Include("_valuationHistory") // Include private field collection
                .FirstOrDefaultAsync(ct => ct.Id == id, cancellationToken);
        }

        public async Task<int> CountAsync(
            CapTableSpecification<Aggregates.CapTable> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .CountAsync(cancellationToken);
        }

        // Override to include default navigation properties when needed
        public override async Task<IQueryable<Aggregates.CapTable>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include("_transactions")
                .Include("_valuationHistory");
        }
    }
}
