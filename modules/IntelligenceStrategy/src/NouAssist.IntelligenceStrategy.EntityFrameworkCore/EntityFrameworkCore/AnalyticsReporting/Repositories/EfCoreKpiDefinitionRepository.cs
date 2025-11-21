using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of the IKpiDefinitionRepository
    /// </summary>
    public class EfCoreKpiDefinitionRepository : 
        EfCoreRepository<IAnalyticsReportingDbContext, KpiDefinition, Guid>,
        IKpiDefinitionRepository
    {
        public EfCoreKpiDefinitionRepository(IDbContextProvider<IAnalyticsReportingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <summary>
        /// Checks if a KPI with the given name exists
        /// </summary>
        public async Task<bool> ExistsByNameAsync(string name, Guid? tenantId = null)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.KpiDefinitions
                .AnyAsync(k => k.Name == name && k.TenantId == tenantId);
        }

        /// <summary>
        /// Gets a KPI by name
        /// </summary>
        public async Task<KpiDefinition> GetByNameAsync(string name, Guid? tenantId = null)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.KpiDefinitions
                .Include(k => k.Thresholds)
                .FirstOrDefaultAsync(k => k.Name == name && k.TenantId == tenantId);
        }

        /// <summary>
        /// Gets KPIs by category
        /// </summary>
        public async Task<List<KpiDefinition>> GetByCategoryAsync(string category, Guid? tenantId = null)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.KpiDefinitions
                .Where(k => k.Category == category && k.TenantId == tenantId)
                .OrderBy(k => k.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Gets KPIs by status
        /// </summary>
        public async Task<List<KpiDefinition>> GetByStatusAsync(KpiStatus status, Guid? tenantId = null)
        {
            var dbContext = await GetDbContextAsync();
            
            // Using EF.Property to access nested value object property
            return await dbContext.KpiDefinitions
                .Where(k => k.TenantId == tenantId && 
                            k.IsActive &&
                            EF.Property<string>(k, "CurrentValue_Status") == status.ToString())
                .OrderBy(k => k.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Overriding GetAsync to include required navigation properties
        /// </summary>
        public override async Task<KpiDefinition> GetAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            if (!includeDetails)
            {
                return await base.GetAsync(id, includeDetails, cancellationToken);
            }

            var dbContext = await GetDbContextAsync();
            return await dbContext.KpiDefinitions
                .Include(k => k.Thresholds)
                .FirstOrDefaultAsync(k => k.Id == id, cancellationToken: cancellationToken);
        }
    }
}
