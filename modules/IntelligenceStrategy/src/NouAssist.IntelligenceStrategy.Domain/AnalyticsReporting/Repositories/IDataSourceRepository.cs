using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories
{
    /// <summary>
    /// Repository for data source aggregates
    /// </summary>
    public interface IDataSourceRepository : IRepository<DataSource, Guid>
    {
        /// <summary>
        /// Checks if a data source with the given name exists
        /// </summary>
        Task<bool> ExistsByNameAsync(string name, Guid? tenantId = null);
        
        /// <summary>
        /// Gets a data source by name
        /// </summary>
        Task<DataSource> GetByNameAsync(string name, Guid? tenantId = null);
        
        /// <summary>
        /// Gets data sources by type
        /// </summary>
        Task<List<DataSource>> GetByTypeAsync(DataSourceType type, Guid? tenantId = null);
        
        /// <summary>
        /// Gets data sources that need refreshing
        /// </summary>
        Task<List<DataSource>> GetDataSourcesForRefreshAsync(int maxResultCount = 10);
    }
}
