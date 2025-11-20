using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of the IAggregatedDataViewRepository
    /// </summary>
    public class EfCoreAggregatedDataViewRepository : 
        EfCoreRepository<IAnalyticsReportingDbContext, AggregatedDataView, Guid>,
        IAggregatedDataViewRepository
    {
        public EfCoreAggregatedDataViewRepository(IDbContextProvider<IAnalyticsReportingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <summary>
        /// Checks if a data view with the given name exists
        /// </summary>
        public async Task<bool> ExistsByNameAsync(string name, Guid? tenantId = null)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.AggregatedDataViews
                .AnyAsync(v => v.Name == name && v.TenantId == tenantId);
        }

        /// <summary>
        /// Gets a data view by name
        /// </summary>
        public async Task<AggregatedDataView> GetByNameAsync(string name, Guid? tenantId = null)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.AggregatedDataViews
                .FirstOrDefaultAsync(v => v.Name == name && v.TenantId == tenantId);
        }

        /// <summary>
        /// Gets data views using a specific source data source
        /// </summary>
        public async Task<List<AggregatedDataView>> GetBySourceDataSourceAsync(Guid dataSourceId, Guid? tenantId = null)
        {
            var dbContext = await GetDbContextAsync();
            
            // Since SourceDataSourceIds is stored as a comma-separated string of Guids,
            // we need to check if it contains our dataSourceId
            var idString = dataSourceId.ToString();
            
            return await dbContext.AggregatedDataViews
                .Where(v => v.TenantId == tenantId && 
                            (EF.Property<string>(v, "SourceDataSourceIds").Contains(idString + ",") || 
                             EF.Property<string>(v, "SourceDataSourceIds").Contains("," + idString) || 
                             EF.Property<string>(v, "SourceDataSourceIds").Contains("," + idString + ",") || 
                             EF.Property<string>(v, "SourceDataSourceIds").Equals(idString)))
                .OrderBy(v => v.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Gets data views that need refreshing
        /// </summary>
        public async Task<List<AggregatedDataView>> GetDataViewsForRefreshAsync(int maxResultCount = 10)
        {
            var dbContext = await GetDbContextAsync();
            var now = DateTime.UtcNow;
            
            return await dbContext.AggregatedDataViews
                .Where(v => EF.Property<bool>(v, "RefreshInterval_AutoRefreshEnabled") && 
                            now.Subtract(v.LastRefreshed).TotalSeconds > 
                            // Use EF Core's value object property access
                            EF.Property<int>(v, "RefreshInterval_Value") * 
                            (EF.Property<string>(v, "RefreshInterval_Unit") == "minutes" ? 60 : 
                             EF.Property<string>(v, "RefreshInterval_Unit") == "hours" ? 3600 : 
                             EF.Property<string>(v, "RefreshInterval_Unit") == "days" ? 86400 : 1))
                .OrderBy(v => v.LastRefreshed)
                .Take(maxResultCount)
                .ToListAsync();
        }
    }
}
