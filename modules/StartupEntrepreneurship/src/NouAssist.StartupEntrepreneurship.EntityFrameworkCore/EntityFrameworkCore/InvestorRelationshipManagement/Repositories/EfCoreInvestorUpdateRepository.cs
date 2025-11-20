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
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core repository implementation for InvestorUpdate aggregate
    /// </summary>
    public class EfCoreInvestorUpdateRepository 
        : EfCoreRepository<IIRMDbContext, InvestorUpdate, Guid>, IInvestorUpdateRepository
    {
        public EfCoreInvestorUpdateRepository(
            IDbContextProvider<IIRMDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<InvestorUpdate>> GetListAsync(
            Specification<InvestorUpdate> specification,
            CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync();
            return await queryable
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<InvestorUpdate>> GetScheduledUpdatesAsync(
            DateTime cutoffDate,
            CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync();
            return await queryable
                .Where(u => u.Status == UpdateStatus.Scheduled && u.ScheduledDate <= cutoffDate)
                .OrderBy(u => u.ScheduledDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<InvestorUpdate>> GetByFrequencyAsync(
            UpdateFrequency frequency,
            CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync();
            return await queryable
                .Where(u => u.Frequency == frequency)
                .OrderByDescending(u => u.SentDate ?? u.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<InvestorUpdate> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var queryable = await WithDetailsAsync();
            return await queryable
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<InvestorUpdate>> GetEngagementReportAsync(
            DateTime fromDate,
            DateTime toDate,
            CancellationToken cancellationToken = default)
        {
            var queryable = await WithDetailsAsync();
            return await queryable
                .Where(u => u.SentDate >= fromDate && u.SentDate <= toDate)
                .OrderByDescending(u => u.SentDate)
                .ToListAsync(cancellationToken);
        }

        // Override to include related entities by default
        public override async Task<IQueryable<InvestorUpdate>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(u => u.Sections)
                .Include(u => u.Metrics)
                .Include(u => u.DistributionLogs)
                .Include(u => u.EngagementRecords);
        }

        protected override IQueryable<InvestorUpdate> ApplyDataFilters(IQueryable<InvestorUpdate> query)
        {
            return base.ApplyDataFilters(query);
        }
    }
}
