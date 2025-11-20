using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.DataSources.Dtos;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Exceptions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Permissions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Services;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.DataSources
{
    /// <summary>
    /// Application service implementation for data source management
    /// </summary>
    [Authorize(AnalyticsReportingPermissions.DataSource.Default)]
    public class DataSourceAppService : ApplicationService, IDataSourceAppService
    {
        private readonly IDataSourceManager _dataSourceManager;
        private readonly IDataSourceRepository _dataSourceRepository;
        private readonly IAggregatedDataViewRepository _dataViewRepository;
        
        public DataSourceAppService(
            IDataSourceManager dataSourceManager,
            IDataSourceRepository dataSourceRepository,
            IAggregatedDataViewRepository dataViewRepository)
        {
            _dataSourceManager = dataSourceManager;
            _dataSourceRepository = dataSourceRepository;
            _dataViewRepository = dataViewRepository;
            
            // Set resource for localization
            LocalizationResource = typeof(AnalyticsReportingResource);
        }
        
        /// <summary>
        /// Gets a data source by ID
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.DataSource.View)]
        public virtual async Task<DataSourceDto> GetAsync(Guid id)
        {
            try
            {
                var dataSource = await _dataSourceRepository.GetAsync(id);
                
                // Check tenant
                if (dataSource.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                return ObjectMapper.Map<DataSource, DataSourceDto>(dataSource);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Gets a paginated list of data sources
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.DataSource.View)]
        public virtual async Task<PagedResultDto<DataSourceDto>> GetListAsync(GetDataSourceListDto input)
        {
            try
            {
                // Get queryable repository to apply filters
                var queryable = await _dataSourceRepository.GetQueryableAsync();
                
                // Apply filters
                queryable = await ApplyListFiltersAsync(queryable, input);
                
                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(queryable);
                
                // Apply sorting and paging
                queryable = queryable
                    .OrderBy(input.Sorting ?? nameof(DataSource.Name))
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount);
                
                // Get data sources
                var dataSources = await AsyncExecuter.ToListAsync(queryable);
                
                // Map to DTOs
                var dataSourceDtos = ObjectMapper.Map<List<DataSource>, List<DataSourceDto>>(dataSources);
                
                return new PagedResultDto<DataSourceDto>(totalCount, dataSourceDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Creates a new data source
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.DataSource.Create)]
        public virtual async Task<DataSourceDto> CreateAsync(CreateDataSourceDto input)
        {
            try
            {
                // Map value objects from input
                var refreshInterval = ObjectMapper.Map<CreateDataSourceDto, RefreshInterval>(input);
                var connectionString = input.ConnectionString != null ? 
                    ObjectMapper.Map<CreateDataSourceDto, DataConnectionString>(input) : null;
                var credentials = input.Credentials != null ? 
                    ObjectMapper.Map<CreateDataSourceDto, DataSourceCredentials>(input) : null;
                
                // Create data source using domain service
                var dataSource = await _dataSourceManager.CreateDataSourceAsync(
                    GuidGenerator.Create(),
                    input.Name,
                    input.Description,
                    input.Type,
                    refreshInterval,
                    input.Configuration,
                    connectionString,
                    credentials,
                    input.SchemaDefinition,
                    input.IsActive,
                    CurrentTenant.Id
                );
                
                return ObjectMapper.Map<DataSource, DataSourceDto>(dataSource);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Updates an existing data source
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.DataSource.Update)]
        public virtual async Task<DataSourceDto> UpdateAsync(Guid id, UpdateDataSourceDto input)
        {
            try
            {
                // Get the data source
                var dataSource = await _dataSourceRepository.GetAsync(id);
                
                // Check tenant
                if (dataSource.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Update basic properties
                dataSource.SetName(input.Name);
                dataSource.SetDescription(input.Description);
                dataSource.UpdateConfiguration(input.Configuration);
                dataSource.UpdateSchemaDefinition(input.SchemaDefinition);
                dataSource.SetActive(input.IsActive);
                dataSource.SetTags(input.Tags);
                
                // Update connection string if provided
                if (input.ConnectionString != null)
                {
                    var connectionString = ObjectMapper.Map<UpdateDataSourceDto, DataConnectionString>(input);
                    dataSource.UpdateConnectionString(connectionString);
                }
                
                // Update refresh interval
                var refreshInterval = ObjectMapper.Map<UpdateDataSourceDto, RefreshInterval>(input);
                dataSource.UpdateRefreshInterval(refreshInterval);
                
                // Save changes
                await _dataSourceRepository.UpdateAsync(dataSource);
                
                return ObjectMapper.Map<DataSource, DataSourceDto>(dataSource);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Deletes a data source
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.DataSource.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                // Get the data source
                var dataSource = await _dataSourceRepository.GetAsync(id);
                
                // Check tenant
                if (dataSource.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Check if used by any aggregated data views
                var usedByDataViews = await _dataViewRepository.GetBySourceDataSourceAsync(id, CurrentTenant.Id);
                if (usedByDataViews.Any())
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:DataSourceInUse"]);
                }
                
                // Delete the data source
                await _dataSourceRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Updates credentials for a data source
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.DataSource.Update)]
        public virtual async Task UpdateCredentialsAsync(Guid id, UpdateCredentialsDto input)
        {
            try
            {
                // Get the data source
                var dataSource = await _dataSourceRepository.GetAsync(id);
                
                // Check tenant
                if (dataSource.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Create credentials value object
                var credentials = ObjectMapper.Map<UpdateCredentialsDto, DataSourceCredentials>(input);
                
                // Update credentials
                dataSource.UpdateCredentials(credentials);
                
                // Save changes
                await _dataSourceRepository.UpdateAsync(dataSource);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Refreshes a data source
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.DataSource.View)]
        public virtual async Task RefreshDataSourceAsync(Guid id)
        {
            try
            {
                // Refresh data source using domain service
                await _dataSourceManager.RefreshDataSourceAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Adds a transformation step to a data source
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.DataSource.ConfigureTransformations)]
        public virtual async Task<DataTransformationStepDto> AddTransformationStepAsync(
            Guid dataSourceId, AddTransformationStepDto input)
        {
            try
            {
                // Map to transformation rule value object
                var transformationRule = ObjectMapper.Map<AddTransformationStepDto, TransformationRule>(input);
                
                // Add transformation step using domain service
                var step = await _dataSourceManager.AddTransformationStepAsync(
                    GuidGenerator.Create(),
                    dataSourceId,
                    transformationRule,
                    input.ExecutionOrder,
                    input.IsEnabled
                );
                
                return ObjectMapper.Map<DataTransformationStep, DataTransformationStepDto>(step);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Updates a transformation step on a data source
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.DataSource.ConfigureTransformations)]
        public virtual async Task<DataTransformationStepDto> UpdateTransformationStepAsync(
            Guid dataSourceId, Guid stepId, UpdateTransformationStepDto input)
        {
            try
            {
                // Get the data source
                var dataSource = await _dataSourceRepository.GetAsync(dataSourceId);
                
                // Check tenant
                if (dataSource.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Find the step
                var step = dataSource.TransformationSteps.FirstOrDefault(s => s.Id == stepId);
                if (step == null)
                {
                    throw new EntityNotFoundException("TransformationStep", stepId);
                }
                
                // Create context for mapping with existing rule
                var context = new Dictionary<string, object>
                {
                    ["ExistingRule"] = step.Rule
                };
                
                // Map to transformation rule value object
                var transformationRule = ObjectMapper.Map<UpdateTransformationStepDto, TransformationRule>(input, context);
                
                // Update the transformation step
                dataSource.UpdateTransformationStep(
                    stepId,
                    transformationRule,
                    input.ExecutionOrder,
                    input.IsEnabled
                );
                
                // Save changes
                await _dataSourceRepository.UpdateAsync(dataSource);
                
                // Get updated step
                var updatedStep = dataSource.TransformationSteps.First(s => s.Id == stepId);
                
                return ObjectMapper.Map<DataTransformationStep, DataTransformationStepDto>(updatedStep);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Removes a transformation step from a data source
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.DataSource.ConfigureTransformations)]
        public virtual async Task DeleteTransformationStepAsync(Guid dataSourceId, Guid stepId)
        {
            try
            {
                // Get the data source
                var dataSource = await _dataSourceRepository.GetAsync(dataSourceId);
                
                // Check tenant
                if (dataSource.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Remove the transformation step
                dataSource.RemoveTransformationStep(stepId);
                
                // Save changes
                await _dataSourceRepository.UpdateAsync(dataSource);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Creates a new aggregated data view
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.DataSource.Create)]
        public virtual async Task<AggregatedDataViewDto> CreateAggregatedDataViewAsync(CreateAggregatedDataViewDto input)
        {
            try
            {
                // Map to refresh interval value object
                var refreshInterval = ObjectMapper.Map<CreateAggregatedDataViewDto, RefreshInterval>(input);
                
                // Create aggregated data view using domain service
                var dataView = await _dataSourceManager.CreateAggregatedDataViewAsync(
                    GuidGenerator.Create(),
                    input.Name,
                    input.Description,
                    input.SourceDataSourceIds,
                    input.AggregationDefinition,
                    refreshInterval,
                    CurrentTenant.Id
                );
                
                // Map to DTO with source data source information
                var dataViewDto = ObjectMapper.Map<AggregatedDataView, AggregatedDataViewDto>(dataView);
                
                // Enrich with source data source information
                dataViewDto.SourceDataSources = await GetDataSourcesInfoAsync(dataView.SourceDataSourceIds);
                
                return dataViewDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Gets an aggregated data view by ID
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.DataSource.View)]
        public virtual async Task<AggregatedDataViewDto> GetAggregatedDataViewAsync(Guid id)
        {
            try
            {
                var dataView = await _dataViewRepository.GetAsync(id);
                
                // Check tenant
                if (dataView.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Map to DTO
                var dataViewDto = ObjectMapper.Map<AggregatedDataView, AggregatedDataViewDto>(dataView);
                
                // Enrich with source data source information
                dataViewDto.SourceDataSources = await GetDataSourcesInfoAsync(dataView.SourceDataSourceIds);
                
                return dataViewDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Gets a paginated list of aggregated data views
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.DataSource.View)]
        public virtual async Task<PagedResultDto<AggregatedDataViewDto>> GetAggregatedDataViewListAsync(GetAggregatedDataViewListDto input)
        {
            try
            {
                // Get queryable repository to apply filters
                var queryable = await _dataViewRepository.GetQueryableAsync();
                
                // Apply filters
                queryable = await ApplyDataViewFiltersAsync(queryable, input);
                
                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(queryable);
                
                // Apply sorting and paging
                queryable = queryable
                    .OrderBy(input.Sorting ?? nameof(AggregatedDataView.Name))
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount);
                
                // Get data views
                var dataViews = await AsyncExecuter.ToListAsync(queryable);
                
                // Map to DTOs
                var dataViewDtos = ObjectMapper.Map<List<AggregatedDataView>, List<AggregatedDataViewDto>>(dataViews);
                
                // Enrich with source data source information
                for (int i = 0; i < dataViews.Count; i++)
                {
                    dataViewDtos[i].SourceDataSources = await GetDataSourcesInfoAsync(dataViews[i].SourceDataSourceIds);
                }
                
                return new PagedResultDto<AggregatedDataViewDto>(totalCount, dataViewDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Updates an aggregated data view
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.DataSource.Update)]
        public virtual async Task<AggregatedDataViewDto> UpdateAggregatedDataViewAsync(Guid id, UpdateAggregatedDataViewDto input)
        {
            try
            {
                // Get the data view
                var dataView = await _dataViewRepository.GetAsync(id);
                
                // Check tenant
                if (dataView.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Update basic properties
                dataView.SetName(input.Name);
                dataView.SetDescription(input.Description);
                dataView.SetSourceDataSources(input.SourceDataSourceIds);
                dataView.UpdateAggregationDefinition(input.AggregationDefinition);
                
                // Update refresh interval
                var refreshInterval = ObjectMapper.Map<UpdateAggregatedDataViewDto, RefreshInterval>(input);
                dataView.SetRefreshInterval(refreshInterval);
                
                // Save changes
                await _dataViewRepository.UpdateAsync(dataView);
                
                // Map to DTO
                var dataViewDto = ObjectMapper.Map<AggregatedDataView, AggregatedDataViewDto>(dataView);
                
                // Enrich with source data source information
                dataViewDto.SourceDataSources = await GetDataSourcesInfoAsync(dataView.SourceDataSourceIds);
                
                return dataViewDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Deletes an aggregated data view
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.DataSource.Delete)]
        public virtual async Task DeleteAggregatedDataViewAsync(Guid id)
        {
            try
            {
                // Get the data view
                var dataView = await _dataViewRepository.GetAsync(id);
                
                // Check tenant
                if (dataView.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Delete the data view
                await _dataViewRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Refreshes an aggregated data view
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.DataSource.View)]
        public virtual async Task RefreshAggregatedDataViewAsync(Guid id)
        {
            try
            {
                // Get the data view
                var dataView = await _dataViewRepository.GetAsync(id);
                
                // Check tenant
                if (dataView.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Mark as refreshed with valid status
                dataView.MarkAsRefreshed(Clock.Now, ValidationStatus.Valid);
                
                // Save changes
                await _dataViewRepository.UpdateAsync(dataView);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Applies filters to data source list query
        /// </summary>
        private async Task<IQueryable<DataSource>> ApplyListFiltersAsync(IQueryable<DataSource> queryable, GetDataSourceListDto input)
        {
            // Filter by active
            if (input.OnlyActive == true)
            {
                queryable = queryable.Where(d => d.IsActive);
            }
            
            // Filter by text
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                queryable = queryable.Where(d => 
                    d.Name.Contains(input.Filter) || 
                    (d.Description != null && d.Description.Contains(input.Filter)));
            }
            
            // Filter by type
            if (!string.IsNullOrWhiteSpace(input.Type) && Enum.TryParse<DataSourceType>(input.Type, true, out var type))
            {
                queryable = queryable.Where(d => d.Type == type);
            }
            
            // Filter by tag
            if (!string.IsNullOrWhiteSpace(input.Tag))
            {
                // This is a simplification - in actual implementation
                // you might need a more complex query to search within the tags array
                queryable = queryable.Where(d => EF.Functions.JsonContains(d.Tags, input.Tag));
            }
            
            // Apply tenant filter
            queryable = queryable.Where(d => d.TenantId == CurrentTenant.Id);
            
            // If transformations are not requested, don't include them in the query
            if (!input.IncludeTransformations)
            {
                queryable = queryable.Include(d => d.TransformationSteps);
            }
            
            return queryable;
        }
        
        /// <summary>
        /// Applies filters to aggregated data view list query
        /// </summary>
        private async Task<IQueryable<AggregatedDataView>> ApplyDataViewFiltersAsync(IQueryable<AggregatedDataView> queryable, GetAggregatedDataViewListDto input)
        {
            // Filter by text
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                queryable = queryable.Where(d => 
                    d.Name.Contains(input.Filter) || 
                    (d.Description != null && d.Description.Contains(input.Filter)));
            }
            
            // Filter by source data source
            if (!string.IsNullOrWhiteSpace(input.SourceDataSourceId) && Guid.TryParse(input.SourceDataSourceId, out var sourceDataSourceId))
            {
                // This is a simplification - in actual implementation 
                // you might need a more complex query to search within the array
                queryable = queryable.Where(d => EF.Functions.JsonContains(d.SourceDataSourceIds, sourceDataSourceId.ToString()));
            }
            
            // Filter by validation status
            if (!string.IsNullOrWhiteSpace(input.ValidationStatus) && Enum.TryParse<ValidationStatus>(input.ValidationStatus, true, out var validationStatus))
            {
                queryable = queryable.Where(d => d.ValidationStatus == validationStatus);
            }
            
            // Apply tenant filter
            queryable = queryable.Where(d => d.TenantId == CurrentTenant.Id);
            
            return queryable;
        }
        
        /// <summary>
        /// Gets data source information for display
        /// </summary>
        private async Task<AggregatedDataSourceDto[]> GetDataSourcesInfoAsync(Guid[] dataSourceIds)
        {
            if (dataSourceIds == null || dataSourceIds.Length == 0)
            {
                return Array.Empty<AggregatedDataSourceDto>();
            }
            
            var result = new List<AggregatedDataSourceDto>();
            
            foreach (var dataSourceId in dataSourceIds)
            {
                try
                {
                    var dataSource = await _dataSourceRepository.GetAsync(dataSourceId);
                    
                    result.Add(new AggregatedDataSourceDto
                    {
                        Id = dataSource.Id,
                        Name = dataSource.Name,
                        Type = dataSource.Type
                    });
                }
                catch
                {
                    // If data source not found, add with unknown name
                    result.Add(new AggregatedDataSourceDto
                    {
                        Id = dataSourceId,
                        Name = "Unknown",
                        Type = DataSourceType.CustomDataSource
                    });
                }
            }
            
            return result.ToArray();
        }
        
        /// <summary>
        /// Error handling helper method
        /// </summary>
        private async Task HandleErrorAsync(Exception ex, LogLevel logLevel = LogLevel.Error)
        {
            // Log the error
            Logger.Log(logLevel, ex, "Error in DataSourceAppService: {Message}", ex.Message);
            
            // Handle specific exceptions
            if (ex is EntityNotFoundException)
            {
                throw new UserFriendlyException(
                    L["AnalyticsReporting:ErrorMessage:EntityNotFound", L["AnalyticsReporting:Entity:DataSource"]],
                    null,
                    ex);
            }
            
            if (ex is AnalyticsReportingDomainException domainException)
            {
                throw new UserFriendlyException(
                    L[domainException.Code, domainException.Message],
                    domainException.Code,
                    ex);
            }
            
            // For other exceptions, throw a generic error
            throw new UserFriendlyException(
                L["AnalyticsReporting:ErrorMessage:GenericError"],
                null,
                ex);
        }
    }
}
