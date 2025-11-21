using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Exceptions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Interfaces.Services;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Localization;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Localization;
using Microsoft.Extensions.Localization;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Services
{
    /// <summary>
    /// Manages operations on data sources
    /// </summary>
    public class DataSourceManager : DomainService, IDataSourceManager
    {
        private readonly IDataSourceRepository _dataSourceRepository;
        private readonly IAggregatedDataViewRepository _dataViewRepository;
        private readonly ILogger<DataSourceManager> _logger;
        
        public DataSourceManager(
            IDataSourceRepository dataSourceRepository,
            IAggregatedDataViewRepository dataViewRepository,
            ILogger<DataSourceManager> logger)
        {
            _dataSourceRepository = dataSourceRepository;
            _dataViewRepository = dataViewRepository;
            _logger = logger;
        }
        
        /// <summary>
        /// Creates a new data source
        /// </summary>
        public async Task<DataSource> CreateDataSourceAsync(
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
            Guid? tenantId = null)
        {
            _logger.LogInformation("Creating data source {Name} of type {Type}", name, type);
            
            // Check for duplicate name
            if (await _dataSourceRepository.ExistsByNameAsync(name, tenantId))
            {
                throw new DuplicateNameException(L["AnalyticsReporting:Entity:DataSource"], name);
            }
            
            // Validate configuration based on type
            ValidateDataSourceConfiguration(type, configuration, connectionString, credentials);
            
            var dataSource = DataSource.Create(
                id,
                name,
                description,
                type,
                refreshInterval,
                configuration,
                connectionString,
                credentials,
                schemaDefinition,
                isActive,
                null,
                tenantId
            );
            
            await _dataSourceRepository.InsertAsync(dataSource);
            
            return dataSource;
        }
        
        /// <summary>
        /// Validates data source configuration based on its type
        /// </summary>
        private void ValidateDataSourceConfiguration(
            DataSourceType type,
            string configuration,
            DataConnectionString connectionString,
            DataSourceCredentials credentials)
        {
            switch (type)
            {
                case DataSourceType.DatabaseView:
                    if (connectionString == null)
                    {
                        throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidDataSourceConfiguration")
                            .WithData("reason", "Connection string is required for database views");
                    }
                    break;
                    
                case DataSourceType.API:
                    if (string.IsNullOrEmpty(configuration))
                    {
                        throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidDataSourceConfiguration")
                            .WithData("reason", "Configuration is required for API data sources");
                    }
                    
                    if (credentials == null)
                    {
                        throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidDataSourceConfiguration")
                            .WithData("reason", "Credentials are required for API data sources");
                    }
                    break;
                    
                case DataSourceType.EventStream:
                    if (string.IsNullOrEmpty(configuration))
                    {
                        throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidDataSourceConfiguration")
                            .WithData("reason", "Configuration is required for event stream data sources");
                    }
                    break;
                    
                // Additional validations for other types
            }
        }
        
        /// <summary>
        /// Adds a transformation step to a data source
        /// </summary>
        public async Task<DataTransformationStep> AddTransformationStepAsync(
            Guid stepId,
            Guid dataSourceId,
            TransformationRule rule,
            int executionOrder,
            bool isEnabled = true)
        {
            var dataSource = await _dataSourceRepository.GetAsync(dataSourceId);
            
            // Make sure rule name is unique
            if (dataSource.TransformationSteps.Any(s => s.Rule.Name == rule.Name))
            {
                throw new DuplicateNameException("TransformationRule", rule.Name);
            }
            
            var step = dataSource.AddTransformationStep(
                stepId,
                rule,
                executionOrder,
                isEnabled
            );
            
            await _dataSourceRepository.UpdateAsync(dataSource);
            
            return step;
        }
        
        /// <summary>
        /// Creates an aggregated data view
        /// </summary>
        public async Task<AggregatedDataView> CreateAggregatedDataViewAsync(
            Guid id,
            string name,
            string description,
            Guid[] sourceDataSourceIds,
            string aggregationDefinition,
            RefreshInterval refreshInterval,
            Guid? tenantId = null)
        {
            _logger.LogInformation("Creating aggregated data view {Name}", name);
            
            // Check for duplicate name
            if (await _dataViewRepository.ExistsByNameAsync(name, tenantId))
            {
                throw new DuplicateNameException("AggregatedDataView", name);
            }
            
            // Validate source data sources
            await ValidateSourceDataSourcesAsync(sourceDataSourceIds, tenantId);
            
            var dataView = new AggregatedDataView(
                id,
                name,
                description,
                sourceDataSourceIds,
                aggregationDefinition,
                refreshInterval,
                DateTime.UtcNow,
                ValidationStatus.NotValidated,
                tenantId
            );
            
            await _dataViewRepository.InsertAsync(dataView);
            
            return dataView;
        }
        
        /// <summary>
        /// Validates that source data sources exist and are accessible
        /// </summary>
        private async Task ValidateSourceDataSourcesAsync(Guid[] sourceDataSourceIds, Guid? tenantId)
        {
            if (sourceDataSourceIds == null || sourceDataSourceIds.Length == 0)
            {
                throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidDataSourceConfiguration")
                    .WithData("reason", "At least one source data source is required");
            }
            
            foreach (var dataSourceId in sourceDataSourceIds)
            {
                var dataSource = await _dataSourceRepository.GetAsync(dataSourceId);
                
                // Check if data source is accessible for this tenant
                if (dataSource.TenantId.HasValue && dataSource.TenantId != tenantId)
                {
                    throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidDataSourceConfiguration")
                        .WithData("reason", $"Data source {dataSource.Name} is not accessible for this tenant");
                }
                
                // Check if data source is active
                if (!dataSource.IsActive)
                {
                    throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidDataSourceConfiguration")
                        .WithData("reason", $"Data source {dataSource.Name} is not active");
                }
            }
        }
        
        /// <summary>
        /// Refreshes a data source
        /// </summary>
        public async Task RefreshDataSourceAsync(Guid dataSourceId)
        {
            var dataSource = await _dataSourceRepository.GetAsync(dataSourceId);
            
            // Skip if not active
            if (!dataSource.IsActive)
            {
                _logger.LogWarning("Attempted to refresh inactive data source {DataSourceId}", dataSourceId);
                return;
            }
            
            try
            {
                // In a real implementation, this would connect to the data source and refresh it
                // For this example, we'll just simulate it
                await Task.Delay(100);
                
                // Mark as refreshed with successful validation
                dataSource.MarkAsRefreshed(DateTime.UtcNow, ValidationStatus.Valid);
                
                await _dataSourceRepository.UpdateAsync(dataSource);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing data source {DataSourceId}", dataSourceId);
                
                // Mark as refreshed with failed validation
                dataSource.MarkAsRefreshed(DateTime.UtcNow, ValidationStatus.ValidationFailed);
                
                await _dataSourceRepository.UpdateAsync(dataSource);
                
                throw new BusinessException("AnalyticsReporting:ErrorMessage:DataConnectionFailed")
                    .WithData("reason", ex.Message);
            }
        }
        
        // Other implementation methods...
        
        private IStringLocalizer<AnalyticsReportingResource> L => LazyServiceProvider.LazyGetRequiredService<IStringLocalizer<AnalyticsReportingResource>>();
    }
}
