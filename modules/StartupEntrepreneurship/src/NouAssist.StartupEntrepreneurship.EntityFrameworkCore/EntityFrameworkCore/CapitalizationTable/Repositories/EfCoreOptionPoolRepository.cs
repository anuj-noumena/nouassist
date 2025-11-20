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
    /// EF Core implementation of IOptionPoolRepository
    /// </summary>
    public class EfCoreOptionPoolRepository 
        : EfCoreRepository<ICapTableDbContext, OptionPool, Guid>, IOptionPoolRepository
    {
        public EfCoreOptionPoolRepository(
            IDbContextProvider<ICapTableDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<OptionPool>> GetByCapTableIdAsync(
            Guid capTableId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(op => op.CapTableId == capTableId)
                .OrderByDescending(op => op.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<OptionPool>> GetActivePoolsAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(op => op.IsActive)
                .ToListAsync(cancellationToken);
        }

        public async Task<OptionPool> FindByNameAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .FirstOrDefaultAsync(op => op.Name == name, cancellationToken);
        }

        public async Task<List<OptionPool>> GetPoolsWithAvailableOptionsAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(op => op.IsActive && 
                           op.TotalReserved.Value > op.TotalGranted.Value)
                .ToListAsync(cancellationToken);
        }
    }
}
