using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories
{
    /// <summary>
    /// Repository for aggregated data views
    /// </summary>
    public interface IAggregatedDataViewRepository : IRepository<AggregatedDataView, Guid>
    {
        /// <summary>
        /// Checks if a data view with the given name exists
        /// </summary>
        Task<bool> ExistsByNameAsync(string name, Guid? tenantId = null);
        
        /// <summary>
        /// Gets a data view by name
        /// </summary>
        Task<AggregatedDataView> GetByNameAsync(string name, Guid? tenantId = null);
        
        /// <summary>
        /// Gets data views using a specific source data source
        /// </summary>
        Task<List<AggregatedDataView>> GetBySourceDataSourceAsync(Guid dataSourceId, Guid? tenantId = null);
        
        /// <summary>
        /// Gets data views that need refreshing
        /// </summary>
        Task<List<AggregatedDataView>> GetDataViewsForRefreshAsync(int maxResultCount = 10);
    }
}
