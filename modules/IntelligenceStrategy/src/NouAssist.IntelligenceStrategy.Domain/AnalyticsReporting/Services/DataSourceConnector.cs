using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.Analytics.Aggregates;
using NouAssist.Analytics.Entities;
using NouAssist.Analytics.Enums;
using NouAssist.Analytics.Exceptions;
using NouAssist.Analytics.Interfaces.Services;
using NouAssist.Analytics.Repositories;
using NouAssist.Analytics.ValueObjects;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp.Domain.Services;
using Volo.Abp.Localization;

namespace NouAssist.Analytics.Services
{
    /// <summary>
    /// Implementation of data source connection and transformation operations
    /// </summary>
    public class DataSourceConnector : DomainService, IDataSourceConnector
    {
        private readonly IDataSourceRepository _dataSourceRepository;
        private readonly ILogger<DataSourceConnector> _logger;
        
        public DataSourceConnector(
            IDataSourceRepository dataSourceRepository,
            ILogger<DataSourceConnector> logger)
        {
            _dataSourceRepository = dataSourceRepository;
            _logger = logger;
        }
        
        public async Task<DataSource> CreateDataSourceAsync(
            string name,
            string description,
            DataSourceType type,
            DataConnectionString connectionString,
            DataSourceCredentials credentials,
            Guid? tenantId)
        {
            try
            {
                var existingDataSource = await _dataSourceRepository.FindByNameAsync(name, tenantId);
                if (existingDataSource != null)
                {
                    throw new AnalyticsDomainException("Analytics:ErrorMessage:DuplicateName")
                        .WithData("entityType", L["Analytics:Entity:DataSource"])
                        .WithData("name", name);
                }
                
                var dataSource = new DataSource(
                    GuidGenerator.Create(),
                    name,
                    description,
                    type,
                    connectionString,
                    credentials,
                    tenantId
                );
                
                return await _dataSourceRepository.InsertAsync(dataSource);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create data source with name {Name}", name);
                throw;
            }
        }
        
        public async Task<DataSource> UpdateDataSourceAsync(
            Guid dataSourceId,
            string name,
            string description,
            DataConnectionString connectionString,
            DataSourceCredentials credentials)
        {
            try
            {
                var dataSource = await _dataSourceRepository.GetAsync(dataSourceId);
                
                // Check name uniqueness if it's being changed
                if (dataSource.Name != name)
                {
                    var existingDataSource = await _dataSourceRepository.FindByNameAsync(name, dataSource.TenantId);
                    if (existingDataSource != null && existingDataSource.Id != dataSourceId)
                    {
                        throw new AnalyticsDomainException("Analytics:ErrorMessage:DuplicateName")
                            .WithData("entityType", L["Analytics:Entity:DataSource"])
                            .WithData("name", name);
                    }
                    
                    dataSource.SetName(name);
                }
                
                dataSource.SetDescription(description);
                dataSource.UpdateConnectionString(connectionString);
                dataSource.UpdateCredentials(credentials);
                
                return await _dataSourceRepository.UpdateAsync(dataSource);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update data source with ID {DataSourceId}", dataSourceId);
                throw;
            }
        }
        
        public async Task<DataTransformationStep> AddTransformationStepAsync(
            Guid dataSourceId,
            string name,
            string transformationType,
            string configuration,
            int order)
        {
            try
            {
                var dataSource = await _dataSourceRepository.GetAsync(dataSourceId);
                
                var step = new DataTransformationStep(
                    GuidGenerator.Create(),
                    name,
                    transformationType,
                    configuration,
                    order
                );
                
                dataSource.AddTransformationStep(step);
                
                await _dataSourceRepository.UpdateAsync(dataSource);
                
                return step;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add transformation step to data source {DataSourceId}", dataSourceId);
                throw;
            }
        }
        
        public async Task RemoveTransformationStepAsync(
            Guid dataSourceId,
            Guid stepId)
        {
            try
            {
                var dataSource = await _dataSourceRepository.GetAsync(dataSourceId);
                
                dataSource.RemoveTransformationStep(stepId);
                
                await _dataSourceRepository.UpdateAsync(dataSource);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove transformation step {StepId} from data source {DataSourceId}", 
                                stepId, dataSourceId);
                throw;
            }
        }
        
        public async Task ReorderTransformationStepsAsync(
            Guid dataSourceId,
            Guid stepId,
            int newOrder)
        {
            try
            {
                var dataSource = await _dataSourceRepository.GetAsync(dataSourceId);
                
                dataSource.ReorderTransformationSteps(stepId, newOrder);
                
                await _dataSourceRepository.UpdateAsync(dataSource);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to reorder transformation step {StepId} in data source {DataSourceId}", 
                                stepId, dataSourceId);
                throw;
            }
        }
        
        public async Task<AggregatedDataView> AddAggregatedDataViewAsync(
            Guid dataSourceId,
            string name,
            string viewDefinition,
            string refreshStrategy)
        {
            try
            {
                var dataSource = await _dataSourceRepository.GetAsync(dataSourceId);
                
                var view = new AggregatedDataView(
                    GuidGenerator.Create(),
                    name,
                    viewDefinition,
                    refreshStrategy
                );
                
                dataSource.AddAggregatedDataView(view);
                
                await _dataSourceRepository.UpdateAsync(dataSource);
                
                return view;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add aggregated data view to data source {DataSourceId}", dataSourceId);
                throw;
            }
        }
        
        public async Task<DataSource> GetDataSourceAsync(Guid dataSourceId)
        {
            return await _dataSourceRepository.GetAsync(dataSourceId);
        }
        
        public async Task<bool> TestConnectionAsync(Guid dataSourceId)
        {
            try
            {
                var dataSource = await _dataSourceRepository.GetAsync(dataSourceId);
                
                // In a real implementation, this would actually test the connection
                // based on the data source type and connection details
                
                // For demonstration purposes, we'll just simulate a successful connection
                _logger.LogInformation("Testing connection to data source {DataSourceId}", dataSourceId);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to test connection to data source {DataSourceId}", dataSourceId);
                return false;
            }
        }
        
        public async Task SetDataSourceActiveAsync(
            Guid dataSourceId,
            bool isActive)
        {
            try
            {
                var dataSource = await _dataSourceRepository.GetAsync(dataSourceId);
                
                dataSource.SetActive(isActive);
                
                await _dataSourceRepository.UpdateAsync(dataSource);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update active status for data source {DataSourceId}", dataSourceId);
                throw;
            }
        }
        
        public async Task RefreshAggregatedDataViewAsync(
            Guid dataSourceId,
            Guid viewId)
        {
            try
            {
                var dataSource = await _dataSourceRepository.GetAsync(dataSourceId);
                
                var view = dataSource.AggregatedDataViews.FirstOrDefault(v => v.Id == viewId);
                if (view == null)
                {
                    throw new AnalyticsDomainException("Analytics:ErrorMessage:EntityNotFound")
                        .WithData("entityType", "AggregatedDataView")
                        .WithData("id", viewId);
                }
                
                // In a real implementation, this would perform the actual data refresh
                // For now, we'll just update the last refreshed timestamp
                
                view.RecordRefresh();
                
                await _dataSourceRepository.UpdateAsync(dataSource);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to refresh aggregated data view {ViewId} in data source {DataSourceId}", 
                                viewId, dataSourceId);
                throw;
            }
        }
        
        private string L(string name)
        {
            return LocalizationResource.GetString(Analytics.Localization.AnalyticsResource.ResourceName, name);
        }
    }
}
