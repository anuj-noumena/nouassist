using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of the IDashboardTemplateRepository
    /// </summary>
    public class EfCoreDashboardTemplateRepository : 
        EfCoreRepository<IAnalyticsReportingDbContext, DashboardTemplate, Guid>,
        IDashboardTemplateRepository
    {
        public EfCoreDashboardTemplateRepository(IDbContextProvider<IAnalyticsReportingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <summary>
        /// Gets system templates
        /// </summary>
        public async Task<List<DashboardTemplate>> GetSystemTemplatesAsync()
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.DashboardTemplates
                .Where(t => t.IsSystem)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Gets templates by category
        /// </summary>
        public async Task<List<DashboardTemplate>> GetTemplatesByCategoryAsync(string category, Guid? tenantId = null)
        {
            var dbContext = await GetDbContextAsync();
            
            // Since Categories is stored as a comma-separated string,
            // we need to do some string manipulation to find templates by category
            return await dbContext.DashboardTemplates
                .Where(t => (t.TenantId == tenantId || t.IsSystem) && 
                            (t.Categories != null && 
                            (t.Categories.Contains(category + ",") || 
                             t.Categories.Contains("," + category) || 
                             t.Categories.Contains("," + category + ",") || 
                             t.Categories.Equals(category))))
                .OrderBy(t => t.Name)
                .ToListAsync();
        }
    }
}
