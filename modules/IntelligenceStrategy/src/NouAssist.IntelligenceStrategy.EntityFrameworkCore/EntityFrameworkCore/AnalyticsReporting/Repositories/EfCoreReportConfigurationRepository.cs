using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of the IReportConfigurationRepository
    /// </summary>
    public class EfCoreReportConfigurationRepository : 
        EfCoreRepository<IAnalyticsReportingDbContext, ReportConfiguration, Guid>,
        IReportConfigurationRepository
    {
        public EfCoreReportConfigurationRepository(IDbContextProvider<IAnalyticsReportingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <summary>
        /// Checks if a report with the given name exists
        /// </summary>
        public async Task<bool> ExistsByNameAsync(string name, Guid? tenantId = null)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.ReportConfigurations
                .AnyAsync(r => r.Name == name && r.TenantId == tenantId);
        }

        /// <summary>
        /// Gets a report by name
        /// </summary>
        public async Task<ReportConfiguration> GetByNameAsync(string name, Guid? tenantId = null)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.ReportConfigurations
                .Include(r => r.Parameters)
                .Include(r => r.Schedules)
                .FirstOrDefaultAsync(r => r.Name == name && r.TenantId == tenantId);
        }

        /// <summary>
        /// Gets reports by category
        /// </summary>
        public async Task<List<ReportConfiguration>> GetByCategoryAsync(string category, Guid? tenantId = null)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.ReportConfigurations
                .Where(r => r.Category == category && r.TenantId == tenantId)
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Overriding GetAsync to include required navigation properties
        /// </summary>
        public override async Task<ReportConfiguration> GetAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            if (!includeDetails)
            {
                return await base.GetAsync(id, includeDetails, cancellationToken);
            }

            var dbContext = await GetDbContextAsync();
            return await dbContext.ReportConfigurations
                .Include(r => r.Parameters)
                .Include(r => r.Schedules)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken: cancellationToken);
        }
    }
}
