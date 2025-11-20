using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of the IDashboardRepository
    /// </summary>
    public class EfCoreDashboardRepository : 
        EfCoreRepository<IAnalyticsReportingDbContext, Dashboard, Guid>,
        IDashboardRepository
    {
        public EfCoreDashboardRepository(IDbContextProvider<IAnalyticsReportingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <summary>
        /// Checks if a dashboard with the given name exists
        /// </summary>
        public async Task<bool> ExistsByNameAsync(string name, Guid? tenantId = null)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Dashboards
                .AnyAsync(d => d.Name == name && d.TenantId == tenantId);
        }

        /// <summary>
        /// Gets a dashboard by name
        /// </summary>
        public async Task<Dashboard> GetByNameAsync(string name, Guid? tenantId = null)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Dashboards
                .Include(d => d.LayoutSetting)
                .Include(d => d.Widgets)
                .FirstOrDefaultAsync(d => d.Name == name && d.TenantId == tenantId);
        }

        /// <summary>
        /// Overriding GetAsync to include required navigation properties
        /// </summary>
        public override async Task<Dashboard> GetAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            if (!includeDetails)
            {
                return await base.GetAsync(id, includeDetails, cancellationToken);
            }

            var dbContext = await GetDbContextAsync();
            return await dbContext.Dashboards
                .Include(d => d.LayoutSetting)
                .Include(d => d.Widgets)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken: cancellationToken);
        }
    }
}
