using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of the IDataSourceRepository
    /// </summary>
    public class EfCoreDataSourceRepository : 
        EfCoreRepository<IAnalyticsReportingDbContext, DataSource, Guid>,
        IDataSourceRepository
    {
        public EfCoreDataSourceRepository(IDbContextProvider<IAnalyticsReportingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <summary>
        /// Checks if a data source with the given name exists
        /// </summary>
        public async Task<bool> ExistsByNameAsync(string name, Guid? tenantId = null)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.DataSources
                .AnyAsync(d => d.Name == name && d.TenantId == tenantId);
        }

        /// <summary>
        /// Gets a data source by name
        /// </summary>
        public async Task<DataSource> GetByNameAsync(string name, Guid? tenantId = null)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.DataSources
                .Include(d => d.TransformationSteps)
                .FirstOrDefaultAsync(d => d.Name == name && d.TenantId == tenantId);
        }

        /// <summary>
        /// Gets data sources by type
        /// </summary>
        public async Task<List<DataSource>> GetByTypeAsync(DataSourceType type, Guid? tenantId = null)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.DataSources
                .Where(d => d.Type == type && d.TenantId == tenantId)
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Gets data sources that need refreshing
        /// </summary>
        public async Task<List<DataSource>> GetDataSourcesForRefreshAsync(int maxResultCount = 10)
        {
            var dbContext = await GetDbContextAsync();
            var now = DateTime.UtcNow;
            
            return await dbContext.DataSources
                .Where(d => d.IsActive && 
                            (d.LastRefreshed == null || 
                             now.Subtract(d.LastRefreshed.Value).TotalSeconds > 
                             // Use EF Core's value object property access
                             EF.Property<int>(d, "RefreshInterval_Value") * 
                             (EF.Property<string>(d, "RefreshInterval_Unit") == "minutes" ? 60 : 
                              EF.Property<string>(d, "RefreshInterval_Unit") == "hours" ? 3600 : 
                              EF.Property<string>(d, "RefreshInterval_Unit") == "days" ? 86400 : 1)))
                .OrderBy(d => d.LastRefreshed)
                .Take(maxResultCount)
                .ToListAsync();
        }

        /// <summary>
        /// Overriding GetAsync to include required navigation properties
        /// </summary>
        public override async Task<DataSource> GetAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            if (!includeDetails)
            {
                return await base.GetAsync(id, includeDetails, cancellationToken);
            }

            var dbContext = await GetDbContextAsync();
            return await dbContext.DataSources
                .Include(d => d.TransformationSteps)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken: cancellationToken);
        }
    }
}
