using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core repository implementation for DueDiligenceDataRoom aggregate
    /// </summary>
    public class EfCoreDueDiligenceDataRoomRepository 
        : EfCoreRepository<IIRMDbContext, DueDiligenceDataRoom, Guid>, IDueDiligenceDataRoomRepository
    {
        public EfCoreDueDiligenceDataRoomRepository(
            IDbContextProvider<IIRMDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<DueDiligenceDataRoom>> GetAccessibleDataRoomsAsync(
            Guid investorId,
            CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync();
            return await queryable
                .Where(dr => dr.IsActive && 
                           (dr.ExpiryDate == null || dr.ExpiryDate > DateTime.UtcNow) &&
                           dr.InvestorAccess.ContainsKey(investorId))
                .OrderBy(dr => dr.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<DueDiligenceDataRoom> GetWithDocumentsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var queryable = await WithDetailsAsync();
            return await queryable
                .Where(dr => dr.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<DueDiligenceDataRoom>> GetActiveDataRoomsAsync(
            CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync();
            return await queryable
                .Where(dr => dr.IsActive && (dr.ExpiryDate == null || dr.ExpiryDate > DateTime.UtcNow))
                .OrderBy(dr => dr.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<DueDiligenceDataRoom>> GetExpiringDataRoomsAsync(
            DateTime beforeDate,
            CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync();
            return await queryable
                .Where(dr => dr.IsActive && 
                           dr.ExpiryDate.HasValue && 
                           dr.ExpiryDate <= beforeDate)
                .OrderBy(dr => dr.ExpiryDate)
                .ToListAsync(cancellationToken);
        }

        // Override to include related entities by default
        public override async Task<IQueryable<DueDiligenceDataRoom>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(dr => dr.Documents);
        }

        protected override IQueryable<DueDiligenceDataRoom> ApplyDataFilters(IQueryable<DueDiligenceDataRoom> query)
        {
            return base.ApplyDataFilters(query);
        }
    }
}
