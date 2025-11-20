using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories
{
    /// <summary>
    /// Repository for dashboard templates
    /// </summary>
    public interface IDashboardTemplateRepository : IRepository<DashboardTemplate, Guid>
    {
        /// <summary>
        /// Gets system templates
        /// </summary>
        Task<List<DashboardTemplate>> GetSystemTemplatesAsync();
        
        /// <summary>
        /// Gets templates by category
        /// </summary>
        Task<List<DashboardTemplate>> GetTemplatesByCategoryAsync(string category, Guid? tenantId = null);
    }
}
