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
    /// Implementation of the dashboard repository
    /// </summary>
    public class DashboardRepository :
        EfCoreRepository<AnalyticsDbContext, Dashboard, Guid>,
        IDashboardRepository
    {
        public DashboardRepository(IDbContextProvider<AnalyticsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Dashboard> FindByNameAsync(string name, Guid? tenantId, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(d => d.TenantId == tenantId && d.Name == name)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Dashboard> GetDefaultDashboardAsync(Guid? tenantId, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(d => d.TenantId == tenantId && d.IsDefault)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task UnsetDefaultDashboardsExceptAsync(Guid dashboardId, Guid? tenantId, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            // Raw SQL approach for better performance on larger tables
            await dbContext.Database.ExecuteSqlRawAsync(
                "UPDATE \"AnalyticsDashboards\" SET \"IsDefault\" = false " +
                "WHERE \"TenantId\" = {0} AND \"Id\" != {1} AND \"IsDefault\" = true",
                tenantId, dashboardId);
        }
        
        public override async Task<IQueryable<Dashboard>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(d => d.Widgets);
        }
    }
}
