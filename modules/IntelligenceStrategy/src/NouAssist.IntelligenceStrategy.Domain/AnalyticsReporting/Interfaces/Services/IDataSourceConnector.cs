using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Analytics.Aggregates;
using NouAssist.Analytics.Entities;
using NouAssist.Analytics.Enums;
using NouAssist.Analytics.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.Analytics.Interfaces.Services
{
    /// <summary>
    /// Service interface for data source connection and transformation operations
    /// </summary>
    public interface IDataSourceConnector : IDomainService
    {
        /// <summary>
        /// Creates a new data source configuration
        /// </summary>
        Task<DataSource> CreateDataSourceAsync(
            string name,
            string description,
            DataSourceType type,
            DataConnectionString connectionString,
            DataSourceCredentials credentials,
            Guid? tenantId);
        
        /// <summary>
        /// Updates an existing data source configuration
        /// </summary>
        Task<DataSource> UpdateDataSourceAsync(
            Guid dataSourceId,
            string name,
            string description,
            DataConnectionString connectionString,
            DataSourceCredentials credentials);
        
        /// <summary>
        /// Adds a transformation step to a data source
        /// </summary>
        Task<DataTransformationStep> AddTransformationStepAsync(
            Guid dataSourceId,
            string name,
            string transformationType,
            string configuration,
            int order);
        
        /// <summary>
        /// Removes a transformation step from a data source
        /// </summary>
        Task RemoveTransformationStepAsync(
            Guid dataSourceId,
            Guid stepId);
        
        /// <summary>
        /// Reorders transformation steps for a data source
        /// </summary>
        Task ReorderTransformationStepsAsync(
            Guid dataSourceId,
            Guid stepId,
            int newOrder);
        
        /// <summary>
        /// Adds an aggregated data view to a data source
        /// </summary>
        Task<AggregatedDataView> AddAggregatedDataViewAsync(
            Guid dataSourceId,
            string name,
            string viewDefinition,
            string refreshStrategy);
        
        /// <summary>
        /// Gets a data source by ID
        /// </summary>
        Task<DataSource> GetDataSourceAsync(Guid dataSourceId);
        
        /// <summary>
        /// Tests connectivity to a data source
        /// </summary>
        Task<bool> TestConnectionAsync(Guid dataSourceId);
        
        /// <summary>
        /// Enables or disables a data source
        /// </summary>
        Task SetDataSourceActiveAsync(
            Guid dataSourceId,
            bool isActive);
        
        /// <summary>
        /// Refreshes an aggregated data view
        /// </summary>
        Task RefreshAggregatedDataViewAsync(
            Guid dataSourceId,
            Guid viewId);
    }
}
