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
    /// EF Core implementation of ISecurityIssuanceRepository
    /// </summary>
    public class EfCoreSecurityIssuanceRepository 
        : EfCoreRepository<ICapTableDbContext, SecurityIssuance, Guid>, ISecurityIssuanceRepository
    {
        public EfCoreSecurityIssuanceRepository(
            IDbContextProvider<ICapTableDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<SecurityIssuance>> GetListAsync(
            CapTableSpecification<SecurityIssuance> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SecurityIssuance>> GetByCapTableIdAsync(
            Guid capTableId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(si => si.CapTableId == capTableId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SecurityIssuance>> GetBySecurityTypeAsync(
            SecurityType securityType,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(si => si.SecurityType == securityType)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SecurityIssuance>> GetActiveWithAvailableSharesAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(si => si.IsActive && 
                           si.TotalAuthorized.Value > si.TotalIssued.Value)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SecurityIssuance>> GetExpiredOptionsAsync(
            DateTime asOfDate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(si => si.SecurityType == SecurityType.StockOption &&
                           si.OptionDetails != null &&
                           si.OptionDetails.ExpiryDate.HasValue &&
                           si.OptionDetails.ExpiryDate.Value < asOfDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<SecurityIssuance> GetWithShareClassesAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Include(si => si.ShareClasses)
                .FirstOrDefaultAsync(si => si.Id == id, cancellationToken);
        }

        // Override to include default navigation properties when needed
        public override async Task<IQueryable<SecurityIssuance>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(si => si.ShareClasses);
        }
    }
}
