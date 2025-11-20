using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories
{
    /// <summary>
    /// Repository for dashboard aggregates
    /// </summary>
    public interface IDashboardRepository : IRepository<Dashboard, Guid>
    {
        /// <summary>
        /// Checks if a dashboard with the given name exists
        /// </summary>
        Task<bool> ExistsByNameAsync(string name, Guid? tenantId = null);
        
        /// <summary>
        /// Gets a dashboard by name
        /// </summary>
        Task<Dashboard> GetByNameAsync(string name, Guid? tenantId = null);
    }
}
