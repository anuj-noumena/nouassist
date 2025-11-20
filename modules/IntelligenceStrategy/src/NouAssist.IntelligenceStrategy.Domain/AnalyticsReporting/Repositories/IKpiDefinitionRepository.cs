using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories
{
    /// <summary>
    /// Repository for KPI definition aggregates
    /// </summary>
    public interface IKpiDefinitionRepository : IRepository<KpiDefinition, Guid>
    {
        /// <summary>
        /// Checks if a KPI with the given name exists
        /// </summary>
        Task<bool> ExistsByNameAsync(string name, Guid? tenantId = null);
        
        /// <summary>
        /// Gets a KPI by name
        /// </summary>
        Task<KpiDefinition> GetByNameAsync(string name, Guid? tenantId = null);
        
        /// <summary>
        /// Gets KPIs by category
        /// </summary>
        Task<List<KpiDefinition>> GetByCategoryAsync(string category, Guid? tenantId = null);
        
        /// <summary>
        /// Gets KPIs by status
        /// </summary>
        Task<List<KpiDefinition>> GetByStatusAsync(KpiStatus status, Guid? tenantId = null);
    }
}
