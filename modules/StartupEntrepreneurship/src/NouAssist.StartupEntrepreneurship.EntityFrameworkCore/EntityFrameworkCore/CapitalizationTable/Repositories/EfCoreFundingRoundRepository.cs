using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Repositories;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Specifications.Base;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of IFundingRoundRepository
    /// </summary>
    public class EfCoreFundingRoundRepository 
        : EfCoreRepository<ICapTableDbContext, FundingRound, Guid>, IFundingRoundRepository
    {
        public EfCoreFundingRoundRepository(
            IDbContextProvider<ICapTableDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<FundingRound>> GetListAsync(
            CapTableSpecification<FundingRound> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<FundingRound>> GetByCapTableIdAsync(
            Guid capTableId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(fr => fr.CapTableId == capTableId)
                .OrderByDescending(fr => fr.StartDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<FundingRound>> GetByStatusAsync(
            FundingRoundStatus status,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(fr => fr.Status == status)
                .ToListAsync(cancellationToken);
        }

        public async Task<FundingRound> GetLatestByCapTableIdAsync(
            Guid capTableId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(fr => fr.CapTableId == capTableId)
                .OrderByDescending(fr => fr.StartDate)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<FundingRound>> GetActiveFundingRoundsAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(fr => fr.Status == FundingRoundStatus.Active)
                .ToListAsync(cancellationToken);
        }

        public async Task<FundingRound> FindByNameAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .FirstOrDefaultAsync(fr => fr.Name == name, cancellationToken);
        }
    }
}
