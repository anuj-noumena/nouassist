using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Analytics.Aggregates;
using NouAssist.Analytics.EntityFrameworkCore;
using NouAssist.Analytics.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Analytics.Repositories
{
    /// <summary>
    /// Implementation of the KPI definition repository
    /// </summary>
    public class KpiDefinitionRepository :
        EfCoreRepository<AnalyticsDbContext, KpiDefinition, Guid>,
        IKpiDefinitionRepository
    {
        public KpiDefinitionRepository(IDbContextProvider<AnalyticsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<KpiDefinition> FindByNameAsync(string name, Guid? tenantId, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(k => k.TenantId == tenantId && k.Name == name)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<KpiDefinition[]> GetByDataSourceAsync(Guid dataSourceId, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(k => k.DataSources.Contains(dataSourceId))
                .ToArrayAsync(cancellationToken);
        }

        public async Task<KpiDefinition[]> GetByStatusAsync(KpiStatus status, Guid? tenantId, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(k => k.TenantId == tenantId && k.Status == status)
                .ToArrayAsync(cancellationToken);
        }
        
        public override async Task<IQueryable<KpiDefinition>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(k => k.Thresholds)
                .Include(k => k.AlertLogs);
        }
    }
}
