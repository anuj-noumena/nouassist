using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories
{
    /// <summary>
    /// Repository for report configuration aggregates
    /// </summary>
    public interface IReportConfigurationRepository : IRepository<ReportConfiguration, Guid>
    {
        /// <summary>
        /// Checks if a report with the given name exists
        /// </summary>
        Task<bool> ExistsByNameAsync(string name, Guid? tenantId = null);
        
        /// <summary>
        /// Gets a report by name
        /// </summary>
        Task<ReportConfiguration> GetByNameAsync(string name, Guid? tenantId = null);
        
        /// <summary>
        /// Gets reports by category
        /// </summary>
        Task<List<ReportConfiguration>> GetByCategoryAsync(string category, Guid? tenantId = null);
    }
}
