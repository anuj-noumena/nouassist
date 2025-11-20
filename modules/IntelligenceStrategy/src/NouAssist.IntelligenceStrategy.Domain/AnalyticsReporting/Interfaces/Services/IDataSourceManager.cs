using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Interfaces.Services
{
    /// <summary>
    /// Interface for data source management operations
    /// </summary>
    public interface IDataSourceManager : IDomainService
    {
        /// <summary>
        /// Creates a new data source
        /// </summary>
        Task<DataSource> CreateDataSourceAsync(
            Guid id,
            string name,
            string description,
            DataSourceType type,
            RefreshInterval refreshInterval,
            string configuration = null,
            DataConnectionString connectionString = null,
            DataSourceCredentials credentials = null,
            string schemaDefinition = null,
            bool isActive = true,
            Guid? tenantId = null);
            
        /// <summary>
        /// Adds a transformation step to a data source
        /// </summary>
        Task<DataTransformationStep> AddTransformationStepAsync(
            Guid stepId,
            Guid dataSourceId,
            TransformationRule rule,
            int executionOrder,
            bool isEnabled = true);
            
        /// <summary>
        /// Creates an aggregated data view
        /// </summary>
        Task<AggregatedDataView> CreateAggregatedDataViewAsync(
            Guid id,
            string name,
            string description,
            Guid[] sourceDataSourceIds,
            string aggregationDefinition,
            RefreshInterval refreshInterval,
            Guid? tenantId = null);
            
        /// <summary>
        /// Refreshes a data source
        /// </summary>
        Task RefreshDataSourceAsync(Guid dataSourceId);
    }
}
