using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Analytics.Aggregates;
using NouAssist.Analytics.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Analytics.Repositories
{
    /// <summary>
    /// Implementation of the report configuration repository
    /// </summary>
    public class ReportConfigurationRepository :
        EfCoreRepository<AnalyticsDbContext, ReportConfiguration, Guid>,
        IReportConfigurationRepository
    {
        public ReportConfigurationRepository(IDbContextProvider<AnalyticsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<ReportConfiguration> FindByNameAsync(string name, Guid? tenantId, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(r => r.TenantId == tenantId && r.Name == name)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ReportConfiguration[]> GetByDataSourceAsync(Guid dataSourceId, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(r => r.DataSources.Contains(dataSourceId))
                .ToArrayAsync(cancellationToken);
        }

        public async Task<ReportConfiguration[]> GetScheduledReportsAsync(DateTime beforeTime, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(r => EF.Property<bool>(EF.Property<object>(r, "Schedule"), "IsEnabled") &&
                           EF.Property<DateTime>(EF.Property<object>(r, "Schedule"), "NextRunTime") <= beforeTime)
                .ToArrayAsync(cancellationToken);
        }
        
        public override async Task<IQueryable<ReportConfiguration>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(r => r.Parameters)
                .Include(r => r.GenerationLogs);
        }
    }
}
